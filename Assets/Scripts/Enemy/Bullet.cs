using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, Stopable
{

    public Rigidbody myRigid;
    public float damage = 1f;
    public float bulletSpeed = 1f;

    public bool isStop;

    private float removeTime;

    private void Update() {
        CheckRemoveBullet();

    }

    void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
    }




    private void OnCollisionEnter(Collision other) {
        PlayerStatus _playerStatus = other.collider.GetComponent<PlayerStatus>();
        if(_playerStatus != null){
            _playerStatus.OnDamage(damage, false, other.contacts[0].point, other.contacts[0].normal, other.collider);
        }
        if(other.collider.GetComponent<Bullet>() == null) RemoveBullet();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Shield") RemoveBullet();
    }

    //일정시간후 자동 제거
    public void SetRemoveTime(int _time){
        removeTime = Time.time + _time;
    }

    private void CheckRemoveBullet(){
        if(Time.time > removeTime) RemoveBullet();
    }

    public virtual void RemoveBullet(){
        if(gameObject.activeSelf){
            ObjectPoolingManager.instance.InsertQueue(gameObject, ObjectPoolingManager.instance.bulletQueue);
        }
    }
    //속도 설정
    public virtual void SetVelocity(float _speed){
        bulletSpeed = _speed;
        myRigid.velocity = (Camera.main.transform.position - transform.position).normalized * bulletSpeed;
        transform.up = (Camera.main.transform.position - transform.position).normalized;
        StartCoroutine(SetVelocityCoroutine());
    }

    IEnumerator SetVelocityCoroutine(){
        Vector3 _vel = myRigid.velocity;
        while(gameObject.activeSelf){
            myRigid.velocity = _vel.normalized * bulletSpeed;
            transform.up = myRigid.velocity.normalized;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void Stop(float _time){
        removeTime += _time;
        StartCoroutine(StopBulletCoroutine(_time));
    }

    IEnumerator StopBulletCoroutine(float _time){
        float _speed = bulletSpeed;
        float _startTime = Time.time;
        while(Time.time < _startTime + _time){
            bulletSpeed = 0.1f;
            yield return null;
        }
        bulletSpeed = _speed;
    }
}

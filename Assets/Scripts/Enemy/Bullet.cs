using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Rigidbody myRigid;
    public float damage = 1f;
    public float bulletSpeed = 1f;

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
        Invoke("RemoveBullet", _time);
    }
    public virtual void RemoveBullet(){
        if(gameObject.activeSelf){
            ObjectPoolingManager.instance.InsertQueue(gameObject, ObjectPoolingManager.instance.bulletQueue);
        }
    }
    //속도 설정
    public virtual void SetVelocity(){
        myRigid.velocity = (Camera.main.transform.position - transform.position).normalized * bulletSpeed;
        transform.up = (Camera.main.transform.position - transform.position).normalized;
    }

    // private void OnTriggerEnter(Collider other) {
    //     PlayerStatus1 _playerStatus = other.GetComponent<PlayerStatus1>();
    //     if(_playerStatus != null){
    //         _playerStatus.OnDamage(damage, Vector3.zero, Vector3.zero, other);
    //     }
    // }

}

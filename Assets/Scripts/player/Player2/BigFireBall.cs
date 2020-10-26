using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFireBall : MonoBehaviour
{

    public GameObject fireBall;
    public float ballSpeed;
    
    private Rigidbody myRigid;
    private int ballNum;

    private void Awake() {
        myRigid = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other) {
        Explosion();
    }

    public void SetRemoveTime(int _time){
        Invoke("RemoveBall", _time);
    }
    public virtual void RemoveBall(){
        if(gameObject.activeSelf) Destroy(gameObject);
    }

    public void Set(Vector3 velocity, int _ballNum){
        myRigid.velocity = velocity * ballSpeed;
        ballNum = _ballNum;
    }

    private void Explosion(){
        for(int i=0; i<ballNum; i++){
            CreateFireBall();
        }
        RemoveBall();
    }
    private void CreateFireBall(){
        GameObject _fireBall = Instantiate(fireBall, transform.position, Quaternion.identity);
        FireBall _ball = _fireBall.GetComponent<FireBall>();
        Vector3 _randomVec = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        _ball.SetVelocity(_randomVec, 0.5f);
        _ball.SetRemoveTime(4);

    }
}

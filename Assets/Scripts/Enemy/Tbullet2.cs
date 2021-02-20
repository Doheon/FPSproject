using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tbullet2 : Bullet
{
    private Vector3 startVelocity;
    public override void RemoveBullet(){
        if(gameObject.activeSelf){
            ObjectPoolingManager.instance.InsertQueue(gameObject, ObjectPoolingManager.instance.tBullet2Queue);
        }
    }
    public void SetVelocityStart(float _speed, Vector3 _startVelcity, float startTime){
        bulletSpeed = _speed;
        startVelocity = _startVelcity;
        StartCoroutine(TrackBulletCoroutine(startTime));
    }
    IEnumerator TrackBulletCoroutine(float _time){
        myRigid.velocity = startVelocity.normalized * bulletSpeed;
        transform.up = startVelocity.normalized;
        yield return new WaitForSeconds(_time);

        while(gameObject.activeSelf){
            Vector3 curVelocity = (Camera.main.transform.position - transform.position).normalized;
            myRigid.velocity = Vector3.Lerp(myRigid.velocity.normalized, curVelocity, 0.3f) * bulletSpeed;
            transform.up = myRigid.velocity.normalized;
            yield return new WaitForSeconds(0.05f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tbullet : Bullet
{
    private Vector3 startVelocity;

    public override void RemoveBullet(){
        if(gameObject.activeSelf){
            ObjectPoolingManager.instance.InsertQueue(gameObject, ObjectPoolingManager.instance.tBulletQueue);
        }
    }

    public override void SetVelocity(float _speed){
        bulletSpeed = _speed;
        startVelocity = (Camera.main.transform.position - transform.position).normalized;
        StartCoroutine(TrackBulletCoroutine());
    }
    IEnumerator TrackBulletCoroutine(){
        myRigid.velocity = startVelocity.normalized * bulletSpeed;
        transform.up = startVelocity.normalized;

        while(gameObject.activeSelf){
            Vector3 curVelocity = (Camera.main.transform.position - transform.position).normalized;
            myRigid.velocity = Vector3.Lerp(myRigid.velocity.normalized, curVelocity, 0.3f) * bulletSpeed;
            transform.up = myRigid.velocity.normalized;
            yield return new WaitForSeconds(0.05f);
        }
    }
}

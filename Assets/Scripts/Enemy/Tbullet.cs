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

    public override void SetVelocity(){
        startVelocity = (Camera.main.transform.position - transform.position).normalized;
        StartCoroutine(TrackBulletCoroutine());
    }
    IEnumerator TrackBulletCoroutine(){
        while(gameObject.activeSelf){
            Vector3 curVelocity = (Camera.main.transform.position - transform.position).normalized;
            myRigid.velocity = Vector3.Lerp(startVelocity, curVelocity, 0.4f) * bulletSpeed;
            transform.up = Vector3.Lerp(startVelocity, curVelocity, 0.4f);
            yield return null;
        }
    }
}

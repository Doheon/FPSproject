using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{    
    public override void ShootBullet(){
        if(hasTarget && Time.time > lastAttackTime + fireRate){
            GameObject bullet = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.bulletQueue);

            if(bullet != null){
                bullet.transform.position = transform.position + Vector3.up * capsuleCollider.bounds.extents.y;
                Bullet _bullet = bullet.GetComponent<Bullet>();
                _bullet.damage = damage;

                _bullet.SetRemoveTime(5);
                _bullet.SetVelocity();
            }
            lastAttackTime = Time.time;
        }
    }
}

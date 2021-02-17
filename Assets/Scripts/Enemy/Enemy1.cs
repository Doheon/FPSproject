using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{    
    
    public override void ShootBullet(){
        
        if(hasTarget && Time.time > lastAttackTime + fireRate){
            
            GameObject bullet = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.bulletQueue);

            if(bullet != null){
                bullet.transform.position = FirePosition.position;
                Bullet _bullet = bullet.GetComponent<Bullet>();
                _bullet.damage = damage;

                _bullet.SetRemoveTime(2);
                _bullet.SetVelocity(50f);
            }
            lastAttackTime = Time.time;
        }
        if(hasTarget && (Time.time > lastAttackTime + fireRate - 0.25f || Time.time < lastAttackTime + 0.25f)) enemyAnimator.SetBool("Right Aim", true);
        else enemyAnimator.SetBool("Right Aim", false);
    }


}

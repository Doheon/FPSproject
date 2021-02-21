using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElite1 : Enemy
{
    private bool canFire = false;
    public override void ShootBullet(){        
       
        if(hasTarget && Time.time > lastAttackTime + fireRate - 0.3f) {
            lastAttackTime = Time.time;
            canFire = true;
            enemyAnimator.SetTrigger("Melee Attack 01");
        }
        if(hasTarget&& canFire && Time.time > lastAttackTime + 0.3f){
            lastAttackTime = Time.time;
            canFire = false;
            GameObject bullet = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.tBulletQueue);
            if(bullet != null){
                bullet.transform.position = FirePosition.position;

                Bullet _bullet = bullet.GetComponent<Bullet>();
                _bullet.damage = damage;
                
                _bullet.SetRemoveTime(2);
                _bullet.SetVelocity(TitleSetting.bulletSpeed);
            }
            
        }        


    }
}

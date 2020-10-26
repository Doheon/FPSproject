using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElite2 : Enemy
{
    public GameObject shield;

    public float shieldCoolTime = 5f;
    public float shieldTime = 2f;

    private float lastShieldTime = -10f;

    public override void Update()
    {
        base.Update();
        ActiveShield();
    }

    public void ActiveShield(){
        if(lastShieldTime + shieldCoolTime < Time.time){
            lastShieldTime = Time.time;
            StartCoroutine(ActiveShieldCoroutine(shieldTime));
        }
    }
    IEnumerator ActiveShieldCoroutine(float _dur){
        shield.SetActive(true);
        yield return new WaitForSeconds(_dur);
        shield.SetActive(false);
    }



    public override void ShootBullet(){
        if(hasTarget && Time.time > lastAttackTime + fireRate){
            for(int i=0; i<3; i++){
                GameObject bullet = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.tBullet2Queue);

                if(bullet != null){
                    bullet.transform.position = transform.position + Vector3.up * capsuleCollider.bounds.extents.y;

                    Tbullet2 _bullet = bullet.GetComponent<Tbullet2>();
                    _bullet.damage = damage;
                    
                    _bullet.SetRemoveTime(1);
                    _bullet.SetVelocityStart(50f,transform.forward + transform.right * (i-1), 0.05f);
                }
            }
            lastAttackTime = Time.time;
        }
    }
}

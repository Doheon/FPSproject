using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElite2 : Enemy
{
    public GameObject shield;
    public Transform firePosition2;

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
                GameObject bullet1 = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.tBullet2Queue);
                GameObject bullet2 = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.tBullet2Queue); 
                if(bullet1 != null){
                    bullet1.transform.position = FirePosition.position;
                    bullet2.transform.position = firePosition2.position;

                    Tbullet2 _bullet1 = bullet1.GetComponent<Tbullet2>();
                    _bullet1.damage = damage;
                    _bullet1.SetRemoveTime(1);
                    _bullet1.SetVelocityStart(TitleSetting.bulletSpeed * 0.6f,transform.forward + transform.right * (i-1), 0.05f);

                    Tbullet2 _bullet2 = bullet2.GetComponent<Tbullet2>();
                    _bullet2.damage = damage;
                    _bullet2.SetRemoveTime(1);
                    _bullet2.SetVelocityStart(TitleSetting.bulletSpeed * 0.6f,transform.forward + transform.right * (i-1), 0.05f);
                }
            }
            lastAttackTime = Time.time;
        }
    }
}

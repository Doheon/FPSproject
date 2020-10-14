using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Gun
{
    public float collectingRate;
    public int shotNum;
    public override void Hit()
    {
        for(int shotnum=0; shotnum<shotNum; shotnum++){
            Vector3 _randomDirection = Random.Range(-collectingRate, collectingRate) * theCam.transform.right + Random.Range(-collectingRate, collectingRate) * theCam.transform.up;
            if(Physics.Raycast(theCam.transform.position, theCam.transform.forward + _randomDirection, out hitInfo, range,  (-1) - (1<<11) )){
                GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(clone, 1f);
                IDamageable target = hitInfo.collider.GetComponent<IDamageable>();
                if(target != null){
                    target.OnDamage(damage, isCrit, hitInfo.point, hitInfo.normal, hitInfo.collider);
                    playerStatus.RestoreHealth(damage * playerStatus.lifeSteal / 100f);
                }
            }
        }
    }
}

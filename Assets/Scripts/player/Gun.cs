using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //------총정보-----
    public enum State{
        Ready,
        Empty,
        Reloading
    }
    public State state{get; private set;}
    public string gunName;
    public float range;
    public float accuracy;

    public float defaultFireRate;
    public float fireRate{
        get{
            return defaultFireRate/playerStatus.attackSpeed * 100f;
        }
    }
    public bool autoAimMode{
        get{
            return playerStatus.autoAimMode;
        }
    }


    private float lastFireTime;
    public int bulletConsume{
        get{
            return playerStatus.bulletConsume;
        }
    }

    public float reloadTime;
    public float defaultDamage;

    public bool isCrit;
    public float damage{
        get{
            float _crit = Random.Range(0f, 100f);
            isCrit = (_crit <= playerStatus.criticalProb);
            float _critDamage = isCrit? playerStatus.criticalDam/100f:1f;
            return defaultDamage * playerStatus.attackDamage / 100f * _critDamage;
        }
    }

    public int currentBullet; //현재 총알
    public int maxBullet;  //최대탄창
    public float retroActionForce;

    public Vector3 originPos;

    //------------------

    //레이저 충돌 정보
    public RaycastHit hitInfo;

    public Camera theCam;
    public GameObject hit_effect_prefab;

    public LayerMask targetLayer;
    public string targetTag = "Enemy"; //autohit에서 사용


    //기타 컴포넌트
    public PlayerStatus playerStatus;
    public ParticleSystem muzzleFlash;
    public AudioSource gunAudioPlayer;
    public AudioClip shotClip;
    public AudioClip ReloadClip;

    public Crosshair theCrosshair;

    public Transform LeftHandle;
    public Transform RightHandle;

    //coroutine
    Coroutine retro1, retro2;
    
    private void Awake(){
        playerStatus = GetComponentInParent<PlayerStatus>();
        gunAudioPlayer = GetComponent<AudioSource>();
        theCrosshair = FindObjectOfType<Crosshair>();
        originPos = transform.localPosition;
        LeftHandle = transform.GetChild(0);
        RightHandle = transform.GetChild(1);

        currentBullet = maxBullet;
        lastFireTime = 0f;

    }

    //-------------------------발포------------------------------
    public bool Fire(){
        if(state == State.Ready && Time.time >= lastFireTime + fireRate && currentBullet > 0){
            lastFireTime = Time.time;
            Shot();
            return true;
        }

        return false;
    }
    public void Shot(){
        muzzleFlash.Play();
        gunAudioPlayer.PlayOneShot(shotClip);
        //theCrosshair.fireAnimation();

        currentBullet -= bulletConsume;
        if(currentBullet <=0){
            state = State.Empty;
        }

        if(autoAimMode) AutoHit();
        else Hit();
        //반동
        transform.localPosition = originPos;
        
        if(retro1 != null) StopCoroutine(retro1);
        if(retro2 != null) StopCoroutine(retro2);
        retro1 = StartCoroutine(RetroActionCoroutine()); //총이 뒤로 반동
        retro2 = StartCoroutine(RetroActionCoroutine2()); //조준점이 위로 반동
    }

    IEnumerator RetroActionCoroutine(){
        Vector3 recoilBack = new Vector3(originPos.x, originPos.y, originPos.z + retroActionForce);
        while(transform.localPosition.z <= recoilBack.z - 0.01f){
            transform.localPosition = Vector3.Lerp(transform.localPosition, recoilBack, 0.4f);
            yield return null;
        }
        while(transform.localPosition.z - originPos.z > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, 0.1f);
            yield return null;
        }
    }
    private float change(float _val){
        if(_val > 180f) return _val-360f;
        else return _val;
    }
    IEnumerator RetroActionCoroutine2(){
        float origin = theCam.transform.localEulerAngles.x;
        while(change(theCam.transform.localEulerAngles.x) > change(origin - accuracy)){
            theCam.transform.localEulerAngles -= Vector3.right * 0.2f;
            yield return null;
        }
    }


    public virtual void Hit(){ //총알부딪힌거 
        if(Physics.Raycast(theCam.transform.position, theCam.transform.forward, out hitInfo, range,  (-1) - (1<<11) )){
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 1f);
            IDamageable target = hitInfo.collider.GetComponent<IDamageable>();
            if(target != null){
                target.OnDamage(damage, isCrit, hitInfo.point, hitInfo.normal, hitInfo.collider);
                playerStatus.RestoreHealth(damage * playerStatus.lifeSteal / 100f);
            }
        }
    }


    private void AutoHit(){
        RaycastHit[] hits = Physics.SphereCastAll(theCam.transform.position, 10f, theCam.transform.forward, range, targetLayer);
        for(int i=0; i<hits.Length; i++){
            IDamageable target = hits[i].transform.GetComponent<IDamageable>();
            if(target != null && hits[i].collider.GetType().Equals(typeof(SphereCollider))){
                if(Physics.Raycast(theCam.transform.position, (hits[i].point - theCam.transform.position).normalized, out hitInfo, range, (-1) - (1<<11))){;
                    if(hitInfo.transform.GetComponent<IDamageable>() == target){
                        target.OnDamage(damage, isCrit, hits[i].point, hits[i].normal, hits[i].collider);
                        playerStatus.RestoreHealth(damage * playerStatus.lifeSteal / 100f);
                        return;
                    }
                }
            }
        }
    }

    //------------------장전--------------------
    public bool Reload(){
        if(state == State.Reloading || currentBullet >= maxBullet) {
            return false;
        }
        gunAudioPlayer.PlayOneShot(ReloadClip);
        StartCoroutine(ReloadRoutine());
        return true;
    }

    private IEnumerator ReloadRoutine(){
        state = State.Reloading;
        yield return new WaitForSeconds(reloadTime);
        currentBullet = maxBullet;
        state = State.Ready;
    }

    //총변경
    public void cancelReload(){
        if(state == State.Reloading){
            StopAllCoroutines();
            state = State.Ready;
        }
    }
}

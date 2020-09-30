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

    public float fireRate;
    private float lastFireTime;

    public float reloadTime;
    public int damage;
    public int currentBullet; //현재 총알
    public int maxBullet;  //최대탄창
    public float retroActionForce;

    public Vector3 originPos;

    //------------------

    //레이저 충돌 정보
    private RaycastHit hitInfo;
    public Camera theCam;
    public GameObject hit_effect_prefab;


    //기타 컴포넌트
    public ParticleSystem muzzleFlash;
    private AudioSource gunAudioPlayer;
    public AudioClip shotClip;
    public AudioClip ReloadClip;

    public Crosshair theCrosshair;

    public Transform LeftHandle;
    public Transform RightHandle;

    private void Awake(){
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
    private void Shot(){
        muzzleFlash.Play();
        gunAudioPlayer.PlayOneShot(shotClip);
        theCrosshair.fireAnimation();

        currentBullet--;
        if(currentBullet <=0){
            state = State.Empty;
        }

        Hit();
        //반동
        transform.localPosition = originPos;
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
        StartCoroutine(RetroActionCoroutine2());
    }

    IEnumerator RetroActionCoroutine(){
        Vector3 recoilBack = new Vector3(originPos.x, originPos.y, retroActionForce);
        
        while(transform.localPosition.z <= retroActionForce - 0.01f){
            transform.localPosition = Vector3.Lerp(transform.localPosition, recoilBack, 0.4f);
            yield return null;
        }
        while(transform.localPosition != originPos){
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


    private void Hit(){
        if(Physics.Raycast(theCam.transform.position, theCam.transform.forward + 
        new Vector3(Random.Range(-theCrosshair.GetAccuracy(), theCrosshair.GetAccuracy()),
                    Random.Range(-theCrosshair.GetAccuracy(), theCrosshair.GetAccuracy()), 
                    0f), out hitInfo, range)){
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 1f);
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

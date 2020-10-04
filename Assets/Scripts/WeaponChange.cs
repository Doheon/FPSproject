using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChange : MonoBehaviour
{
    //무기 중복 교체 실행 방지
    public static bool isChangeWeapon = false;
    public static Gun currentWeapon;
    
    //무기교체 딜레이, 무기교체가 끝난시점
    public float changeWeaponDelayTime;
    public float changeWeaponEndDelayTime;
    
    public Gun[] guns;
    private PlayerInput playerInput;
    private PlayerShooter playerShooter;
    private Animator playerAnimator;

    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();


    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);   
        }
        playerInput = GetComponent<PlayerInput>();
        playerShooter = GetComponent<PlayerShooter>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isChangeWeapon){
            if(playerInput.weapon1 && currentWeapon != guns[0]){
                StartCoroutine(ChangeWeaponCoroutine(guns[0].gunName));
            }
            else if(playerInput.weapon2 && currentWeapon != guns[1]){
                StartCoroutine(ChangeWeaponCoroutine(guns[1].gunName));
            }
            else if(playerInput.weapon3 && currentWeapon != guns[2]){
                StartCoroutine(ChangeWeaponCoroutine(guns[2].gunName));
            }
        }
    }

    //무기교체 딜레이 시간
    public IEnumerator ChangeWeaponCoroutine(string _name){
        isChangeWeapon = true;

        playerAnimator.SetTrigger("WeaponOut");
        yield return new WaitForSeconds(changeWeaponDelayTime);
        CancelPreWeaponAction();
        Change(_name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);
        playerAnimator.SetTrigger("WeaponIn");
        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction(){
        currentWeapon.cancelReload();
        //저격조준 해제 나중에 추가
    }

    private void Change(string _name){
        playerShooter.GunChange(gunDictionary[_name]);
    }
}

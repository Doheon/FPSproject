using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;
    public Camera theCam;

    public Transform gunHolder;
    private Transform leftHandle;
    private Transform rightHandle;

    private PlayerInput playerInput;
    private Animator playerAnimator;

    private bool isSniperMode = false;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        leftHandle = gun.LeftHandle;
        rightHandle = gun.RightHandle;
        WeaponChange.currentWeapon = gun;
    }

    private void OnEnable(){
        gun.gameObject.SetActive(true);
    }
    private void OnDisable(){
       gun.gameObject.SetActive(false);
    }

    void Update()
    {
        fireORreload();
        
        UpdateUI();
    }

    private void fireORreload(){
        if(playerInput.fire){
            if(gun.Fire()){
                playerAnimator.SetTrigger("Fire");
            }
        }
        else if(playerInput.reload){
            if(gun.Reload()){
                playerAnimator.SetTrigger("Reload");
            }
        }
    }

    private void UpdateUI(){
        UIManager.instance.updateBulletText(gun.currentBullet, gun.maxBullet);
        SniperMode();
    }

    private void OnAnimatorIK(int layerIndex){
        gunHolder.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandle.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandle.rotation);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandle.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandle.rotation);   
    }

    public void GunChange(Gun _gun){
        if(_gun != gun){
            WeaponChange.currentWeapon.gameObject.SetActive(false);
            gun = _gun;
            gun.gameObject.SetActive(true);
            WeaponChange.currentWeapon = gun;
        }
        leftHandle = gun.LeftHandle;
        rightHandle = gun.RightHandle;
    }

    private void SniperMode(){
        if(gun.gunName == "Sniper" && playerInput.rightMouseDown){
            theCam.fieldOfView = 30f;
            UIManager.instance.SniperMode();
            gun.gameObject.transform.localScale = Vector3.zero;
            isSniperMode = true;
        }
        else if(gun.gunName == "Sniper" && playerInput.rightMouseUp){
            theCam.fieldOfView = 60f;
            UIManager.instance.cancelSniperMode();
            gun.gameObject.transform.localScale = Vector3.one;
            isSniperMode = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus1 : LivingEntity
{
    //hp, sp, skill, 공격력, 등등 모든 상태 조절
    public float SP;
    public float startSP = 100f;
    public float spendSP = 1f;
    public float restoreSP = 1f;

    //component
    private PlayerInput playerInput;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update(){
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SP = startSP;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider)
    {
        base.OnDamage(damage, hitPoint, hitNormal, hitCollider);
        UIManager.instance.ChangeHP(HP);
    }

    public void DecreaseSP(){
        if(SP - spendSP * Time.deltaTime > 0) {
            SP-= spendSP * Time.deltaTime;
            UIManager.instance.ChangeSP(SP);
        }
        else {
            SP = 0;
            UIManager.instance.ChangeSP(SP);
        }
    }

}

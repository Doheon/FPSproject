using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : LivingEntity
{
    //hp, sp, skill, 공격력, 등등 모든 상태 조절
    public float SP;
    public float startSP = 100f;
    public float spendSP = 20f;
    public float restoreSP = 20f;
    public float attackDamage = 100f;
    public float criticalProb = 0f;
    public float criticalDam = 200f;

    public float armor = 0f;
    public float damageReduction{
        get{
            return armor/(100f + armor);
        }
    }

    public float skillHaste = 0f;
    public float coolReduction{
        get{
            return skillHaste/(100f + skillHaste);
        }
    }
    
    public float attackSpeed = 100f;
    public float lifeSteal = 0f;
    public float moveSpeed = 100f;


    public int bulletConsume = 1;
    public bool autoAimMode = false;


    public float hpGrouth;
    public float adGrouth;


    private Vector3 originPos;
    private Quaternion originRot;
    //component

    public virtual void Awake() {
        originPos = transform.position;
        originRot = transform.rotation;

        UIManager.instance.HPslider.maxValue = startHP;
        UIManager.instance.HPslider.value = startHP;
        UIManager.instance.SPslider.maxValue = startSP;
        UIManager.instance.SPslider.value = startSP;

        GameManager.instance.PlayerLevelUp += LevelUPGrouth;
        GameManager.instance.StageClear += MoveToDefault;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SP = startSP;
    }

    public override void OnDamage(float damage, bool isCrit, Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider)
    {
        damage *= (1f-damageReduction);
        base.OnDamage(damage, isCrit, hitPoint, hitNormal, hitCollider);
        UIManager.instance.ChangeHP(HP, startHP);
        UIManager.instance.UpdateHPText(Mathf.RoundToInt(HP), (int)startHP);
    }

    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
        if(HP > startHP) HP = startHP;
        UIManager.instance.ChangeHP(HP, startHP);
        UIManager.instance.UpdateHPText(Mathf.RoundToInt(HP), (int)startHP);
    }

    public void DecreaseSP(){
        if(SP - spendSP * Time.deltaTime > 0) {
            SP-= spendSP * Time.deltaTime;
        }
        else {
            SP = 0;
        }
        UIManager.instance.ChangeSP(SP);
        UIManager.instance.UpdateSPText(Mathf.RoundToInt(SP), (int)startSP);
    }

    public void IncreaseSP(){
        if(SP + restoreSP * Time.deltaTime < startSP){
            SP += restoreSP * Time.deltaTime;
            UIManager.instance.ChangeSP(SP);
            UIManager.instance.UpdateSPText(Mathf.RoundToInt(SP), (int)startSP);
        }
    }

    public void LevelUPGrouth(){
        startHP += hpGrouth;
        HP += hpGrouth;
        attackDamage += adGrouth;

        UIManager.instance.ChangeHP(HP, startHP);
        UIManager.instance.UpdateHPText(Mathf.RoundToInt(HP), (int)startHP);
    }

    public void MoveToDefault(){        
        StartCoroutine(MoveToDefaultCoroutine());
        transform.rotation = originRot;
    }

    IEnumerator MoveToDefaultCoroutine(){
        while((transform.position - originPos).magnitude > 0.5f){
            transform.position = Vector3.Lerp(transform.position, originPos, 0.02f);
            yield return null;
        }
    }

    //능력치 바꿔주는 함수들

    public void AddAttackDamage(float _val){
        attackDamage += _val;
    }
    public void AddAttackSpeed(float _val){
        attackSpeed += _val;
    }
    public void AddCriticalDamage(float _val){
        criticalDam += _val;
    }
    public void AddCriticalProb(float _val){
        criticalProb += _val;
    }
    public void AddHP(float _val){
        startHP += _val;
        HP += _val;
        UIManager.instance.ChangeHP(HP, startHP);
        UIManager.instance.UpdateHPText(Mathf.RoundToInt(HP), (int)startHP);
    }

    public void AddArmor(float _val){
        armor += _val;
    }
    public void AddSkillHaste(float _val){
        skillHaste += _val;
    }
    public void AddMoveSpeed(float _val){
        moveSpeed += _val;
    }
    public void AddLifeSteal(float _val){
        lifeSteal += _val;
    }


}

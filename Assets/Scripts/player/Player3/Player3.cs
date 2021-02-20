using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player3 : PlayerStatus
{
//skill1
    public int skill1Level;
    public int maxSkill1Level;
    public float Skill1BaseCoolTime = 30f;
    public float Skill1RealCoolTime{
        get{
            return Skill1BaseCoolTime * (1f - coolReduction);
        }
    }

    public LayerMask stopLayer1, stopLayer2;

    public float skill1Damage{
        get{
            return attackDamage/50f *(skill1Level * 2f + 5f);
        }
    }

    public float skill1DurTime{
        get{
            return skill1Level * 5f + 5f;
        }
    }

    public float lastSkill1Time = -100f;
    public ParticleSystem skill1ParticleSystem;
    public Material stunMat;
    public Slider skill1DurSlider;

    //skill2
    public int skill2Level;
    public int maxSkill2Level;
    public float Skill2BaseCoolTime = 20f;
    public float Skill2RealCoolTime{
        get{
            return Skill2BaseCoolTime * (1f - coolReduction);
        }
    }

    public float skill2Force;
    

    public float lastSkill2Time = -100f;


    //skill3.

    public int skill3Level;
    public int maxSkill3Level;
    public float Skill3BaseCoolTime = 20f;
    public float Skill3RealCoolTime{
        get{
            return Skill3BaseCoolTime * (1f - coolReduction);
        }
    }

    public float skill3Damage{
        get{
            return skillDam/3f *(skill3Level + 2f);
        }
    }

    public int maxSkill3Num{
        get{
            return 1 + (skill3Level);
        }
    }



    public float lastSkill3Time = -100f;
    private float lastFireTime = -100f;

    public PlayerShooter playerShooter;
    public RaycastHit hitInfo;
    public GameObject explosionPrefab;
    public LayerMask targetLayer;
    public int skill3Num;
    public TextMeshProUGUI skill3NumText;

    


    public int canLevelUp = 1;
    public override void Awake()
    {
        base.Awake();
        GameManager.instance.PlayerLevelUp += () => canLevelUp++;
        playerShooter = GetComponent<PlayerShooter>();
        
        //skill1
        UIManager.instance.skill1LevelSlider.maxValue = maxSkill1Level;
        UIManager.instance.Skill1CantLevelUp();
        lastSkill1Time = -Skill1RealCoolTime;
        GameManager.instance.PlayerLevelUp += CanSkill1LevelUp;

        //skill2
        UIManager.instance.skill2LevelSlider.maxValue = maxSkill2Level;
        UIManager.instance.Skill2CanLevelUp();
        lastSkill2Time = -Skill2RealCoolTime;
        GameManager.instance.PlayerLevelUp += CanSkill2LevelUp;

        //skill3
        UIManager.instance.skill3LevelSlider.maxValue = maxSkill3Level;
        UIManager.instance.Skill3CanLevelUp();
        lastSkill3Time = -Skill3RealCoolTime;
        GameManager.instance.PlayerLevelUp += CanSkill3LevelUp;
    }

    private void Update() {
        //skill1
        CheckCanSkill1();
        CheckSkill1LevelUp();

        //skill2
        CheckCanSkill2();
        CheckSkill2LevelUp();

        //skill3
        CheckCanSkill3();
        CheckSkill3LevelUp();
    }
    //skill1
    private void CheckCanSkill1(){
        if(Time.time < lastSkill1Time + Skill1RealCoolTime){
            UIManager.instance.Skill1CoolDown(lastSkill1Time + Skill1RealCoolTime - Time.time, Skill1RealCoolTime);
            UIManager.instance.Skill1CoolDownText(lastSkill1Time + Skill1RealCoolTime - Time.time);
        }
        else if(PlayerInput.instance.skill1 && skill1Level > 0){
            lastSkill1Time = Time.time;
            UseSkill1();
        }
        else if(skill1Level>0) {
            UIManager.instance.Skill1CoolDown(0f, Skill1RealCoolTime);
            UIManager.instance.Skill1CoolDownText(0f);
        }
    }
    public void UseSkill1()
    {
        skill1ParticleSystem.Play();
        StartCoroutine(skill1SliderCoroutine());
        Collider[] allObjects = Physics.OverlapSphere(transform.position, 50f, stopLayer1 + stopLayer2);

        for(int i=0; i<allObjects.Length; i++){
            Stopable stoable = allObjects[i].GetComponent<Stopable>();
            if(stoable != null){
                stoable.Stop(skill1DurTime);
            }
        }
    }


    IEnumerator skill1SliderCoroutine(){
        float _startTime = Time.time;

        skill1DurSlider.gameObject.SetActive(true);
        while(Time.time < _startTime + skill1DurTime){
            skill1DurSlider.value = (skill1DurTime - Time.time + _startTime) / skill1DurTime;
            yield return null;
        }
        skill1DurSlider.gameObject.SetActive(false);
    }

    public void CheckSkill1LevelUp(){
        if(canLevelUp > 0 && PlayerInput.instance.skill1Up && 1 + (1+skill1Level) * 3 <= GameManager.instance.playerLevel){
            canLevelUp--;
            if(skill1Level == 0) UIManager.instance.Skill1CoolDown(0,1);
            skill1Level++;
            UIManager.instance.UpdateSkill1Level(skill1Level);

            UIManager.instance.Skill1CantLevelUp();
            UIManager.instance.Skill2CantLevelUp();
            UIManager.instance.Skill3CantLevelUp();

        }
    }

    public void CanSkill1LevelUp(){
        if(skill1Level < maxSkill1Level && 1 + (1+skill1Level) * 3 <= GameManager.instance.playerLevel ){
            UIManager.instance.Skill1CanLevelUp();
        }
    }
    
    //skill2
    private void CheckCanSkill2(){
        if(Time.time < lastSkill2Time + Skill2RealCoolTime){
            UIManager.instance.Skill2CoolDown(lastSkill2Time + Skill2RealCoolTime - Time.time, Skill2RealCoolTime);
            UIManager.instance.Skill2CoolDownText(lastSkill2Time + Skill2RealCoolTime - Time.time);
        }
        else if(PlayerInput.instance.skill2 && skill2Level > 0){
            lastSkill2Time = Time.time;
            UseSkill2();
        }
        else if(skill2Level>0) {
            UIManager.instance.Skill2CoolDown(0f, Skill2RealCoolTime);
            UIManager.instance.Skill2CoolDownText(0f);
        }
    }
    public void UseSkill2()
    {
        playerMovement.yVelocity = skill2Force;
    }

    public void CheckSkill2LevelUp(){
        if(canLevelUp > 0 && PlayerInput.instance.skill2Up){
            canLevelUp--;
            if(skill2Level == 0) UIManager.instance.Skill2CoolDown(0,1);
            skill2Level++;
            UIManager.instance.UpdateSkill2Level(skill2Level);

            UIManager.instance.Skill1CantLevelUp();
            UIManager.instance.Skill2CantLevelUp();
            UIManager.instance.Skill3CantLevelUp();

        }
    }
    public void CanSkill2LevelUp(){
        if(skill2Level < maxSkill2Level){
            UIManager.instance.Skill2CanLevelUp();
        }
    }

    //skill3
    private void CheckCanSkill3(){
        if(Time.time < lastSkill3Time + Skill3RealCoolTime && skill3Num < maxSkill3Num){
            UIManager.instance.Skill3CoolDown(lastSkill3Time + Skill3RealCoolTime - Time.time, Skill3RealCoolTime);
            UIManager.instance.Skill3CoolDownText(lastSkill3Time + Skill3RealCoolTime - Time.time);
        }
        
        if(skill3Level>0 && Time.time >= lastSkill3Time + Skill3RealCoolTime) {
            UIManager.instance.Skill3CoolDown(0f, Skill3RealCoolTime);
            UIManager.instance.Skill3CoolDownText(0f);
        }
        
        if( Time.time >= lastSkill3Time + Skill3RealCoolTime && skill3Level>0 && skill3Num < maxSkill3Num){
            lastSkill3Time = Time.time;
            skill3Num++;
        }        
        



        if(playerShooter.gun.isFire && playerShooter.isSniperMode && PlayerInput.instance.fire && skill3Level > 0 && skill3Num>0){
            lastFireTime = Time.time;
            if(skill3Num == maxSkill3Num) lastSkill3Time = Time.time;
            skill3Num--;
            UseSkill3();
        }
        if(PlayerInput.instance.fire) playerShooter.gun.isFire = false;

        skill3NumText.text = skill3Num.ToString();

    }
    public void UseSkill3()
    {
        ExplosionShot(1f);
    }

    
    public void ExplosionShot(float scale){
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, playerShooter.gun.range, (-1) - (1<<11) )){
            GameObject clone = Instantiate(explosionPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 1f);

            Collider[] enemys = Physics.OverlapSphere(hitInfo.point, 10f * scale, targetLayer);
            HashSet<IDamageable> enemySet = new HashSet<IDamageable>();

            for(int i=0; i<enemys.Length; i++){
                IDamageable target = enemys[i].GetComponent<IDamageable>();
                if(target != null && enemySet.Add(target)){
                    target.OnDamage(skill3Damage, false, hitInfo.point, hitInfo.normal, hitInfo.collider);
                }
            }
        }
    }

    public void CheckSkill3LevelUp(){
        if(canLevelUp > 0 && PlayerInput.instance.skill3Up){
            canLevelUp--;
            if(skill3Level == 0) UIManager.instance.Skill3CoolDown(0,1);
            skill3Level++;
            UIManager.instance.UpdateSkill3Level(skill3Level);

            UIManager.instance.Skill1CantLevelUp();
            UIManager.instance.Skill2CantLevelUp();
            UIManager.instance.Skill3CantLevelUp();
        }
    }

    public void CanSkill3LevelUp(){
        if(skill3Level < maxSkill3Level){
            UIManager.instance.Skill3CanLevelUp();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : PlayerStatus
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

    public int skill1Num{
        get{
            return 50 + 30 * skill1Level;
        }
    }

    public float skill1Damage{
        get{
            return skillDam/50f *(skill1Level * 2f + 5f);
        }
    }

    public float lastSkill1Time = -100f;

    //skill2
    public int skill2Level;
    public int maxSkill2Level;
    public float Skill2BaseCoolTime = 20f;
    public float Skill2RealCoolTime{
        get{
            return Skill2BaseCoolTime * (1f - coolReduction);
        }
    }
    public float skill2Stuntime{
        get{
            return 1f + (skill2Level * 0.5f);
        }
    }
    public float skill2Damage{
        get{
            return skillDam / 20f * (skill2Level *5f + 3f);
        }
    }
    public float lastSkill2Time = -100f;

    public LayerMask targetlayer;
    public ParticleSystem skill2Effect;
    public Material stunMat;


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
            return skillDam/100f *(skill3Level * 5f + 5f);
        }
    }

    public int skill3BallNum{
        get{
            return 10 + 1*skill3Level;
        }
    }


    public float lastSkill3Time = -100f;

    public PlayerShooter playerShooter;
    public GameObject fireBall;



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
        playerShooter.gun.gunAudioPlayer.PlayOneShot(playerShooter.gun.shotClip);
        for(int i=(200-ObjectPoolingManager.instance.rfbQueue.Count); i<skill1Num; i++){
            GameObject _ball = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.rfbQueue);
            RotateFireBall _fireBall  = _ball.GetComponent<RotateFireBall>();
            _fireBall.Set(skill1Damage, transform.position);
        }

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
        StartCoroutine(skill2Coroutine());
    }

    private void skill2(){
        skill2Effect.Play();
        Collider[] colliders = Physics.OverlapSphere(transform.position, 13.5f, targetlayer);
        for(int i=0; i<colliders.Length; i++){
            if(colliders[i].GetType().Equals(typeof(SphereCollider))){
                Enemy _enemy = colliders[i].GetComponent<Enemy>();
                if(_enemy) _enemy.OnDamage(skill2Damage, false, colliders[i].transform.position, Vector3.zero, colliders[i]);
                if(_enemy) _enemy.Stun(skill2Stuntime, stunMat);
            }
        }
    }

    IEnumerator skill2Coroutine(){
        isNodamage = true;
        skill2();
        yield return new WaitForSeconds(3f);
        isNodamage = false;
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
        if(Time.time < lastSkill3Time + Skill3RealCoolTime){
            UIManager.instance.Skill3CoolDown(lastSkill3Time + Skill3RealCoolTime - Time.time, Skill3RealCoolTime);
            UIManager.instance.Skill3CoolDownText(lastSkill3Time + Skill3RealCoolTime - Time.time);
        }
        else if(playerShooter.gun.gunName != "Sniper" && PlayerInput.instance.skill3 && skill3Level > 0){
            lastSkill3Time = Time.time;
            UseSkill3();
        }
        else if(skill3Level>0) {
            UIManager.instance.Skill3CoolDown(0f, Skill3RealCoolTime);
            UIManager.instance.Skill3CoolDownText(0f);
        }
    }
    public void UseSkill3()
    {
        playerShooter.gun.gunAudioPlayer.PlayOneShot(playerShooter.gun.shotClip);
        for(int i=0; i<skill3BallNum; i++){
            GameObject _ball = Instantiate(fireBall, playerShooter.gun.muzzleFlash.transform.position + transform.forward * 2, Quaternion.identity);
            FireBall _fireBall = _ball.GetComponent<FireBall>();
            _fireBall.SetVelocity(Camera.main.transform.forward, 0.3f);
            _fireBall.Set(skill3Damage);
            _fireBall.SetRemoveTime(2);
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

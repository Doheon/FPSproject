using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player1 : PlayerStatus
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

    public float skill1DurTime{
        get{
            return 3f + 2f * skill1Level;
        }
    }
    public float lastSkill1Time = -100f;


    //skill2
    public Animator[] shieldAnimator;
    public GameObject shieldObject;
    public Slider shieldTimeSlider;

    public int skill2Level;
    public int maxSkill2Level;
    public float Skill2BaseCoolTime = 20f;
    public float Skill2RealCoolTime{
        get{
            return Skill2BaseCoolTime * (1f - coolReduction);
        }
    }
    public float skill2DurTime{
        get{
            return 3f + 2f * skill2Level;
        }
    }
    public float lastSkill2Time = -100f;

    //skill3.
    public PlayerShooter playerShooter;
    public GameObject energyBall;

    public int skill3Level;
    public int maxSkill3Level;
    public float Skill3BaseCoolTime = 20f;
    public float Skill3RealCoolTime{
        get{
            return Skill3BaseCoolTime * (1f - coolReduction);
        }
    }
    public float skill3Stuntime{
        get{
            return 1f + (skill3Level * 0.5f);
        }
    }

    public float lastSkill3Time = -100f;



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
        StartCoroutine(Skill1Coroutine());
    }

    IEnumerator Skill1Coroutine(){
        bulletConsume = 0;
        autoAimMode = true;

        yield return new WaitForSeconds(skill1DurTime);
        bulletConsume = 1;
        autoAimMode = false;
    }

    public void CheckSkill1LevelUp(){
        if(canLevelUp > 0 && PlayerInput.instance.skill1Up && (1+skill1Level) * 5 <= GameManager.instance.playerLevel){
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
        if(skill1Level < maxSkill1Level && (1+skill1Level) * 5 <= GameManager.instance.playerLevel ){
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
        shieldObject.SetActive(true);
        shieldTimeSlider.value = 0f;
        StartCoroutine(Skill2Coroutine());
    }
    IEnumerator Skill2Coroutine(){
        float _time = Time.time;
        if(skill2Level == 1){
            shieldAnimator[0].SetTrigger("toSize2");
            while(Time.time < _time + skill2DurTime){
                shieldTimeSlider.value = (Time.time - _time)/skill2DurTime;
                yield return null;
            }
            shieldAnimator[0].SetTrigger("toSize0");
        }

        else if(skill2Level == 2){
            shieldAnimator[0].SetTrigger("toSize3");
            while(Time.time < _time + skill2DurTime){
                shieldTimeSlider.value = (Time.time - _time)/skill2DurTime;
                yield return null;
            }
            shieldAnimator[0].SetTrigger("toSize0");
        }
        else if(skill2Level == 3){
            shieldAnimator[0].SetTrigger("toSize4");
            while(Time.time < _time + skill2DurTime){
                shieldTimeSlider.value = (Time.time - _time)/skill2DurTime;
                yield return null;
            }
            shieldAnimator[0].SetTrigger("toSize0");
        }
        else if(skill2Level >= 4){
            shieldAnimator[0].SetTrigger("toSize4");
            shieldAnimator[1].SetTrigger("toSize4");
            shieldAnimator[2].SetTrigger("toSize4");
            while(Time.time < _time + skill2DurTime){
                shieldTimeSlider.value = (Time.time - _time)/skill2DurTime;
                yield return null;
            }
            shieldAnimator[0].SetTrigger("toSize0");
            shieldAnimator[1].SetTrigger("toSize0");
            shieldAnimator[2].SetTrigger("toSize0");
        }
        
        yield return new WaitForSeconds(0.25f);
        shieldObject.SetActive(false);
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
        GameObject _ball = Instantiate(energyBall, playerShooter.gun.muzzleFlash.transform.position, Quaternion.identity);
        EnergyBall _energyBall = _ball.GetComponent<EnergyBall>();
        _energyBall.SetVelocity();
        _energyBall.SetRemoveTime(4);
        _energyBall.Set(attackDamage/10 *(skill3Level * 3f + 5f), skill3Stuntime);

        // float _increase = (skill3Level*2f + 5f);
        // attackDamage *= _increase;
        // autoAimMode = true;
        // bulletConsume = 0;

        // playerShooter.gun.Shot();

        // bulletConsume = 1;
        // attackDamage /= _increase;
        // autoAimMode = false;
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

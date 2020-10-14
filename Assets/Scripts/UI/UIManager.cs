using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }
    private static UIManager m_instance;

    public Gun gun;
    public GameObject go_BulletHUD;
    
    //총알 텍스트
    public TextMeshProUGUI BulletText;
    //sniper
    public GameObject theSniperMode;
    //crosshair
    public GameObject theCrossHair;
    //hp, sp
    public Slider HPslider;
    public Slider SPslider;
    //hp,spbar text
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI SPText;

    //damage
    public TextMeshProUGUI damageText;
    public GameObject damage;
    public Camera mainCam;
    
    //stage & enemy
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI enemyLeft;

    //skill 1
    public Slider skill1CoolSlider;
    public Animator skill1LevelUp;
    public Slider skill1LevelSlider;
    public TextMeshProUGUI skill1CoolText;

    //skill 2
    public Slider skill2CoolSlider;
    public Animator skill2LevelUp;
    public Slider skill2LevelSlider;
    public TextMeshProUGUI skill2CoolText;

    //skill 3
    public Slider skill3CoolSlider;
    public Animator skill3LevelUp;
    public Slider skill3LevelSlider;
    public TextMeshProUGUI skill3CoolText;


    //level
    public Slider expSlider;
    public TextMeshProUGUI levelText;

    //stageClear
    public Animator stageClearAnimator;



    private void Start(){
        BulletText.text = gun.currentBullet + "/" + gun.maxBullet;
        theSniperMode.SetActive(false);

        stageClearAnimator.gameObject.SetActive(true);

        mainCam = Camera.main;
        skill1CoolSlider.value = skill1CoolSlider.maxValue;
        skill2CoolSlider.value = skill2CoolSlider.maxValue;
        skill3CoolSlider.value = skill3CoolSlider.maxValue;
        expSlider.value = 0f;
    }
    
    //text
    public void updateBulletText(int _cur, int _max){
        BulletText.text = _cur + "/" + _max;
    }
    public void UpdateHPText(int _cur, int _max){
        HPText.text = _cur + "/" + _max;
    }
    public void UpdateSPText(int _cur, int _max){
        SPText.text = _cur + "/" + _max;
    }
    public void UpdateStageText(int _main, int _sub){
        stageText.text = _main + "-" + _sub;
    }
    public void UpdateEnemyLeftText(int _left){
        enemyLeft.text = "Enemy Left: " + _left;
    }
    public void UpdateLevelText(int _level){
        levelText.text = _level.ToString();
    }



    public void SniperMode(){
        theSniperMode.SetActive(true);
    }
    public void cancelSniperMode(){
        theSniperMode.SetActive(false);
    }

    Coroutine ChangeSliderCoroutine;
    public void ChangeHP(float _cur, float _max){
        HPslider.maxValue = _max;
        if(ChangeSliderCoroutine != null) StopCoroutine(ChangeSliderCoroutine);
        ChangeSliderCoroutine = StartCoroutine(ChangeSlider(HPslider, _cur));
    }

    private IEnumerator ChangeSlider(Slider _slider, float _val){

        while(Mathf.Abs(_slider.value - _val) > 0.01f){
            _slider.value = Mathf.Lerp(_slider.value, _val, 0.2f);
            yield return null;
        }
    }
    public void ChangeSP(float _sp){
        SPslider.value = _sp;
    }

    //damage
    public void DisplayDamage(int _damage, bool isCrit, Vector3 _hitPosition){
        damageText.text = _damage.ToString();
        if(isCrit) damageText.color = new Color(255/255f, 132/255f, 112/255f);
        else damageText.color = Color.white;

        GameObject _damageText = Instantiate(damage, _hitPosition, Quaternion.identity, transform);
        _damageText.transform.position = mainCam.WorldToScreenPoint(_hitPosition);
        float _distance = (mainCam.transform.position - _hitPosition).magnitude;
        StartCoroutine(TextUP(_damageText, _hitPosition, _distance));
        Destroy(_damageText, 1f);
    }

    private IEnumerator TextUP(GameObject _text, Vector3 _worldPosition, float _distance){
        float moveDistance = _distance/10f;
        Vector3 goalPosition = _worldPosition + Vector3.up * Random.Range(moveDistance/2, moveDistance) + Vector3.left * Random.Range(-moveDistance,moveDistance);
        while((goalPosition.y - _worldPosition.y) > 0.01f){
            _worldPosition  = Vector3.Lerp(_worldPosition, goalPosition, 0.4f);
            UpdateToWorld(_text, _worldPosition);
            yield return null;
        }

        while(_text != null){
            UpdateToWorld(_text, _worldPosition);
            yield return null;
        }
    }

    private void UpdateToWorld(GameObject _object, Vector3 _position){
        _object.transform.position = mainCam.WorldToScreenPoint(_position);
    }

    //stage Clear
    public void StageClearText(){
        if(GameManager.instance.subStage == 0) return;
        stageClearAnimator.SetTrigger("Visible");
    }



    //skill 1
    public void Skill1CoolDown(float _cur, float _max){
        skill1CoolSlider.maxValue = _max;
        skill1CoolSlider.value = _cur;
    }
    public void Skill1CanLevelUp(){
        skill1LevelUp.SetBool("canLevelUp", true);
    }
    public void Skill1CantLevelUp(){
        skill1LevelUp.SetBool("canLevelUp", false);
    }
    public void UpdateSkill1Level(int _level){
        skill1LevelSlider.value = _level;
    }
    public void Skill1CoolDownText(float _cool){
        if(_cool>=1) skill1CoolText.text = Mathf.Floor(_cool).ToString();
        else if(_cool < 0.01f) skill1CoolText.text = "";
        else skill1CoolText.text = (Mathf.Floor(_cool * 10f) * 0.1f).ToString();
    }

    //skill 2
    public void Skill2CoolDown(float _cur, float _max){
        skill2CoolSlider.maxValue = _max;
        skill2CoolSlider.value = _cur;
    }
    public void Skill2CanLevelUp(){
        skill2LevelUp.SetBool("canLevelUp", true);
    }
    public void Skill2CantLevelUp(){
        skill2LevelUp.SetBool("canLevelUp", false);
    }
    public void UpdateSkill2Level(int _level){
        skill2LevelSlider.value = _level;
    }
    public void Skill2CoolDownText(float _cool){
        if(_cool>=1f) skill2CoolText.text = Mathf.Floor(_cool).ToString();
        else if(_cool < 0.01f) skill2CoolText.text = "";
        else skill2CoolText.text = (Mathf.Floor(_cool * 10f) * 0.1f).ToString();
    }

    //skill 3
    public void Skill3CoolDown(float _cur, float _max){
        skill3CoolSlider.maxValue = _max;
        skill3CoolSlider.value = _cur;
    }
    public void Skill3CanLevelUp(){
        skill3LevelUp.SetBool("canLevelUp", true);
    }
    public void Skill3CantLevelUp(){
        skill3LevelUp.SetBool("canLevelUp", false);
    }
    public void UpdateSkill3Level(int _level){
        skill3LevelSlider.value = _level;
    }
    public void Skill3CoolDownText(float _cool){
        if(_cool>=1f) skill3CoolText.text = Mathf.Floor(_cool).ToString();
        else if(_cool < 0.01f) skill3CoolText.text = "";
        else skill3CoolText.text = (Mathf.Floor(_cool * 10f) * 0.1f).ToString();
    }

    //exp
    public void UpdateExp(float _cur, float _max){
        expSlider.maxValue = _max;
        expSlider.value = _cur;
    }

}

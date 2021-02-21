using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ExpBall : LivingEntity
{
    public float exp{
        get{
            return ((GameManager.instance.mainStage-1) * 5 + GameManager.instance.subStage) *3 + 6f;  //60%
        }
    }
    public Slider hpSlider;
    private Coroutine hpsliderCoroutine;

    private Animator expBallAnim;

    private void Awake() {
        expBallAnim = GetComponent<Animator>();
    }
    private void Update() {
        LookAtPlayer();
    }
    private void LookAtPlayer(){
        Vector3 _lookAt = new Vector3(Camera.main.transform.position.x, hpSlider.transform.position.y, Camera.main.transform.position.z);
        hpSlider.transform.LookAt(_lookAt);
    }

    public void SetExpBallHP(float _startHP){
        startHP = _startHP;
        HP = _startHP;
        hpSlider.maxValue = startHP;
        hpSlider.value = startHP;
    }
    public void SetRemoveTime(float _time){
        Invoke("RemoveExpBall", _time);
    }
    public void RemoveExpBall(){
        if(gameObject.activeSelf) StartCoroutine(RemoveExpBallCoroutine());
    }
    IEnumerator RemoveExpBallCoroutine(){
        expBallAnim.SetTrigger("Remove");
        yield return new WaitForSeconds(0.25f);
        if(gameObject.activeSelf) ObjectPoolingManager.instance.InsertQueue(gameObject, ObjectPoolingManager.instance.expBallQueue);
    }

    public void DestroyExpBall(float _exp){
        int _ballNum = Mathf.RoundToInt(_exp/exp);
        for(int i=0; i<_ballNum; i++){
            GameObject expPiece = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.expPieceQueue);
            expPiece.transform.position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));

        }
        if(gameObject.activeSelf){
            ObjectPoolingManager.instance.InsertQueue(gameObject, ObjectPoolingManager.instance.expBallQueue);
        }
    }

    public override void OnDamage(float damage, bool isCrit, Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider)
    {
        base.OnDamage(damage, isCrit, hitPoint, hitNormal, hitCollider);

        if(hpsliderCoroutine != null) StopCoroutine(hpsliderCoroutine);
        if(!dead) hpsliderCoroutine = StartCoroutine(changeSlider(hpSlider, HP));

        UIManager.instance.DisplayDamage((int)damage, isCrit, hitPoint);

        if (HP <= 0 && !dead) Die();
    }

    private IEnumerator changeSlider(Slider _slider, float _val){
        while(Mathf.Abs(_slider.value - _val) > 0.01f){
            _slider.value = Mathf.Lerp(_slider.value, _val, 0.1f);
            yield return null;
        }
    }

}

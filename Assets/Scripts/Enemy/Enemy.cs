using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public Slider hpSlider;
    public float damage = 20f;
    public float fireRate;
    public float exp;
    public float lastAttackTime;

    public Renderer enemyRenderer;
    public Material stunMaterial;
    public CapsuleCollider capsuleCollider;

    public LayerMask TargetLayer;
    private LivingEntity targetEntity;
    public float sightDistance = 20f;
    public float updatePathTime = 0.5f;
    private float lastUpdateTime;
    public bool isStunned;

    public NavMeshAgent pathFinder;

    private Coroutine hpsliderCoroutine;
    private PlayerStatus player;




    public bool hasTarget{
        get{
            if(targetEntity != null && !targetEntity.dead) return true;
            return false;
        }
    }


    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        player = FindObjectOfType<PlayerStatus>();

        lastAttackTime = 0f;
        hpSlider.maxValue = startHP;
        hpSlider.value = startHP;
        
        isStunned = false;
    }

    void Update() {
        LookAtPlayer();
        SetPath();
        if(!isStunned){
            ShootBullet();
            pathFinder.isStopped = false;
        }
        else{
            pathFinder.isStopped = true;
        }
    }

    private void LookAtPlayer(){
        Vector3 _lookAt = new Vector3(Camera.main.transform.position.x, hpSlider.transform.position.y, Camera.main.transform.position.z);
        hpSlider.transform.LookAt(_lookAt);
    }

    //총 발사
    public virtual void ShootBullet(){

    }


    //적의 특성 설정
    public void Setup(float newHealth, float newDamage, float newSpeed, float newExp, Color skinColor){
        startHP = newHealth;
        HP = newHealth;
        exp = newExp;

        hpSlider.maxValue = startHP;
        hpSlider.value = startHP;

        damage = newDamage;
        pathFinder.speed = newSpeed;
        enemyRenderer.material.color = skinColor;
    }

    //navmesh관련

    private void SetPath(){
        if(Time.time > updatePathTime + lastUpdateTime){
            lastUpdateTime = Time.time;
            UpdatePath();
        }
    }

    private void UpdatePath(){
        if(hasTarget){
            pathFinder.SetDestination(targetEntity.transform.position);
        }
        else{
            Collider[] colliders = Physics.OverlapSphere(transform.position, sightDistance, TargetLayer);
            if(colliders.Length > 0){
                LivingEntity livingEntity = colliders[0].GetComponent<LivingEntity>();
                Vector3 _direction = (livingEntity.transform.position - transform.position - Vector3.up * 2).normalized;
                if(Physics.Raycast(transform.position + Vector3.up * 2 * capsuleCollider.bounds.extents.y, _direction, out RaycastHit _hit, sightDistance)){
                    if(_hit.transform.tag == "Player"){
                        targetEntity = livingEntity;
                        pathFinder.SetDestination(targetEntity.transform.position);
                    }
                }
            }
            if(!hasTarget){
                pathFinder.SetDestination(transform.position + new Vector3(Random.Range(-10f,10f), 0f,Random.Range(-10f,10f)));
            }
        }

    }

    //ondamage관련
    public override void OnDamage(float damage, bool isCrit, Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider)
    {
        base.OnDamage(damage, isCrit, hitPoint, hitNormal, hitCollider);

        targetEntity = player;

        if(hitCollider.GetType().Equals(typeof(SphereCollider))){
            HP -= damage;
            damage *= 2;
        }
        
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

    public void Stun(float _time){
        StartCoroutine(StunCoroutine(_time));
    }

    IEnumerator StunCoroutine(float _time){
        isStunned = true;
        Material _ori = enemyRenderer.material;
        enemyRenderer.material = stunMaterial;
        yield return new WaitForSeconds(_time);
        isStunned = false;
        enemyRenderer.material = _ori;
    }

    //die
    public override void Die()
    {
        base.Die();
        //pathFinder.isStopped = true;
        pathFinder.enabled = false;
        gameObject.SetActive(false);
    }

}

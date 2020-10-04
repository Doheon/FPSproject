using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyType1 : LivingEntity
{
    public Slider hpSlider;
    public float damage = 20f;
    public float fireRate;
    private float lastAttackTime;
    private Renderer enemyRenderer;


    public LayerMask TargetLayer;
    private LivingEntity targetEntity;
    public float sightDistance = 20f;

    public NavMeshAgent pathFinder;



    private bool hasTarget{
        get{
            if(targetEntity != null && !targetEntity.dead) return true;
            return false;
        }
    }


    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();

        enemyRenderer = GetComponentInChildren<Renderer>();
    }

    private void Start(){
    }

    void Update() {
        hpSlider.transform.LookAt(Camera.main.transform.position); //hp바가 항상 블레이어를 쳐다보게함
        UpdatePath();
    }


    //적의 특성 설정
    public void Setup(float newHealth, float newDamage, float newSpeed, Color skinColor){
        startHP = newHealth;
        HP = newHealth;

        damage = newDamage;
        pathFinder.speed = newSpeed;
        enemyRenderer.material.color = skinColor;
    }

    //navmesh관련
    private void UpdatePath(){
        if(hasTarget){
            pathFinder.isStopped = false;
            pathFinder.SetDestination(targetEntity.transform.position);
        }
        else{
            pathFinder.isStopped = true;
            Collider[] colliders = Physics.OverlapSphere(transform.position, sightDistance, TargetLayer);
            if(colliders.Length >0){
                LivingEntity livingEntity = colliders[0].GetComponent<LivingEntity>();
                Vector3 _direction = (livingEntity.transform.position - transform.position - Vector3.up * 2).normalized;
                if(Physics.Raycast(transform.position + Vector3.up * 2, _direction, out RaycastHit _hit, sightDistance)){
                    if(_hit.transform.name == "Player"){
                        targetEntity = livingEntity;
                    }
                }
            }
        }
    }

    //ondamage관련
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider)
    {
        base.OnDamage(damage, hitPoint, hitNormal, hitCollider);
        if(hitCollider.GetType().Equals(typeof(SphereCollider))){
            HP -= damage;
        }
        if (HP <= 0 && !dead) Die();

        StopAllCoroutines();
        if(!dead) StartCoroutine(changeSlider(hpSlider, HP));
    }

    private IEnumerator changeSlider(Slider _slider, float _val){
        while(Mathf.Abs(_slider.value - _val) > 0.01f){
            _slider.value = Mathf.Lerp(_slider.value, _val, 0.1f);
            yield return null;
        }
    }

    //die
    public override void Die()
    {
        base.Die();
        pathFinder.isStopped = true;
        pathFinder.enabled = false;
        gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFireBall : MonoBehaviour
{
    public Rigidbody myRigid;
    public float damage = 1f;
    public float ballSpeed = 1f;
    public float radius = 1f;
    private float curRadius = 0f;

    public LayerMask targetLayer;
    private Enemy target;
    private RaycastHit hitInfo;
    public PlayerStatus player;
    public LivingEntity playerLiving;
    private bool hasTarget;

    private float startT;
    private float startTime;


    
    void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerStatus>();
        playerLiving = player.GetComponent<LivingEntity>();
    }



    private void Update() {
        if(!hasTarget) SetVelocity();
    }

    private void OnEnable() {
        hasTarget = false;
        startTime = Time.time;
        curRadius = 0f;
    }

    private void OnTriggerEnter(Collider other) {
        LivingEntity _enemy = other.GetComponent<LivingEntity>();
        if( _enemy != null && _enemy != playerLiving){
            hasTarget = true;
            StartCoroutine(TrackEnemyCoroutine(_enemy));
        }
    }
    IEnumerator TrackEnemyCoroutine(LivingEntity _enemy){
        while(gameObject){
            if(_enemy == null) {
                hasTarget = false;
                break;
            }

            Vector3 _target;
            Enemy isEnemy = _enemy.GetComponent<Enemy>();
            if(isEnemy != null) _target = isEnemy.transform.position + isEnemy.posCollider.center;
            else _target = _enemy.transform.position;

            Vector3 _curVel = (_target - transform.position).normalized;
            myRigid.velocity = Vector3.Lerp(myRigid.velocity.normalized, _curVel, 0.3f) * ballSpeed;
            
            if(Vector3.Distance(transform.position, _target) < 1f){
                _enemy.OnDamage(damage, false, _enemy.transform.position, Vector3.one, new CapsuleCollider());
                RemoveBall();
            }
            yield return null;
        }
    }

    public virtual void RemoveBall(){
        if(gameObject.activeSelf) ObjectPoolingManager.instance.InsertQueue(gameObject, ObjectPoolingManager.instance.rfbQueue);
    }

    public void Set(float _dam, Vector3 _pos){
        hasTarget = false;
        transform.position = _pos;
        damage = _dam;
        Vector3 randomVec = (Random.Range(-1f,1f) * Vector3.right + Random.Range(-1f, 1f) * Vector3.forward).normalized * radius;
        startT = Mathf.Atan2(randomVec.z, randomVec.x);
    }

    public void SetVelocity(){
        Vector3 _center = player.transform.position;
        if(curRadius < radius) curRadius += 0.1f;
        else curRadius = radius;

        if(Time.time > startTime + 3f) RemoveBall();

        Vector3 _pos = new Vector3(Mathf.Cos((startT + Time.time) * ballSpeed / curRadius), 0f, Mathf.Sin((startT + Time.time) * ballSpeed / curRadius)) * curRadius + _center;
        myRigid.MovePosition(_pos);
    }




}

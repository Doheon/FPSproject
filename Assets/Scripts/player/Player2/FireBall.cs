using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public Rigidbody myRigid;
    public float damage = 1f;
    public float ballSpeed = 1f;
    public float collectingRate;

    public LayerMask targetLayer;
    private Vector3 startVelocity;
    private Enemy target;
    private RaycastHit hitInfo;
    public bool canAttack;

    void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
        canAttack = true;
    }

    // private void OnTriggerEnter(Collider other) {
    //     Enemy _enemy = other.GetComponent<Enemy>();
    //     CapsuleCollider temp = new CapsuleCollider();
    //     if(_enemy != null && canAttack){
    //         canAttack = false;
    //         _enemy.OnDamage(damage, false, _enemy.enemyRenderer.transform.position, Vector3.one, temp);
    //         RemoveBall();
    //     }
    // }

    public void SetRemoveTime(int _time){
        Invoke("RemoveBall", _time);
    }
    public virtual void RemoveBall(){
        if(gameObject.activeSelf) Destroy(gameObject);
    }

    public void Set(float _dam){
        damage = _dam;
    }

    public void SetVelocity(Vector3 startVelocity, float durTime){
        Vector3 _randomDirection = Random.Range(-collectingRate, collectingRate) * Camera.main.transform.right + Random.Range(-collectingRate, collectingRate) * Camera.main.transform.up;
        //Vector3 _randomDirection = new Vector3(Random.Range(-collectingRate, collectingRate), Random.Range(-collectingRate, collectingRate), Random.Range(-collectingRate, collectingRate));
        myRigid.velocity = (startVelocity + _randomDirection).normalized * ballSpeed;
        StartCoroutine(TrackEnemyCoroutine(durTime));
    }

    IEnumerator TrackEnemyCoroutine(float durTime){
        yield return new WaitForSeconds(durTime);

        Collider[] closeEnemys = Physics.OverlapSphere(transform.position, 15f, targetLayer);
        System.Array.Sort(closeEnemys, (a, b) => Vector3.Distance(a.transform.position, transform.position) < Vector3.Distance(b.transform.position, transform.position) ? -1:1);

        if(closeEnemys.Length > 0){
            LivingEntity target = closeEnemys[0].GetComponent<LivingEntity>();
            while(gameObject && target){
                Enemy isEnemy = target.GetComponent<Enemy>();
                Vector3 targetPos;
                if(isEnemy != null) targetPos = isEnemy.transform.position + isEnemy.posCollider.center;
                else targetPos = target.transform.position;

                Vector3 _curvel = (targetPos - transform.position).normalized;
                myRigid.velocity = Vector3.Lerp(myRigid.velocity.normalized, _curvel, 0.3f) * ballSpeed;

                if(Vector3.Distance(transform.position, targetPos) < 1f){
                    target.OnDamage(damage, false, targetPos, Vector3.one, new CapsuleCollider());
                    RemoveBall();
                }
                yield return null;
            }
        }
        else RemoveBall();
    }
}

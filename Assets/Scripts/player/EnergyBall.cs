using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    public Rigidbody myRigid;
    public float damage = 1f;
    public float ballSpeed = 1f;
    public float range = 20f;
    public float stunTime = 3f;
    public LayerMask targetLayer;

    private Vector3 startVelocity;
    private Enemy target;
    private RaycastHit hitInfo;
    public bool canAttack;

    public Material stunMat;

    void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other) {
        Enemy _enemy = other.GetComponent<Enemy>();
        CapsuleCollider temp = new CapsuleCollider();
        if(_enemy != null && canAttack){
            canAttack = false;
            _enemy.Stun(stunTime, stunMat);
            _enemy.OnDamage(damage, false, other.transform.position, Vector3.one, temp);
            RemoveBall();
        }
    }

    public void SetRemoveTime(int _time){
        Invoke("RemoveBall", _time);
    }
    public virtual void RemoveBall(){
        if(gameObject.activeSelf) Destroy(gameObject);
    }

    public void Set(float _dam, float _stunTime){
        stunTime = _stunTime;
        damage = _dam;
    }

    public void SetVelocity(){
        myRigid.velocity = Camera.main.transform.forward * ballSpeed; 
        if(Physics.SphereCast(transform.position, 5f, Camera.main.transform.forward, out hitInfo,range, targetLayer)){
            target = hitInfo.transform.GetComponent<Enemy>();
            StartCoroutine(TrackEnemyCoroutine());
        }
    }
    IEnumerator TrackEnemyCoroutine(){
        yield return new WaitForSeconds(0.1f);
        while(gameObject.activeSelf && target.gameObject.activeSelf){
            Vector3 curVelocity = (target.transform.position + target.posCollider.center - transform.position).normalized;
            myRigid.velocity = Vector3.Lerp(myRigid.velocity.normalized, curVelocity, 0.3f) * ballSpeed;
            transform.up = curVelocity;
            yield return null;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPiece : MonoBehaviour
{
    public float speed;
    public Rigidbody myRigid;

    public float health{
        get{
            return ((GameManager.instance.mainStage-1)*5 + GameManager.instance.subStage) * 0.1f + 0.5f;
        }
    }
    public float exp{
        get{
            return ((GameManager.instance.mainStage-1) * 5 + GameManager.instance.subStage) *3 + 6f;  //60%
        }
    }

    private void Awake() {
        myRigid = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        StartCoroutine(ExpControlCoroutine());
    }

    IEnumerator ExpControlCoroutine(){
        myRigid.velocity = new Vector3(Random.Range(-speed, speed), Random.Range(-speed, speed), Random.Range(-speed, speed));
        yield return new WaitForSeconds(0.1f);
        while(gameObject.activeSelf){
            myRigid.velocity = (Camera.main.transform.position - transform.position).normalized * speed;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            PlayerStatus _player = other.GetComponent<PlayerStatus>();
            _player.RestoreHealth(health);
            GameManager.instance.AddExp(exp);
            ObjectPoolingManager.instance.InsertQueue(gameObject, ObjectPoolingManager.instance.expPieceQueue);
        }
    }
}

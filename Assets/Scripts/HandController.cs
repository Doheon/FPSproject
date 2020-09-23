using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    // 현재 장착된 hand형 타입 무기
    [SerializeField]
    private Hand currentHand;

    //공격중
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;//광선 맞은 녀석의 정보를 불러옴

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TryAttack();
        
    }
    private void TryAttack(){
        if(Input.GetButton("Fire1")){
            if(!isAttack){
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine(){
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");
        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;

        //공격 활성화 시점.
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isAttack = false;
    }
    IEnumerator HitCoroutine(){
        while(isSwing){
            if(CheckObject()){
                //충돌함
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    private bool CheckObject(){
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range)){
            return true;
        }
        return true;
    }
}

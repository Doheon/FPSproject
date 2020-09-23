using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public string handName; 
    public float range; //공격범위
    public int damage; //공격력
    public float workSpeed; 
    public float attackDelay; //공격딜레이
    public float attackDelayA; //공격 활성화 시점
    public float attackDelayB; //공격 비활성화 시점

    public Animator anim;

}

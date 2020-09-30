using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Animator animator;
    private float gunAccuracy;

    public GameObject go_CrosshairHUD;
    
    public void fireAnimation(){
        animator.SetTrigger("Fire");
    }
    public void walkAnimation(bool _flag){
        animator.SetBool("Walking", _flag);
    }

    public float GetAccuracy(){
        gunAccuracy = 0.02f;
        return gunAccuracy;
    }
}

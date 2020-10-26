using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Movement : PlayerMovement
{
    public float dashDistance;
    private float _dtime = -10f;

    public override void Run(){
        applySpeed = walkSpeed;
        
        if(PlayerInput.instance.dash && status.SP > status.spendSP){
            _dtime = Time.time;
            status.DecreaseSPVal(status.spendSP);
        }
        else status.IncreaseSP();
        if(Time.time < _dtime + 0.1f) {
            direction += direction * dashDistance;
        }
        
    }
 
}

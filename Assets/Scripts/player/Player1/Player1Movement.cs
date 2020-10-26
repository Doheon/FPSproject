using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Movement : PlayerMovement
{
    public float runSpeed;
    public override void Run(){
        if(PlayerInput.instance.run && canRun && status.SP > 0){
            applySpeed = runSpeed * status.moveSpeed / 100f;
            status.DecreaseSP();
        }
        else{
            if(status.SP < 0.1f) canRun = false;
            applySpeed = walkSpeed * status.moveSpeed / 100f;
            status.IncreaseSP();
        }
        if(status.SP > 50) canRun = true;
    }
}
 
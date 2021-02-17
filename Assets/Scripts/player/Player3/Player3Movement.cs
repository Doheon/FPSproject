using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3Movement : PlayerMovement
{
    public override void Run()
    {
        applySpeed = walkSpeed;
        if(PlayerInput.instance.run && canRun && status.SP > 0){
            yVelocity = 0f;
            status.DecreaseSP();
        }
        else{
            if(status.SP < 0.1f) canRun = false;
            status.IncreaseSP();
        }
        if(status.SP > 50) canRun = true;
    }
}

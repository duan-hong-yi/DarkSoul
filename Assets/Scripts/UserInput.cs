using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : IUserInput
{
    [Header("====== KeySettings  ======")]
    public string KeyUp = "w";
    public string KeyDown = "s";
    public string KeyLeft = "a";
    public string KeyRight = "d";
    public string KeyA = "left shift";
    public string KeyB = "space";
    public string KeyC = "l";
    public string KeyD ;
    public string KeyJUp;
    public string KeyJDown;
    public string KeyJLeft;
    public string KeyJRight;



    private void Start()
    {
        InputEnable = true;
        Dmag = 0;
        Dvec = new Vector3(0, 0, 0);
        KeyA = "left shift";
        KeyB = "space";
        KeyC = "j";
        KeyJUp = "up";
        KeyJLeft = "left";
        KeyJRight = "right";
        KeyJDown = "down";

    }

    private void Update()
    {
        targetDup = (Input.GetKey(KeyUp) == true ? 1.0f : 0) -( Input.GetKey(KeyDown) == true ? 1.0f : 0);
        targetDright = (Input.GetKey(KeyRight) == true ? 1.0f : 0) - (Input.GetKey(KeyLeft) == true ? 1.0f : 0);
        if(!InputEnable)
        {
            targetDright = 0;
            targetDup = 0;
        }
        Dup = Mathf.SmoothDamp(Dup, targetDup,ref velocityUp, 0.05f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityRight, 0.05f);

        float Dup2, Dright2;
        Dup2 = SquareToCircle( new Vector2(Dup,Dright)).x;
        Dright2 = SquareToCircle(new Vector2(Dup, Dright)).y;

        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2);
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;
        
        if(Input.GetKeyDown(KeyA))
        {
            run = true;
        }
        if(Input.GetKeyUp(KeyA))
        {
            run = false;
        }
 

        Vector2 mc = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        JRight = Mathf.SmoothDamp(JRight, mc.x,ref RightV, 0.3f);
        JUp = Mathf.SmoothDamp(JUp, mc.y, ref UpV, 0.3f); ;
        bool tempJump = Input.GetKey(KeyB);
        if(tempJump!=lastJump&&tempJump==true)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        lastJump = tempJump;

        bool tempAttack = Input.GetKey(KeyC);
        if (tempAttack != lastAttack && tempAttack == true)
        {
            attack = true;
        }
        else
        {
            attack = false;
        }
        lastAttack = tempAttack;

    }
 
}

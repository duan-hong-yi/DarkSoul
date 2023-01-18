using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{
    [Header("====== Physics ======")]
    public float targetDup;
    public float targetDright;
    public float Dup;
    public float Dright;
    public float velocityUp;
    public float velocityRight;
    public float Dmag;
    public float JUp;
    public float JRight;
    public Vector3 Dvec;
    public float UpV;
    public float RightV;

    [Header("====== State ======")]
    public bool run;
    public bool jump;
    public bool lastJump;
    public bool InputEnable;
    public bool attack;
    public bool lastAttack;


    public Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = new Vector2();

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }
}

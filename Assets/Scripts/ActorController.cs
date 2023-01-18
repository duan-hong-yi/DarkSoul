using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    [Header("====== Basic Settings ======")]
    public GameObject model;
    public IUserInput moveInput;

    [Header("====== Physics ======")]
    public float RunSpeed;
    public float WalkSpeed;
    public bool JumpLocker;
    public float JumpForce=3f;
    public float rollingForce = 5f;
    private float currentForce;
    private Vector3 deltaPosition;

    [SerializeField]
    private Animator anim;

    private Transform tr;

    public Vector3 d_rotation;

    private Rigidbody mrigidbody;
    [SerializeField]
    private Vector3 moveDirection;
    public float Speed = 2f;

    [Header("====== Lerp Settings ======")]
    public float RunSmoother;
    public float Velocity;
    public Vector3 lastOffGroundVelocity;
    public bool lockVelocity;
    public bool isFalling;
    public bool isOnGround;
    public bool isRolling;
    public bool isJumping;
    [SerializeField]
    private float lerpTarget;
    // Start is called before the first frame update
    void Awake()
    {
        
        anim = this.GetComponentInChildren<Animator>();
        model = anim.gameObject;
        moveInput = this.GetComponent<IUserInput>();
        tr = transform;
        mrigidbody = this.GetComponent<Rigidbody>();

        RunSpeed = 2.0f;
        WalkSpeed = 1.0f;
        JumpLocker = false;
        lockVelocity = false;
        isFalling = false;
        isOnGround = true;
        lastOffGroundVelocity = Vector3.zero;
        isRolling = false;
        isJumping = false;
        deltaPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        RunSmoother = moveInput.run ? Mathf.SmoothDamp(RunSmoother, 2.0f, ref Velocity, 0.2f) : Mathf.SmoothDamp(RunSmoother, 1.0f, ref Velocity, 0.2f);
        anim.SetFloat("forward", moveInput.Dmag * RunSmoother);

        d_rotation = moveInput.Dright * tr.right + moveInput.Dup * tr.forward;
        if (Mathf.Abs(d_rotation.x) >= 0.01f || Mathf.Abs(d_rotation.z) >= 0.01f)
        {
            model.transform.forward = Vector3.Slerp(model.transform.forward, d_rotation, 0.2f);//让转身带旋转
        }

        if (moveInput.jump)
        {
            anim.SetTrigger("Jump");
        }
        if (moveInput.attack && !isRolling && !isJumping)
        {
            anim.SetTrigger("Attack");
        }
        if (!JumpLocker)
            moveDirection = moveInput.Dmag * model.transform.forward;
        if (Vector3.Magnitude(lastOffGroundVelocity) > 3.0f || JumpLocker == true)
            anim.SetTrigger("roll");
        else
            anim.ResetTrigger("roll");
        if (isOnGround && !isRolling)
            lastOffGroundVelocity = Vector3.zero;
        if (JumpLocker)
            isJumping = true;
        else
            isJumping = false;
    }
    private void FixedUpdate()
    {

        if (!lockVelocity)
        { 
            mrigidbody.velocity = new Vector3(((moveInput.run ? RunSpeed : WalkSpeed) * Speed * moveDirection).x, mrigidbody.velocity.y, 
                ((moveInput.run ? RunSpeed : WalkSpeed) * Speed * moveDirection).z) + new Vector3(0, currentForce, 0);
        } 
        currentForce = 0;
        if (!isOnGround)
            lastOffGroundVelocity = mrigidbody.velocity;
        if(CheckState("attack1hC","Attack"))
            mrigidbody.position += deltaPosition;
        deltaPosition = Vector3.zero;
    }
    private bool CheckState(string stateName,string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }

    void OnJump()
    {
        moveInput.InputEnable = false;
        JumpLocker = true;
        currentForce = JumpForce;
        anim.SetTrigger("roll");
    }
   
    public void OnGround()
    {
        isOnGround = true;
        anim.SetBool("IsOnGround", true);
    }
    public void OffGround()
    {
        isOnGround = false;
        anim.SetBool("IsOnGround", false);
    }
    public void OnGroundEnter()
    {
        moveInput.InputEnable = true;
        JumpLocker = false;
    }
    public void OnRollEnter()
    {
        moveInput.InputEnable = false;
        lockVelocity = true;
        isRolling = true;
    }
    public void OnRollState()
    {
        mrigidbody.velocity = model.transform.forward*rollingForce;
       
    }
    public void OnExitRoll()
    {
        if(!isFalling)
        lockVelocity = false;
        isRolling = false;
    }
    public void OnFallingEnter()
    {
        lockVelocity = true;
        isFalling = true;
    }
    public void OnFallingExit()
    {
        isFalling = false;
        if(!isRolling)
        lockVelocity = false;
    }
    public void OnBackJumpEnter()
    {
        OnJump();
    }
    public void OnBackJumpState()
    {
        if(!lockVelocity)
        mrigidbody.velocity = mrigidbody.velocity - model.transform.forward * rollingForce;
        lockVelocity = true;
     
    }
    public void OnBackJumpExit()
    {
        lockVelocity = false;
    }
    public void OnEnterAttack1h()
    {
        
        moveInput.InputEnable = false;
        lockVelocity = true;
        lerpTarget = 1.0f;
    }
    public void OnEnterAttackIdle()
    {
      
        lockVelocity = false;
        moveInput.InputEnable = true;
        lerpTarget = 0;
    }
    public void OnAttack1hUpdate()
    {
        mrigidbody.velocity = model.transform.forward * anim.GetFloat("Attack1hAVelocity");
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("Attack")),lerpTarget, 0.05f));
    }
    public void OnAttackIdleUpdate()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("Attack")), lerpTarget, 0.05f));
    }
    public void OnRootMotionChanged(object _msg)
    {
        Vector3 position = (Vector3)_msg;
        deltaPosition += position;
    }
}

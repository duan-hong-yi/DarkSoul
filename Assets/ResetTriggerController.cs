using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTriggerController : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = this.GetComponentInParent<Animator>();
    }
    public void ResetTrigger(string signal)
    {
        anim.ResetTrigger(signal);
        print(signal);
    }
}

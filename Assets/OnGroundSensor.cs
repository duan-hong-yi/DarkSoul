using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider capsule;
    private Vector3 Point1, Point2;
    private float radius;
    [SerializeField]
    private float offset;
    private void Awake()
    {
        capsule = this.transform.parent.GetComponent<CapsuleCollider>();
        radius = capsule.radius;
    }
    private void FixedUpdate()
    {
        Point1 = capsule.transform.position + transform.up * capsule.height - transform.up * radius;
        Point2 = capsule.transform.position - transform.up * offset;
        Collider[] cols = Physics.OverlapCapsule(Point1, Point2, radius, LayerMask.GetMask("Ground"));
        if(cols.Length!=0)
        {
            SendMessageUpwards("OnGround");
        }
        else
        {
            SendMessageUpwards("OffGround");
        }
    }

}

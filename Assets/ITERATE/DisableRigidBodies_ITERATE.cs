using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRigidBodies_ITERATE : MonoBehaviour
{
    public float MoveSpeed = 1f;
    private Rigidbody[] physicalRigidBodies;

    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
        physicalRigidBodies = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
    }


    // Let the rigidbody take control and detect collisions.
    public void EnableRagdoll()
    {
        animator.enabled = false;

        foreach (var rb in physicalRigidBodies)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            //rb.velocity = velocityOfLogicalRigidBody;
        }
    }

    // Let animation control the rigidbody and ignore collisions.
    void DisableRagdoll()
    {
        animator.enabled = true;
        //logicalRigidBody.isKinematic = false;
        //logicalRigidBody.detectCollisions = true;

        foreach (var rb in physicalRigidBodies)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
    }   
}

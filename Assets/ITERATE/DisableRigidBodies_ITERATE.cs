using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRigidBodies_ITERATE : MonoBehaviour
{
    public float MoveSpeed = 1f;
    public GameObject ragdollParent;
    private Rigidbody[] physicalRigidBodies;

    private Animator animator;
    private Rigidbody logicalRigidBody;


    void Start()
    {
        animator = GetComponent<Animator>();
        logicalRigidBody = GetComponent<Rigidbody>();
        physicalRigidBodies = ragdollParent.GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
    }


    // Let the rigidbody take control and detect collisions.
    public void EnableRagdoll()
    {
        var velocityOfLogicalRigidBody = logicalRigidBody.velocity;
        animator.enabled = false;
        logicalRigidBody.isKinematic = true;
        logicalRigidBody.detectCollisions = false;

        foreach (var rb in physicalRigidBodies)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            rb.velocity = velocityOfLogicalRigidBody;
        }
    }

    // Let animation control the rigidbody and ignore collisions.
    void DisableRagdoll()
    {
        animator.enabled = true;
        logicalRigidBody.isKinematic = false;
        logicalRigidBody.detectCollisions = true;

        foreach (var rb in physicalRigidBodies)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
    }   
}

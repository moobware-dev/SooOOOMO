using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class RikishiController : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    Animator m_Animator;
    CapsuleCollider m_Capsule;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
    }

    public void SetDesiredAimTarget(Vector3 targetInWorldSpace) {
        transform.LookAt(targetInWorldSpace);
    }

    public void Move(Vector3 move)
    {
        m_Rigidbody.velocity = move;

        UpdateAnimator(move);
    }

    void UpdateAnimator(Vector3 move)
    {
        if (move.sqrMagnitude > 0f)
        {
            Debug.Log("They tryna move dawg");
        }
    }
}

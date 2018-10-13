using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class RikishiController : MonoBehaviour
{
    public float MoveSpeed = 1f;
    public float shoveForce = 100f;

    Rigidbody rigidBody;
    Animator animator;
    CapsuleCollider capsule;
    RikishiController enemy;

    bool isShoved = false;
    Vector3 shovedForce;
    bool enemyInRange;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        var rikishis = FindObjectsOfType<RikishiController>();
        if(rikishis[0] != this) {
            enemy = rikishis[0];
        } else if (rikishis[1] != this) {
            enemy = rikishis[1];
        }
    }

    public void SetDesiredAimTarget(Vector3 targetInWorldSpace) {
        transform.LookAt(targetInWorldSpace);
    }

    public void Move(Vector3 move)
    {
        move.y = rigidBody.velocity.y;
        rigidBody.velocity = move * MoveSpeed;

        UpdateAnimator(move);
    }

    public void AttemptShove() {
        animator.SetTrigger("Hadouken");
    }

    public void ShoveForce() {
        if (enemyInRange) {
            enemy.GetShoved((enemy.gameObject.transform.position - transform.position) * shoveForce);
        }
    }

    void GetShoved(Vector3 force)
    {
        isShoved = true;
        shovedForce = force;
        transform.parent.GetComponent<Rigidbody>().freezeRotation = false;
    }

    public void SetEnemyIsInRange(bool inRange)
    {
        Debug.Log("Enemy in range: " + inRange);
        this.enemyInRange = inRange;
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("You in the danger zone boy: " + other);
    }

    void UpdateAnimator(Vector3 move)
    {
        if (move.sqrMagnitude > 0.1f)
        {
            Debug.Log("They tryna move dawg");
            animator.SetTrigger("Stop Dancing");
        }
    }
}

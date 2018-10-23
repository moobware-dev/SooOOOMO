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
        if (isShoved == true) {
            return;
        }
        transform.LookAt(targetInWorldSpace);
    }

    public void Move(Vector3 move)
    {
        if (isShoved == true)
        {
            return;
        }
        move.y = rigidBody.velocity.y;
        rigidBody.velocity = move * MoveSpeed;

        UpdateAnimator(move);
    }

    public void AttemptShove() {
        animator.SetTrigger("Hadouken");
    }

    public void ShoveForce() {
        if (enemyInRange) {
            Debug.Log("This: " + this + " did the shoving");
            enemy.GetShoved((enemy.gameObject.transform.position - transform.position) * shoveForce);
        }
    }

    void GetShoved(Vector3 force)
    {
        Debug.Log("This: " + this + " got shoved");
        isShoved = true;
        shovedForce = force;
        rigidBody.freezeRotation = false;
    }

    void FixedUpdate()
    {
        if (isShoved)
        {
            Debug.Log("Force applied on: " + this);
            this.animator.SetTrigger("Shoved");
            GetComponentInParent<Rigidbody>().AddForce(shovedForce);
            isShoved = false;
            var enemyInputProviders = GetComponents<RikishiEnemyInputProvider>();
            foreach (var enemyInputProvider in enemyInputProviders) {
                enemyInputProvider.enabled = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("detected thing: " + other);
        if (other.CompareTag("Enemy"))
        {
            enemyInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log("detected thing: " + other);
        if (other.CompareTag("Enemy"))
        {
            enemyInRange = false;
        }
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

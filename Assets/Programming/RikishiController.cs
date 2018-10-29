using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class RikishiController : MonoBehaviour
{
    public float MoveSpeed = 1f;
    public float shoveForce = 100f;

    public float dodgeStepForce = 100f;
    public float dodgeHopForce = 100f;

    public float minimumTurnAmountThreshold = 0.01f;
    public float minimumMoveAmountThreshold = 0.5f;

    Rigidbody rigidBody;
    Animator animator;
    CapsuleCollider capsule;
    RikishiController enemy;

    bool isShoved = false;
    Vector3 shovedForce;
    bool enemyInRange;

    bool doinTheDance = true;

    Vector3 previousAimTarget;
    Vector3 currentAimTarget;

    bool isDodging = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        var rikishis = FindObjectsOfType<RikishiController>();
        if (rikishis[0] != this)
        {
            enemy = rikishis[0];
        }
        else if (rikishis[1] != this)
        {
            enemy = rikishis[1];
        }
    }

    public void SetDesiredAimTarget(Vector3 targetInWorldSpace)
    {
        if (isShoved)
        {
            return;
        }
        previousAimTarget = currentAimTarget;
        currentAimTarget = targetInWorldSpace;
        transform.LookAt(currentAimTarget);
    }

    public void Move(Vector3 move)
    {
        if (isShoved || isDodging)
        {
            return;
        }
        move.y = rigidBody.velocity.y;
        rigidBody.velocity = move * MoveSpeed;

        UpdateAnimator(move);
    }

    public void AttemptShove()
    {
        animator.SetTrigger("Hadouken");
    }

    public void ShoveForce()
    {
        if (enemyInRange)
        {
            Debug.Log("This: " + this + " did the shoving");
            enemy.GetShoved((enemy.gameObject.transform.position - transform.position) * shoveForce);
        }
    }

    public void DodgeRight() {
        if (isDodging) {
            return; 
        }
        isDodging = true;
        animator.SetTrigger("Dodge Right");
        rigidBody.AddForce(transform.right * dodgeStepForce);
    }

    public void DodgeRightStep() {
        rigidBody.AddForce(transform.right * dodgeHopForce);
    }

    public void DodgeDone() {
        isDodging = false;
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
            isShoved = false;
            Debug.Log("Force applied on: " + this);
            this.animator.SetTrigger("Shoved");
            GetComponentInParent<Rigidbody>().AddForce(shovedForce);
            var enemyInputProviders = GetComponents<RikishiEnemyInputProvider>();
            foreach (var enemyInputProvider in enemyInputProviders)
            {
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
        if (doinTheDance && move.sqrMagnitude > 0.1f)
        {
            Debug.Log("They tryna move dawg");
            animator.SetTrigger("Stop Dancing");
            doinTheDance = false;
        }

        var turning = (previousAimTarget - currentAimTarget).sqrMagnitude > minimumTurnAmountThreshold;

        // thanks smart unity people: https://docs.unity3d.com/Manual/AmountVectorMagnitudeInAnotherDirection.html
        // "The magnitude of an object’s rigidbody.velocity vector will give the speed 
        // in its direction of overall motion but to isolate the speed in the forward direction,
        // you should use the dot product"
        var playerForwardMovement = Mathf.Abs(Vector3.Dot(rigidBody.velocity, transform.forward));
        var playerRightMovement = Mathf.Abs(Vector3.Dot(rigidBody.velocity, transform.right));

        var standingStill = (
            !turning
            && playerForwardMovement < minimumMoveAmountThreshold
            && playerRightMovement < minimumMoveAmountThreshold);
        
        var turningInPlace =  (
            turning
            && playerForwardMovement < minimumMoveAmountThreshold
            && playerRightMovement < minimumMoveAmountThreshold);
        
        var walkingStraight = (
            !turning
            && playerForwardMovement > minimumMoveAmountThreshold
            && playerRightMovement < minimumMoveAmountThreshold);

        var strafing = (
            playerForwardMovement < minimumMoveAmountThreshold
            && playerRightMovement > minimumMoveAmountThreshold);

        if (standingStill)
        {
            animator.SetBool("Moving", false);
        }
        else if (strafing || turningInPlace)
        {
            animator.SetBool("Moving", true);
            animator.SetFloat("Walk0Strafe1Blend", 1f, 0.1f, Time.deltaTime);
        }
        else if (walkingStraight)
        {
            animator.SetBool("Moving", true);
            animator.SetFloat("Walk0Strafe1Blend", 0f, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetBool("Moving", true);
            animator.SetFloat("Walk0Strafe1Blend", 0.5f, 0.1f, Time.deltaTime);
        }
    }
}

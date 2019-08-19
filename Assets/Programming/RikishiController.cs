using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class RikishiController : MonoBehaviour
{
    public Material playerMaterial;
    public GameObject ragdollParent;
    private Rigidbody[] physicalRigidBodies;

    public float MoveSpeed = 1f;
    public float shoveForce = 100f;

    public float minimumTurnAmountThreshold = 0.01f;
    public float minimumMoveAmountThreshold = 0.5f;

    Rigidbody logicalRigidBody;
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

    //public Vector3 RollinSpeed = Vector3.zero;
    //private bool rollin = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        logicalRigidBody = GetComponent<Rigidbody>();
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

        var rendererer = GetComponentInChildren<Renderer>();
        rendererer.material = new Material(Shader.Find("Standard"));
        rendererer.material.color = Random.ColorHSV(0, 1, 0.5f, 1);

        StartCoroutine(MonkeyPatchFlyingSumoWreslterBug());

        physicalRigidBodies = ragdollParent.GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
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
        move.y = logicalRigidBody.velocity.y;
        logicalRigidBody.velocity = move * MoveSpeed;

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
            //Debug.Log("This: " + this + " did the shoving");
            enemy.GetShoved((enemy.gameObject.transform.position - transform.position).normalized * shoveForce);
        }
    }

    void GetShoved(Vector3 force)
    {
        Debug.Log("This: " + this + " got shoved");
        isShoved = true;
        shovedForce = force;
        logicalRigidBody.freezeRotation = false;
    }

    void FixedUpdate()
    {
        if (isShoved)
        {
            isShoved = false;
            //Debug.Log("Force applied on: " + this);
            this.animator.SetTrigger("Shoved");
            logicalRigidBody.AddForce(shovedForce);
            logicalRigidBody.AddRelativeForce(-1 * transform.forward * 200f);
            StartCoroutine(GoLimpAfterABit());
            var enemyInputProviders = GetComponents<RikishiEnemyInputProvider>();
            foreach (var enemyInputProvider in enemyInputProviders)
            {
                enemyInputProvider.enabled = false;
            }

            var playerInputProviders = GetComponents<RikishiPlayerInputProvider>();
            foreach (var playerInputProvider in playerInputProviders)
            {
                playerInputProvider.enabled = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("detected thing: " + other);
        if (other.gameObject != transform.gameObject)
        {
            enemyInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("detected thing: " + other);
        if (other.gameObject != transform.gameObject)
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
        var playerForwardMovement = Mathf.Abs(Vector3.Dot(logicalRigidBody.velocity, transform.forward));
        var playerRightMovement = Mathf.Abs(Vector3.Dot(logicalRigidBody.velocity, transform.right));

        var standingStill = (
            !turning
            && playerForwardMovement < minimumMoveAmountThreshold
            && playerRightMovement < minimumMoveAmountThreshold);

        var turningInPlace = (
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

    IEnumerator GoLimpAfterABit()
    {
        yield return new WaitForSeconds(0.5f);
        EnableRagdoll();
    }

    IEnumerator MonkeyPatchFlyingSumoWreslterBug()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            if (transform.position.y > 100000)
            {
                SceneManager.LoadScene(0);
            }
        }
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
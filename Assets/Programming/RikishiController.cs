using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RikishiController : MonoBehaviour
{
    public Transform shoveColliderPosition;
    public bool isThePlayer;
    public float shoveForce = 100f;
    Animator animator;
    RikishiController enemy;
    Rigidbody enemyRigidBody;

    bool isShoved = false;
    Vector3 shovedForce;

    Vector3 shoveColliderHalfExtents;

    bool enemyInRange = false;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        var rikishis = FindObjectsOfType<RikishiController>();
        enemy = rikishis[0] == this ? rikishis[1] : rikishis[0];
        enemyRigidBody = enemy.gameObject.GetComponent<Rigidbody>();
        //var referenceCollider = shoveColliderPosition.GetComponent<BoxCollider>();
        //Debug.Log(string.Format("Refrence collider extents: {0}", referenceCollider.bounds.extents));
        //shoveColliderHalfExtents = referenceCollider.bounds.extents / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isThePlayer) {
            return; 
        }

        if (Input.anyKeyDown) {
            animator.SetTrigger("Stop Dancing");
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Hadouken");
        }
    }

    void FixedUpdate()
    {
        if (isShoved) {
            this.animator.SetTrigger("Shoved");
            GetComponentInParent<Rigidbody>().AddForce(shovedForce);
            isShoved = false;
        }
    }

    void ShoveForce() {
        //Debug.Log("Animation event received");

        //var hits =  Physics.BoxCastAll(shoveColliderPosition.position, shoveColliderHalfExtents, shoveColliderPosition.forward);

        //Debug.Log(string.Format("Hit stuff: {0}", hits.Length));
        if (enemyInRange) {
            enemy.GetShoved((enemy.gameObject.transform.position - transform.position) * shoveForce);
        }
    }

    void GetShoved(Vector3 force) {
        isShoved = true;
        shovedForce = force;
        transform.parent.GetComponent<Rigidbody>().freezeRotation = false;
    }

    public void SetEnemyIsInRange(bool inRange) {
        Debug.Log("Enemy in range: " + inRange);
        this.enemyInRange = inRange;
    }
}

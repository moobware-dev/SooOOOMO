using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RikishiController : MonoBehaviour
{
    public bool isThePlayer;
    public float shoveForce = 100f;
    Animator animator;
    RikishiController enemy;
    Rigidbody enemyRigidBody;

    bool isShoved = false;
    Vector3 shovedForce;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        var rikishis = FindObjectsOfType<RikishiController>();
        enemy = rikishis[0] == this ? rikishis[1] : rikishis[0];
        enemyRigidBody = enemy.gameObject.GetComponent<Rigidbody>();
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

    private void FixedUpdate()
    {
        if (isShoved) {
            this.animator.SetTrigger("Shoved");
            GetComponentInParent<Rigidbody>().AddForce(shovedForce);
            isShoved = false;
        }
    }

    void ShoveForce() {
        Debug.Log("Animation event received");
        enemy.GetShoved((enemy.gameObject.transform.position - transform.position) * shoveForce);
    }

    void GetShoved(Vector3 force) {
        isShoved = true;
        shovedForce = force;
    }
}

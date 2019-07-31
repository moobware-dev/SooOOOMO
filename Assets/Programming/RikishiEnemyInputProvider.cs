using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RikishiController))]
[RequireComponent(typeof(ParameterizedFlabbiness))]
public class RikishiEnemyInputProvider : MonoBehaviour
{
    public float TrackSpeed;
    private Transform _trackingTransform;

    Transform mainCameraTransform;
    Transform playerTransform;

    RikishiController rikishiController;
    ParameterizedFlabbiness zeFlabben;
    Scoreboard scoreboard;

    Root behaviorTree = BT.Root();

    void Start()
    {
        mainCameraTransform = Camera.main.transform;
        playerTransform = GameObject.FindWithTag("Player").transform;
        rikishiController = GetComponent<RikishiController>();
        zeFlabben = GetComponent<ParameterizedFlabbiness>();
        scoreboard = FindObjectOfType<Scoreboard>();
        zeFlabben.Flabbiness = scoreboard.GetEnemyScore() * 34f;
        rikishiController.shoveForce = zeFlabben.Flabbiness * 3;

        _trackingTransform = new GameObject().transform;
        _trackingTransform.position = playerTransform.forward;

        // BT class from the 2D Game Kit
        // https://learn.unity.com/tutorial/2d-game-kit-advanced-topics#5c7f8528edbc2a002053b77a
        behaviorTree.OpenBranch(

            //BT.If(() => { return m_EnemyBehaviour.Target != null; }).OpenBranch(
            //    BT.Call(m_EnemyBehaviour.CheckTargetStillVisible),
            //    BT.Call(m_EnemyBehaviour.OrientToTarget),
            //    BT.Trigger(m_Animator, "Shooting"),
            //    BT.Call(m_EnemyBehaviour.RememberTargetPos),
            //    BT.WaitForAnimatorState(m_Animator, "Attack")
            //),

            //BT.If(() => { return m_EnemyBehaviour.Target == null; }).OpenBranch(
            //    BT.Call(m_EnemyBehaviour.ScanForPlayer)
            //),

            BT.Call(LockOntoTarget)
        );
    }

    void OnDrawGizmosSelected()
    {
        if (_trackingTransform == null)
        {
            return;
        }
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = Color.black;
        Gizmos.DrawCube(_trackingTransform.position, Vector3.one / 4);
    }

    void LockOntoTarget()
    {
        _trackingTransform.position = Vector3.Lerp(_trackingTransform.position, playerTransform.position, Time.deltaTime * TrackSpeed);
        rikishiController.SetDesiredAimTarget(_trackingTransform.position);
    }

    private void Update()
    {
        behaviorTree.Tick();
    }
}

/*
 *
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTAI;

namespace Gamekit2D
{
    public class SpitterBT : MonoBehaviour
    {
        Animator m_Animator;
        Damageable m_Damageable;
        Root m_Ai = BT.Root();
        EnemyBehaviour m_EnemyBehaviour;

        private void OnEnable()
        {
            m_EnemyBehaviour = GetComponent<EnemyBehaviour>();
            m_Animator = GetComponent<Animator>();

            m_Ai.OpenBranch(

                BT.If(() => { return m_EnemyBehaviour.Target != null; }).OpenBranch(
                    BT.Call(m_EnemyBehaviour.CheckTargetStillVisible),
                    BT.Call(m_EnemyBehaviour.OrientToTarget),
                    BT.Trigger(m_Animator, "Shooting"),
                    BT.Call(m_EnemyBehaviour.RememberTargetPos),
                    BT.WaitForAnimatorState(m_Animator, "Attack")
                ),

                BT.If(() => { return m_EnemyBehaviour.Target == null; }).OpenBranch(
                    BT.Call(m_EnemyBehaviour.ScanForPlayer)
                )
            );
        }

        private void Update()
        {
            m_Ai.Tick();
        }
    }
}
*/

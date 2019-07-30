using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RikishiController))]
[RequireComponent(typeof(ParameterizedFlabbiness))]
public class RikishiEnemyInputProvider : MonoBehaviour
{

    public float LockOnToTargetRotationSpeed = 1f;

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

    private Vector3 actualLookTarget = Vector3.zero;

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = Color.black;
        Gizmos.DrawCube(actualLookTarget, Vector3.one / 4);
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        var dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    void LockOntoTarget()
    {
        var localUp = transform.InverseTransformPoint(transform.up);
        var localForward = transform.InverseTransformPoint(transform.forward);
        var localDirectionOfPlayer = transform.InverseTransformDirection(playerTransform.position - transform.position);
        var angleNeededToFacePlayer = Vector3.SignedAngle(localForward, localDirectionOfPlayer, localUp);
        Debug.Log("local direction of player = " + localDirectionOfPlayer);
        Debug.Log("angle to needed to face player = " + angleNeededToFacePlayer);

        var angleAdjustedForTimeAndTurnSpeed = Mathf.Lerp(0, angleNeededToFacePlayer, Time.deltaTime * LockOnToTargetRotationSpeed);
        Debug.Log("how about this, does this make sense?" + angleAdjustedForTimeAndTurnSpeed);

        var localTarget = RotatePointAroundPivot(localForward, localUp, new Vector3(0, angleAdjustedForTimeAndTurnSpeed, 0));
        Debug.Log("local target: " + localTarget);
        var newLookTarget = transform.TransformPoint(localTarget);
        Debug.Log("world target: " + newLookTarget);

        actualLookTarget = newLookTarget;

        /*
         * I get numbers that make sense with this code, I just don't know what to do with them :/=-
        //var localUp = transform.InverseTransformPoint(transform.up);
        //var localForward = transform.InverseTransformPoint(transform.forward);
        //var localDirectionOfPlayer = transform.InverseTransformDirection(playerTransform.position - transform.position);
        //var angleNeededToFacePlayer = Vector3.SignedAngle(localForward, localDirectionOfPlayer, localUp);

        //Debug.Log("local direction of player = " + localDirectionOfPlayer);
        //Debug.Log("angle to needed to face player = " + angleNeededToFacePlayer);
        */


        //Vector3 targetDir = playerTransform.position - transform.position;

        //// The step size is equal to speed times frame time.
        //float step = LockOnToTargetRotationSpeed * Time.deltaTime;

        //Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        //Debug.DrawRay(transform.position, newDir, Color.black);

        //// Move our position a step closer to the target.
        //transform.rotation = Quaternion.LookRotation(newDir);

        //actualLookTarget = newLookTarget;

        //// Move our position a step closer to the target.
        //transform.rotation = Quaternion.LookRotation(newLookTarget);

        //Vector3 relativePos = playerTransform.position - transform.position;

        //// the second argument, upwards, defaults to Vector3.up
        //var idealRotation = Quaternion.LookRotation(relativePos, transform.up);
        ////transform.rotation = rotation;

        //var actualRotation = Quaternion.Lerp(transform.rotation, idealRotation, Time.deltaTime * LockOnToTargetRotationSpeed);

        //var newLookTarget = actualRotation * transform.forward;
        //Debug.DrawRay(transform.position, newLookTarget, Color.black);



        //var degreesToTurnToLookDirectlyAtTheTarget = Vector3.SignedAngle(transform.forward, playerTransform.position, transform.up);
        //var lerpedDegreesWellActuallyTurn = Mathf.LerpAngle(0, degreesToTurnToLookDirectlyAtTheTarget, Time.deltaTime * LockOnToTargetRotationSpeed);

        //actualLookTarget = Quaternion.AngleAxis(lerpedDegreesWellActuallyTurn, transform.up) * transform.forward;

        //Debug.Log("Degrees to turn to look at player = " + degreesToTurnToLookDirectlyAtTheTarget);
        //Debug.Log("Degrees we're turning this frame  = " + lerpedDegreesWellActuallyTurn);

        rikishiController.SetDesiredAimTarget(newLookTarget);
        ////rikishiController.SetDesiredAimTarget(idealLookTarget);
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

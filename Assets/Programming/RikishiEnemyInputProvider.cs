using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RikishiController))]
[RequireComponent(typeof(ParameterizedFlabbiness))]
public class RikishiEnemyInputProvider : MonoBehaviour {

    Transform mainCameraTransform;
    Transform playerTransform;

    RikishiController rikishiController;
    ParameterizedFlabbiness zeFlabben;
    Scoreboard scoreboard;

    void Start()
    {
        //mainCameraTransform = Camera.main.transform;
        playerTransform = GameObject.FindWithTag("Player").transform;
        rikishiController = GetComponent<RikishiController>();
        zeFlabben = GetComponent<ParameterizedFlabbiness>();
        scoreboard = FindObjectOfType<Scoreboard>();
        //zeFlabben.Flabbiness = scoreboard.GetEnemyScore() * 34f;
        //rikishiController.shoveForce = zeFlabben.Flabbiness * 3;
    }

    //void FixedUpdate()
    //{
    //    var lookInPlayersDirection = Vector3.ProjectOnPlane((playerTransform.position - transform.position), Vector3.up);
    //    rikishiController.SetDesiredAimTarget(lookInPlayersDirection);
    //    rikishiController.Move(Vector3.zero);
    //}
}

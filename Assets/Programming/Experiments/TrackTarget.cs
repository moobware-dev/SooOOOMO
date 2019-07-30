using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTarget : MonoBehaviour
{
    public float TrackingGizmoRadius = 0.3f;

    public Transform Target;
    public float TrackSpeed;

    private Vector3 _trackedPosition = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var distanceFromMeToYou = Target.position - transform.position;
        var angleFromMyFrontToYou = Vector3.SignedAngle(transform.forward, distanceFromMeToYou, Vector3.up);
        Debug.Log("muh angle: " + angleFromMyFrontToYou);

        var angleStep = angleFromMyFrontToYou * Time.deltaTime * TrackSpeed;
        Debug.Log("angle step: " + angleStep);

        var proportionateRotation = Quaternion.AngleAxis(angleStep, Vector3.up);
        _trackedPosition = transform.forward + proportionateRotation * transform.forward;

        transform.LookAt(_trackedPosition);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_trackedPosition, TrackingGizmoRadius);
    }
}

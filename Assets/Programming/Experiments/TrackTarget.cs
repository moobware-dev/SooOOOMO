using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTarget : MonoBehaviour
{
    public float TrackingGizmoRadius = 0.3f;

    public Transform Target;
    public float TrackSpeed;

    private Transform _trackingTransform;


    // Start is called before the first frame update
    void Start()
    {
        _trackingTransform = new GameObject().transform;
        _trackingTransform.position = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        _trackingTransform.position = Vector3.Lerp(_trackingTransform.position, Target.position, Time.deltaTime * TrackSpeed);
        transform.LookAt(_trackingTransform.position);
    }

    private void OnDrawGizmos()
    {
        if (_trackingTransform == null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_trackingTransform.position, TrackingGizmoRadius);
    }
}

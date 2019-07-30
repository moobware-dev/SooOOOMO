using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitStartingPoint : MonoBehaviour
{
    public float OrbitSpeed = 1f;
    public float OrbitRadius = 1f;
    private Vector3 _startPoint;

    // Start is called before the first frame update
    void Start()
    {
        _startPoint = transform.position;
        transform.position += new Vector3(0, 0, OrbitRadius);
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(_startPoint, Vector3.up, OrbitSpeed * Time.deltaTime);
    }
}

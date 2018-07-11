using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RikishiController : MonoBehaviour {

    public float MoveSpeed = 1f;

	void Start () {
		
	}
	
	void Update () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position = new Vector3(transform.position.x + (h * Time.deltaTime * MoveSpeed), 0, transform.position.z + (v * Time.deltaTime * MoveSpeed));
    }
}

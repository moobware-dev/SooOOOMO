using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemyAlertRikishi : MonoBehaviour {

    RikishiProtoController rikishi;

	// Use this for initialization
	void Start () {
        rikishi = GameObject.FindWithTag("Player").GetComponentInChildren<RikishiProtoController>();
        //Debug.Log("rikishi: " + rikishi);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("detected thing: " + other);
        if (other.CompareTag("Enemy")) {
            rikishi.SetEnemyIsInRange(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log("detected thing: " + other);
        if (other.CompareTag("Enemy"))
        {
            rikishi.SetEnemyIsInRange(false);
        }
    }
}

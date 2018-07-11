using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject annoyingBannerPrefab;

	// Use this for initialization
	void Start () {
        var canvas = GameObject.FindWithTag("UI");
        Instantiate(annoyingBannerPrefab, canvas.transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

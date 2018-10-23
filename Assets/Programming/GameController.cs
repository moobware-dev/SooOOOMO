using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject annoyingBannerPrefab;
    public GameObject firstSceneAlreadyLoadedNugget;

	// Use this for initialization
	void Start () {
        var bannerAlreadyShown = (GameObject.FindWithTag("FirstSceneAlreadyLoadedNugget") != null);
            if (!bannerAlreadyShown) {
            var canvas = GameObject.FindWithTag("UI");
            Instantiate(annoyingBannerPrefab, canvas.transform);
            Instantiate(firstSceneAlreadyLoadedNugget);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

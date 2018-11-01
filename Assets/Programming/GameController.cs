using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject annoyingBannerPrefab;
    public GameObject firstSceneAlreadyLoadedNugget;

	// Use this for initialization
	void Start () {

        var today = DateTime.Now;
        var halloween = new DateTime(today.Year, 10, 31);

        if (SceneManager.GetActiveScene().name != "HallowScene"
            && (today < halloween + TimeSpan.FromDays(1)
                || today > halloween - TimeSpan.FromDays(1))) {
            SceneManager.LoadScene("HallowScene");
            return; // XD
        }

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

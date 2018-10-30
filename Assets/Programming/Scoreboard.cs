using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour {

    public Text PlayerScore;
    public Text EnemyScore;

    private int playerScore;
    private int enemyScore;

    private const string PLAYER_SCORE_KEY = "PlayerScore";
    private const string ENEMY_SCORE_KEY = "EnemyScore";

	// Use this for initialization
	void Start () {
        var firstSceneAlreadyLoaded = (GameObject.FindWithTag("FirstSceneAlreadyLoadedNugget") != null);
        if (!firstSceneAlreadyLoaded)
        {
            Debug.Log("Hey jag do the thing!");
            PlayerPrefs.SetInt(PLAYER_SCORE_KEY, 0);
            PlayerPrefs.SetInt(ENEMY_SCORE_KEY, 0);
        }
        playerScore = PlayerPrefs.GetInt(PLAYER_SCORE_KEY);
        enemyScore = PlayerPrefs.GetInt(ENEMY_SCORE_KEY);

        PlayerScore.text = playerScore.ToString();
        EnemyScore.text = enemyScore.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RecordPlayerWin() {
        playerScore++;
        PlayerScore.text = playerScore.ToString();
        PlayerPrefs.SetInt(PLAYER_SCORE_KEY, playerScore);
    }

    public void RecordEnemyWin()
    {
        enemyScore++;
        EnemyScore.text = enemyScore.ToString();
        PlayerPrefs.SetInt(ENEMY_SCORE_KEY, enemyScore);
    }

    public int GetPlayerScore() {
        return playerScore;
    }

    public int GetEnemyScore() {
        return enemyScore;
    }
}

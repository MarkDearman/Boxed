using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    public Text Score;

	// Use this for initialization
	void Start ()
    {
        Score.text = "SCORE : " + PlayerPrefs.GetInt("currentScore").ToString();
    }
}

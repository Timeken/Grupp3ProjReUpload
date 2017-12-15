using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    Text text;
    static int score;

    public int Score
    {
        get { return score; }
        set { score += value; }
    }

	void Start ()
    {
        text = GameObject.Find("ScoreText").GetComponent<Text>();
        score = 0;
	}
	
	void Update ()
    {
        text.text = "Score: " + score;
    }
}
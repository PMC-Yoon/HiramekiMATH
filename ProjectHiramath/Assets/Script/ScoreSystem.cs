using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {
    private int Score;
    private Text TEXT;
	// Use this for initialization
	void Start () {
        TEXT = this.gameObject.GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {
        TEXT.text = string.Format("{0:D10}",Score);

    }

    public void ScorePlus(int nScore)
    {
        Score += nScore;
    }
}

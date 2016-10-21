using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {
    private int Score;
    private Text TEXT;
    private Text BorderText;
	// Use this for initialization
	void Start () {
        TEXT = this.gameObject.GetComponent<Text>();
        BorderText = GameObject.Find("BorderText").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        TEXT.text = string.Format("{0:D10}",Score);

    }

    public void ScorePlus(int nScore)
    {
        Score += nScore;
    }
    public int ScoreCheck()
    {
        return Score;
    }

    public void BorderSet(int nBorder)
    {
        BorderText.text = string.Format("{0,3}",nBorder);
    }
}

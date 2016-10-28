using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {
    private int Score;
    private int ProductScore;
    private Text TEXT;
    private Text BorderText;
    private float TimeCount;
    private float PlusSpeed;
    private int Border;
    // Use this for initialization
    void Start () {
        TEXT = this.gameObject.GetComponent<Text>();
        BorderText = GameObject.Find("BorderText").GetComponent<Text>();
        ProductScore = 0;
        TimeCount = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if(Score > ProductScore)
        {
            TimeCount += 1.0f * Time.deltaTime;
            // ProductScore += (Score - ProductScore) / 3;
            if (TimeCount >= PlusSpeed)
            {
                ProductScore++;
                TimeCount = 0;
            }
        }
        TEXT.text = string.Format("{0:D3}",ProductScore);

        if (Border <= Score)
        {
            TEXT.color = new Color(0.15f, 1.0f, 0, 1.0f);
        }
        else
        {
            TEXT.color = new Color(0, 0, 0, 1.0f);
        }
    }

    public void ScorePlus(int nScore)
    {
        Score += nScore;
        PlusSpeed = 0.1f / (Score - ProductScore);
    }

    public void ScoreUndo(int nScore)
    {
        Score -= nScore;
        ProductScore = Score;
    }
    public int ScoreCheck()
    {
        return Score;
    }

    public void BorderSet(int nBorder)
    {
        BorderText.text = string.Format("{0,3}",nBorder);
        Border = nBorder;
    }

    //Borderを取得する関数を追加
    public int GetBorderValue()
    {
        return Border;
    }
}

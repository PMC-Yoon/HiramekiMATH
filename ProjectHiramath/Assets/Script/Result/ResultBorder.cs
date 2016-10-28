using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ResultBorder : MonoBehaviour {

    GameObject score;
    // Use this for initialization
    void Start()
    {

        score = GameObject.Find("Score");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "" + score.GetComponent<ScoreSystem>().GetBorderValue();
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class pauseResultBorder : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        GetComponent<Text>().text = GameObject.Find("Score").GetComponent<ScoreSystem>().GetBorderValue() + "点以上とれ！";
	
	}
}

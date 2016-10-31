using UnityEngine;
using System.Collections;

public class Backbutton : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void BackButton()
    {
        GameObject.Find("Fade").GetComponent<Fade>().NextSceneName = "StageSelect";
        GameObject.Find("Fade").GetComponent<Fade>().FadeStart();

    }

}

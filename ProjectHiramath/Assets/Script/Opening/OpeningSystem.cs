using UnityEngine;
using System.Collections;

public class OpeningSystem : MonoBehaviour {

    public int SceneChangeTime;
    private float time;

	// Use this for initialization
	void Start () {
        time = 0;
	
	}
	
	// Update is called once per frame
	void Update () {

        time += Time.deltaTime;
        if( time > SceneChangeTime)
        {
            GetComponent<SceneChange>().SceneLoad();
        }
	
	}
}

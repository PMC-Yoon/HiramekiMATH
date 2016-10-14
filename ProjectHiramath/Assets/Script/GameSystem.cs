using UnityEngine;
using System.Collections;

public class GameSystem : MonoBehaviour {

    GameObject timer;
    GameObject BlockArray;
	// Use this for initialization
	void Start () {
        timer = GameObject.Find("Timer");
        BlockArray = GameObject.Find("BlockBoxArray");
	
	}
	
	// Update is called once per frame
	void Update () {

        if( timer.GetComponent<Timer>().GetTimerZeroFlg())
        {
            BlockArray.GetComponent<BlockBox>().CheckNumber();
        }


	}
}

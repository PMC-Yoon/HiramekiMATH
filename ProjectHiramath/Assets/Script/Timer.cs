using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {


    public float Seconds;
    Text text;
    


	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();

      



	}
	
	// Update is called once per frame
	void Update () {

        Seconds -= Time.deltaTime;

        if( Seconds < 0 )
        {
            Seconds = 0.0f;
        }

        text.text = Seconds.ToString("000");
  
	}


    public bool GetTimerZeroFlg()
    {
        if( Seconds < 0.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetTime(int time)
    {

    }
}

using UnityEngine;
using System.Collections;

public class GalileoSkill : MonoBehaviour {

    public GameObject HintBox;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetHintBox()
    {
        HintBox.SetActive(true);
    }
    public void HideHintBox()
    {
        HintBox.SetActive(false);

    }
}

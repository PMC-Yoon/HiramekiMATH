using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class BorderMessage : MonoBehaviour {


    Text text;

	// Use this for initialization
	void Start () {

        text = this.GetComponent<Text>();
        text.text = "";

	}
	
	// Update is called once per frame
	void Update () {

        int border = GameObject.Find("StageData").GetComponent<StageData>().GetStageBorder(CharacterSelectSystem.SelectCharacter, StageSelectSystem.SelectStage);
        text.text = border + "点以上をとれ！";


    }
}

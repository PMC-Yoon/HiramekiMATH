using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectCharacterPanelController : MonoBehaviour {

    //キャラクターの表示画像
    public Sprite GalileoSprite;
    public Sprite EinsteinSprite;
    public Sprite NewtonSprite;
    public Sprite ArchimedesSprite;

    

	// Use this for initialization
	void Start () {
	
	}

    public void CharacterPanelDataSet()
    {
        switch (CharacterSelectSystem.SelectCharacter)
        {
            case 0: //Galileo
                {
                    transform.FindChild("explanation").gameObject.GetComponent<Text>().text = "Galileoの説明文";
                    transform.FindChild("Skillexplanation").gameObject.GetComponent<Text>().text = "GalileoのSkill説明文";
                    transform.FindChild("CharacterName").gameObject.GetComponent<Text>().text = "ガリレオ";
                    transform.FindChild("CharacterImage").gameObject.GetComponent<Image>().sprite = GalileoSprite;
                    break;
                }
            case 1: //Archimedes
                {
                    transform.FindChild("explanation").gameObject.GetComponent<Text>().text = "Archimedesの説明文";
                    transform.FindChild("Skillexplanation").gameObject.GetComponent<Text>().text = "ArchimedesのSkill説明文";
                    transform.FindChild("CharacterName").gameObject.GetComponent<Text>().text = "アルキメデス";
                    transform.FindChild("CharacterImage").gameObject.GetComponent<Image>().sprite = ArchimedesSprite;
                    break;
                }
            case 2: //Einstein
                {
                    transform.FindChild("explanation").gameObject.GetComponent<Text>().text = "Einsteinの説明文";
                    transform.FindChild("Skillexplanation").gameObject.GetComponent<Text>().text = "EinsteinのSkill説明文";
                    transform.FindChild("CharacterName").gameObject.GetComponent<Text>().text = "アインシュタイン";
                    transform.FindChild("CharacterImage").gameObject.GetComponent<Image>().sprite = EinsteinSprite;
                    break;
                }
            case 3: //Newton
                {
                    transform.FindChild("explanation").gameObject.GetComponent<Text>().text = "Newtonの説明文";
                    transform.FindChild("Skillexplanation").gameObject.GetComponent<Text>().text = "NewtonのSkill説明文";
                    transform.FindChild("CharacterName").gameObject.GetComponent<Text>().text = "ニュートン";
                    transform.FindChild("CharacterImage").gameObject.GetComponent<Image>().sprite = NewtonSprite;
                    break;
                }

        }

    }
	

}

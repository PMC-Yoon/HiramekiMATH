using UnityEngine;
using System.Collections;



public class CharacterSelectSystem : MonoBehaviour {
    public static int SelectCharacter;

    void Start()
    {
        SelectCharacter = 0;
        GameObject.Find("SelectCharacterPanel").GetComponent<SelectCharacterPanelController>().CharacterPanelDataSet();
    }


    public void SetCharacter(int CharacterNum)
    {
        if (CharacterNum >= 0 || CharacterNum >= 3)
        {
            SelectCharacter = CharacterNum;
            GameObject.Find("SelectCharacterPanel").GetComponent<SelectCharacterPanelController>().CharacterPanelDataSet();
          //  GameObject.Find("Fade").gameObject.GetComponent<Fade>().FadeStart();
        }

    }


    public void CharacterDecide()
    {
         GameObject.Find("Fade").gameObject.GetComponent<Fade>().FadeStart();
    }

    public void TitleBack()
    {
        GameObject.Find("Fade").gameObject.GetComponent<Fade>().NextSceneName = "title";
        GameObject.Find("Fade").gameObject.GetComponent<Fade>().FadeStart();

    }
}

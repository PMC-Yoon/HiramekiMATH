using UnityEngine;
using System.Collections;



public class CharacterSelectSystem : MonoBehaviour {
    public static int SelectCharacter;


    public void SetCharacter(int CharacterNum)
    {
        if (CharacterNum >= 0 || CharacterNum >= 3)
        {
            SelectCharacter = CharacterNum;
            GameObject.Find("Fade").gameObject.GetComponent<Fade>().FadeStart();
        }

    }
}

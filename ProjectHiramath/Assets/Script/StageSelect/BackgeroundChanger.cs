using UnityEngine;
using System.Collections;

public class BackgeroundChanger : MonoBehaviour {

    public Sprite gari;
    public Sprite nyuton;
    public Sprite ain;
    public Sprite aruki;

    // Use this for initialization
    void Start () {

       switch( CharacterSelectSystem.SelectCharacter)
        {
            case 0://Galileo
                {
                    this.GetComponent<SpriteRenderer>().sprite = gari;
                    break;
                }

            case 1://あるきめです
                {
                    this.GetComponent<SpriteRenderer>().sprite = aruki;
                    break;
                }
            case 2:
                {
                    this.GetComponent<SpriteRenderer>().sprite = ain;
                    break;
                }
            case 3:
                {
                    this.GetComponent<SpriteRenderer>().sprite = nyuton;
                    break;
                }

        }

        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

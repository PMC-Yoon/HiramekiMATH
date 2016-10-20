using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BlockSelector : MonoBehaviour
{
    public Sprite[] NumBox;
    public int Number;
    private int PrevNum;
	// Use this for initialization
	void Start () {

        gameObject.GetComponent<SpriteRenderer>().sprite = NumBox[Number]; 
        PrevNum = Number;
    }
	
	// Update is called once per frame
	void Update () {
	    if(Number != PrevNum)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = NumBox[Number];
            PrevNum = Number;
        }
    }

    public void NumberChanger(bool bNumber,int nNum)
    {
        
        Number = nNum;
        if (bNumber)
        {
            //Debug.Log("BASS");
        }
        else
        {
          //  Debug.Log("KICK");
            Number = nNum + 10;
        }
    }
    
    public void Active()
    {
        this.gameObject.SetActive(false);
    }
   /* public void OnPointerDown(PointerEventData eventData)
    {
        Number = 11;
    }*/
}

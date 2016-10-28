using UnityEngine;
using System.Collections;

public class SelectOperator : MonoBehaviour {
    public GameObject FramePlus;
    public GameObject FrameMin;
    public GameObject FrameMulti;
    public GameObject FrameSkill;
    GameObject numarray;

	// Use this for initialization
	void Start () {
        numarray = GameObject.Find("BlockBoxArray");
        
	}
	
	// Update is called once per frame
	void Update () {

        FramePlus.SetActive(false);
        FrameMulti.SetActive(false);
        FrameMin.SetActive(false);
        FrameSkill.SetActive(false);
        switch (numarray.GetComponent<BlockBox>().GetChangeNum())
        {
            case 0:
                {//かける
                    FrameMulti.SetActive(true);
                    break;
                }
            case 1:
                {
                    FramePlus.SetActive(true);
                    break; 
                }

            case 2:
                {
                    FrameMin.SetActive(true);
                    break;
                }
            case 3:
                {
                    FrameSkill.SetActive(true);
                    break;
                }

        }

	
	}
}

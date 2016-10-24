using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class RemainingNumber : MonoBehaviour {
    GameObject blockarray;
    public int operatorNum;//演算子番号
    // Use this for initialization
	void Start () {
        blockarray = GameObject.Find("BlockBoxArray");
	
	}
	
	// Update is called once per frame
	void Update () {
        int hoge = blockarray.GetComponent<BlockBox>().NumberLastCheck(operatorNum);
        this.GetComponent<Text>().text = blockarray.GetComponent<BlockBox>().NumberLastCheck(operatorNum).ToString("00");
	
	}
}

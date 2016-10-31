using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreProduct : MonoBehaviour {
    private bool ProdFlag;
    private Text TEXT;
    private Transform ScorePos;
    private Vector3 MoveSize;
    private BlockBox Box;
    private Camera camera;
    // Use this for initialization
    void Start () {
        ProdFlag = false;
        ScorePos = GameObject.FindGameObjectWithTag("Score").transform;
        TEXT = GetComponent<Text>();
        Box = GameObject.Find("BlockBoxArray").GetComponent<BlockBox>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        if(ProdFlag)
        {
            if (TEXT.color.a < 1.0f)
            {
                Vector3 pos;
                TEXT.color = new Color(TEXT.color.r, TEXT.color.g, TEXT.color.b, TEXT.color.a + 1.0f * Time.deltaTime);
                pos = transform.position;
                pos.y += 1.0f * Time.deltaTime;
                //transform.position = pos;
                MoveSize = (ScorePos.position - transform.position).normalized;
                MoveSize *= 200.0f;
            }
            else
            {
                transform.Translate(MoveSize * Time.deltaTime);
            }

            if(ScorePos.position.y - transform.position.y <= 1.0f)
            {
                TEXT.color = new Color(TEXT.color.r, TEXT.color.g, TEXT.color.b, 0.0f);
                Box.CheckNumber();
                ProdFlag = false;
            }

        }
	
	}

    public void Product(int Score,Vector3 pos)
    {
        Debug.Log("落とした！");
        TEXT.text = string.Format("{0:D}", Score);
        ProdFlag = true;
        // transform.localPosition = camera.WorldToViewportPoint(pos);
        transform.position = pos;
       // transform.localPosition = pos;
        Debug.Log(pos);
    }
}

//===========================================================================================
//    スプライトアニメーション関数
//    制作者：福岡利基
//    概要
//    2Dアニメーションを再生する関数。
//　　テクスチャ、アニメの長さ、数、切り替え時間などを登録することで自動的に再生してくれる。
//===========================================================================================

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour {
    public Sprite[] TexBox; //テクスチャ格納
    private int[] AnimNum; //アニメ開始番号
    public int[] AnimSize; //アニメ枚数
    public float[] Second;
    private int NowAnim;
    private int AnimKind; //現在のアニメの種類
    private float nCount;
    private bool Pause;

    public bool canvas;
    private bool AnimEnd;
    public bool Loop;

    // Use this for initialization
    void Start () {
        AnimNum = new int[AnimSize.Length];
        AnimNum[0] = 0;
        for (int n = 1; n < AnimSize.Length; n++)
        {
            AnimNum[n] = AnimNum[n - 1] + AnimSize[n - 1];
        }
        NowAnim = 0;
        AnimKind = 0;
        nCount = 0;

        Pause = false;

        if(canvas)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Image>().enabled = false;
        }

        AnimEnd = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!Pause)
        {
            nCount += 1 * Time.deltaTime;
        }
        else
        {
            nCount += 1 * Time.unscaledDeltaTime;
        }
         if(nCount >= Second[AnimKind])
         {
             nCount = 0;
            NowAnim++;
            if (NowAnim - AnimNum[AnimKind] >= AnimSize[AnimKind])
            {
                if (Loop)
                {
                    NowAnim = AnimNum[AnimKind];
                }
                else
                {
                    ResetAnim();
                    this.gameObject.SetActive(false);
                }
            }
            if (!canvas)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = TexBox[NowAnim];
            }
            else
            {
                gameObject.GetComponent<Image>().sprite = TexBox[NowAnim];
            }

        }  
	}

    public void ChangeAnim(int nNum)
    {
        if (nNum < AnimSize.Length)
        {
			if (AnimKind != nNum) {
				NowAnim = AnimNum [nNum];
			}
				AnimKind = nNum;
			
        }
    }

    public void PauseAnim()
    {
        Pause = true;
    }

    public void NormalAnim()
    {
        Pause = false;
    }

    public void ChangeMode()
    {
        canvas = canvas ? false : true;
        if (canvas)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Image>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<Image>().enabled = false;
        }
    }

    public void ResetAnim()
    {
        NowAnim = AnimNum[AnimKind];
    }

}

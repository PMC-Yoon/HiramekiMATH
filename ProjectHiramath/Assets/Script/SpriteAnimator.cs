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

    }
	
	// Update is called once per frame
	void Update () {
        nCount += 1 * Time.deltaTime;
         if(nCount >= Second[AnimKind])
         {
             nCount = 0;
            NowAnim++;
            if (NowAnim - AnimNum[AnimKind] >= AnimSize[AnimKind])
                NowAnim = AnimNum[AnimKind];
            gameObject.GetComponent<SpriteRenderer>().sprite = TexBox[NowAnim];

        }  
	}

    public void ChangeAnim(int nNum)
    {
        if (nNum < AnimSize.Length)
        {
            NowAnim = AnimNum[nNum];
            AnimKind = nNum;
        }
    }
}

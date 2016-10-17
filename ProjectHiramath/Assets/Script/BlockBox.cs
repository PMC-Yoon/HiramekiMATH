
//ブロック配列の実装
//Terabayashi,Kazuaki
//

using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class BlockBox : MonoBehaviour {

    //構造体定義
    public struct BLOCKBOX
    {
        public int data;//情報
        public bool Number;//trueなら番号と判断する
        public bool use;//使用しているか
        public Vector3 pos;

        //追加人：福岡
        public bool Delete; //消えた
        public GameObject Block;
    }

    //宣言
    public BLOCKBOX[,] a_Block = new BLOCKBOX[6, 9];

    GameObject puzzlebackground;

    public GameObject NumberBlock; //ブロック割当
    private ScoreSystem Score;
    private float nCount;
    public float BlockTime; //せり出してくるまでの時間
    private bool EraseFlag;
    
    void Awake()
    {

    }

    void Start()
    {
        //ブロックの中心座標を求める処理 2016/10/11 KazuakiTerabayashi 
        puzzlebackground = GameObject.Find("puzzleBackground");
        
        //puzzleの背景のwidth,heightを取得する
        float width;
        float height;
        width = puzzlebackground.GetComponent<SpriteRenderer>().bounds.size.x;
        height = puzzlebackground.GetComponent<SpriteRenderer>().bounds.size.y;


        

        //ブロックの幅を求める
        float blockwidth;
        if (NumberBlock != null)
        {
            blockwidth = width / 6.0f * 0.5f;
        }
        else
        {
            //仮の値を入れておく
            blockwidth = 0.35f;
        }
        //背景の位置
        Vector3 posBackground = puzzlebackground.transform.position;

        //１個１個の間隔をもとめる
        float intervalX, intervalY;
        intervalX = width / 6;
        intervalY = height / 9;

        //一番左上の位置
        Vector3 poLU = new Vector3(posBackground.x - (width / 2) + blockwidth, posBackground.y + (height/2) - blockwidth, 0);

        //位置情報代入
        for( int x = 0, y = 0;y < 9; x++)
        {

            a_Block[x, y].pos = new Vector3(poLU.x + (intervalX * x), poLU.y - (intervalY * y), 0);

            if (x == 5)
            {
                x = -1;
                y++;
            }
        }

        //ブロックの中心座標を求める処理 2016/10/11 KazuakiTerabayashi ここまで



        //数字のランダム配置 追加　福岡　2016/10/11 ここから
        int nNum;
        for (int n = 0; n < 6; n++)
        {
            for (int m = 0; m < 9; m++)
            {
                a_Block[n, m].Block = Instantiate(NumberBlock) as GameObject;
                a_Block[n, m].Block.transform.SetParent(this.transform);
                a_Block[n, m].Block.transform.localPosition = a_Block[n, m].pos;
                a_Block[n, m].Block = a_Block[n, m].Block;
                //deleteData(n, m);

                /* if (Random.Range(0, 101) >= 75)
                 {
                     nNum = Random.Range(0, 3);
                     addData(n, m, false, nNum);

                 }
                 else
                 {
                     nNum = Random.Range(0, 10);
                     addData(n, m, true, nNum);
                 }*/

                a_Block[n, m].use = false;
                a_Block[n, m].Delete = false;
                a_Block[n, m].Block.SetActive(false);

                //追記　Terabayashi
                /* if (m > 4)
                 {

                     a_Block[n, m].use = false;
                     a_Block[n, m].Delete = false;
                     a_Block[n, m].Block.SetActive(false);
                 }*/
                //追記ここまで
            }
        }

        BlockGenerate(6, 9);
        for (int n = 0; n < 6; n++)
        {
            for (int m = 0; m < 9; m++)
            {
                if (m > 4)
                {

                    a_Block[n, m].use = false;
                    a_Block[n, m].Delete = false;
                    a_Block[n, m].Block.SetActive(false);
                }
           }
        }
                //数字のランダム配置 追加　福岡　2016/10/11 ここまで

                //スコアシステムの発見 2016/10/12
                Score = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreSystem>();
        nCount = 0;

        EraseFlag = false;
    }

    void Update()
    {
        nCount += 1.0f * Time.deltaTime;
        if(nCount >= BlockTime && !EraseFlag)
        {
            BlockAdvance();
            nCount = 0;
        }
        //計算処理　追加　福岡　2016/10/11 ここから
        if (Input.GetMouseButtonDown(1) || EraseFlag) //計算フラグ成立
        {
            CheckNumber();
            EraseFlag = false;
        }
        //計算処理　追加　福岡　2016/10/11 ここまで
    }



    //計算処理　追加　福岡　2016/10/11 ここから
    public void CheckNumber()
    {
        for (int n = 0; n < 6; n++)
        {
            for (int m = 0; m < 9; m++)
            {
                if (a_Block[n, m].use)
                {
                    if (n < 4 && a_Block[n + 1, m].use && a_Block[n + 2, m].use && ((a_Block[n, m].Number == true) && (a_Block[n + 1, m].Number == false)) && ((a_Block[n, m].Number == true) && (a_Block[n + 2, m].Number == true)))
                    {
                        //Debug.Log("ﾇｯ");
                        a_Block[n, m].Delete = true;
                        a_Block[n + 1, m].Delete = true;
                        a_Block[n + 2, m].Delete = true;
                        if (a_Block[n + 1, m].data == 1)
                        {
                            Debug.Log(a_Block[n, m].data + "+" + a_Block[n + 2, m].data);
                            Score.ScorePlus(a_Block[n, m].data + a_Block[n + 2, m].data);
                        }
                        else if (a_Block[n + 1, m].data == 2)
                        {
                            Debug.Log(a_Block[n, m].data + "-" + a_Block[n + 2, m].data);
                            Score.ScorePlus(a_Block[n, m].data - a_Block[n + 2, m].data);
                        }
                        else
                        {
                            Debug.Log(a_Block[n, m].data + "*" + a_Block[n + 2, m].data);
                            Score.ScorePlus(a_Block[n, m].data * a_Block[n + 2, m].data);
                        }
                    }
                    if (m < 7 && a_Block[n, m + 1].use && a_Block[n, m + 2].use && ((a_Block[n, m].Number == true) && (a_Block[n, m + 1].Number == false)) && ((a_Block[n, m].Number == true) && (a_Block[n, m + 2].Number == true)))
                    {
                        //Debug.Log("ﾇｯ");
                        a_Block[n, m].Delete = true;
                        a_Block[n, m + 1].Delete = true;
                        a_Block[n, m + 2].Delete = true;
                        if (a_Block[n, m + 1].data == 1)
                        {
                            Debug.Log(a_Block[n, m].data + "+" + a_Block[n, m + 2].data);
                            Score.ScorePlus(a_Block[n, m].data + a_Block[n, m + 2].data);
                        }
                        else if (a_Block[n, m + 1].data == 2)
                        {
                            Debug.Log(a_Block[n, m].data + "-" + a_Block[n, m + 2].data);
                            Score.ScorePlus(a_Block[n, m].data - a_Block[n, m + 2].data);
                        }
                        else
                        {
                            Debug.Log(a_Block[n, m].data + "*" + a_Block[n, m + 2].data);
                            Score.ScorePlus(a_Block[n, m].data * a_Block[n, m + 2].data);
                        }
                    }
                }
            }
        }

        for (int n = 0; n < 6; n++)
        {
            for (int m = 0; m < 9; m++)
            {
                if (a_Block[n, m].Delete)
                {
                    deleteData(n, m);
                }
            }
        }

        FallBlock();
    }
    //計算処理　追加　福岡　2016/10/11 ここまで

    //ブロックを上に詰める
    public void FallBlock()
    {
        int nNum = 0;
        for (int n = 0; n < 6; n++)
        {
            for (int m = 0; m < 8; m++)
            {
                nNum = m + 1;
                if(!a_Block[n,m].use)
                {
                    while (nNum <= 8 && !a_Block[n, nNum].use)
                        nNum++;
                    if (nNum <= 8)
                    {
                        addData(n, m, a_Block[n, nNum].Number, a_Block[n, nNum].data);
                        deleteData(n, nNum);
                    }
                }
            }
        }
    }

    //ブロックせり出し
    public void BlockAdvance()
    {
        for (int n = 5; n >= 0; n--)
        {
            for (int m = 8; m > 0; m--)
            {
                if (a_Block[n, m].use)
                {
                    deleteData(n, m);
                    addData(n, m, a_Block[n, m - 1].Number, a_Block[n, m - 1].data);
                }
                else
                {
                    a_Block[n, m].Number = a_Block[n, m - 1].Number;
                    a_Block[n, m].data = a_Block[n, m - 1].data;
                    if(a_Block[n, m - 1].use)
                    {
                        addData(n, m, a_Block[n, m].Number, a_Block[n, m].data);
                    }
                    
                }
            }
        }

        for (int n = 0; n < 6; n++)
         {
             int nNum;
             deleteData(n,0);
            /* if (Random.Range(0, 101) >= 75)
             {
                 nNum = Random.Range(0, 3);
                 addData(n, 0, false, nNum);

             }
             else
             {
                 nNum = Random.Range(0, 10);
                 addData(n, 0, true, nNum);
             }*/
         }

        BlockGenerate(6, 1);
    } 


    //そのデータが使われているかチェック 追加　福岡　2016/10/11ここから
    public bool SpaceCheck(int x, int y)
    {
        return a_Block[x, y].use;
    }
    //そのデータが使われているかチェック 追加　福岡　2016/10/11ここまで




    //構造体データ入手関数
    public BLOCKBOX GetData(int x, int y)
    {
        return a_Block[x, y];
    }


    //データセット関数（　配列X位置, 配列Y位置 , 番号データかどうか , データ）
    public bool addData(int arrayposx , int arrayposy, bool Number , int data )
    {
        if(a_Block[arrayposx, arrayposy].use)
        {
            return false;
        }

        a_Block[arrayposx, arrayposy].Number = Number;
        a_Block[arrayposx, arrayposy].data = data;
        a_Block[arrayposx, arrayposy].use = true;

        //データ項目追加　hukuoka
        a_Block[arrayposx, arrayposy].Delete = false;
        a_Block[arrayposx, arrayposy].Block.SetActive(true);
        a_Block[arrayposx, arrayposy].Block.GetComponent<BlockSelector>().NumberChanger(Number, data);
        
        return true;

    }

    //削除関数（配列位置X,配列位置Y）
    public bool deleteData(int arrayposX, int arrayposY )
    {
        if( !a_Block[arrayposX,arrayposY].use)
        {
            //その場所には何もない
            return false;
        }


        //削除項目追加　hukuoka
        a_Block[arrayposX, arrayposY].Delete = false;
        a_Block[arrayposX, arrayposY].Block.SetActive(false);

        return true;
    }


    //その配列の一番下のY座標を返す
    public int GetArrayY(int X)
    {
        for(int i = 0; i < 9; i ++)
        {
            if( a_Block[X,i].use == false)
            {
                return i - 1;
            }
        }
        return -1;//エラー
    }

    public void EraseAwake()
    {
        EraseFlag = true;
    }

    public void BlockGenerate(int x,int y)
    {
        int nNum;
        for(int n = 0; n < x; n++)
        {
            for(int m = 0; m < y; m++)
            {
                if (Random.Range(0, 101) >= 75)
                {
                    nNum = Random.Range(0, 3);
                    addData(n, m, false, nNum);

                }
                else
                {
                    nNum = Random.Range(0, 10);
                    addData(n, m, true, nNum);
                }
            }
        }

    }


}


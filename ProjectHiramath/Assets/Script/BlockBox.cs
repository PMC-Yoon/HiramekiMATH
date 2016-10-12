
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
            blockwidth = NumberBlock.GetComponent<SpriteRenderer>().bounds.size.x * 0.5f;
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
                /* if (n == 0 && m == 0)
                     a_Block[n, m].pos = Vector3.zero;
                 else
                 {
                     a_Block[n, m].pos.x = 0.7f * n;
                     a_Block[n, m].pos.y = -0.7f * m;
                     a_Block[n, m].pos.z = 0;
                 }*/
             //    a_Block[n, m].pos.x = 0.68f * n;
              //  a_Block[n, m].pos.y = -0.65f * m;
              //  a_Block[n, m].pos.z = 0;



                a_Block[n, m].Block = Instantiate(NumberBlock) as GameObject;
                a_Block[n, m].Block.transform.SetParent(this.transform);
                a_Block[n, m].Block.transform.localPosition = a_Block[n, m].pos;
                a_Block[n, m].Block = a_Block[n, m].Block;
                //a_Block[n, m].Block.SetActive(false);


                if (Random.Range(0, 101) >= 75)
                {
                    nNum = Random.Range(0, 3);
                    addData(n, m, false, nNum);
                    //a_Block[n, m].Block.transform.GetComponent<BlockSelector>().NumberChanger(false, nNum);
                    //a_Block[n, m].Block.SetActive(false);

                }
                else
                {
                    nNum = Random.Range(0, 10);
                    addData(n, m, true, nNum);
                    //a_Block[n, m].Block.transform.GetComponent<BlockSelector>().NumberChanger(true, nNum);
                }

            }
        }
        //数字のランダム配置 追加　福岡　2016/10/11 ここまで




    }

    void Update()
    {
        //計算処理　追加　福岡　2016/10/11 ここから
        if (Input.GetMouseButtonDown(1)) //計算フラグ成立
        {
            CheckNumber();
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
                if (n < 4 && ((a_Block[n, m].Number == true) && (a_Block[n + 1, m].Number == false)) && ((a_Block[n, m].Number == true) && (a_Block[n + 2, m].Number == true)))
                {
                    //Debug.Log("ﾇｯ");
                    a_Block[n, m].Delete = true;
                    a_Block[n + 1, m].Delete = true;
                    a_Block[n + 2, m].Delete = true;
                }
                if (m < 7 && ((a_Block[n, m].Number == true) && (a_Block[n, m + 1].Number == false)) && ((a_Block[n, m].Number == true) && (a_Block[n, m + 2].Number == true)))
                {
                    //Debug.Log("ﾇｯ");
                    a_Block[n, m].Delete = true;
                    a_Block[n, m + 1].Delete = true;
                    a_Block[n, m + 2].Delete = true;
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
                    //GameObject.FindGameObjectWithTag("hoge").transform.GetChild((n + 1) * (m + 1) - 1).gameObject.SetActive(false);
                }
            }
        }
    }
    //計算処理　追加　福岡　2016/10/11 ここまで


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
        a_Block[arrayposx, arrayposy].Block.GetComponent<BlockSelector>().NumberChanger(Number, data);
        a_Block[arrayposx, arrayposy].Block.SetActive(true);
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

        //データを初期化させる
        a_Block[arrayposX, arrayposY].use = false;
        a_Block[arrayposX, arrayposY].Number = false;
        a_Block[arrayposX, arrayposY].data = 0;

        //削除項目追加　hukuoka
        a_Block[arrayposX, arrayposY].Delete = false;
        a_Block[arrayposX, arrayposY].Block.SetActive(false);

        return true;
    }



}


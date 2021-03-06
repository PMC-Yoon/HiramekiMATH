﻿
//ブロック配列の実装
//Terabayashi,Kazuaki
//

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;


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

        public GameObject Eff;
    }

    //宣言
    public BLOCKBOX[,] a_Block = new BLOCKBOX[6, 6];
    private int[] NumberLast;

    GameObject puzzlebackground;

    public GameObject NumberBlock; //ブロック割当
    public GameObject EraseEffect; //消去エフェクト
    private ScoreSystem Score;
    private float nCount;
    public float BlockTime; //せり出してくるまでの時間
    private bool EraseFlag;

    private int ChangeNum; //変換記号

    private int AreaWidth;  //横に何個並ぶか
    private int AreaHeight; //縦に何個並ぶか

    private int Border; //ボーダー仮収納箱
    private bool bEnd;

    private StageData stageData;
    private int CharaNum;
    private int StageNum;

    private BLOCKBOX[,] LastBlock;
    private int LastScore;
    private int LastSymbol;
    private bool UndoFlag;

    private int EraseBlockX;
    private int EraseBlockY;
    private bool EraseHorizontal;
    private bool EraseVertical;

    public float RotateTime;
    private Vector3 DestScale;

    private bool ProductFlag;
    private bool ProductNowFlag;

    private string[] ButtonName = new string[4];
    private BorderProduct borderProduct;

	private Character_Box Player;
	private Character_Box Enemy;
	private AnimationManager anim;
	private int[,] EnemyCharaBox;

    void Awake()
    {

    }

    void Start()
    {
        AreaWidth = 6;
        AreaHeight = 6;

        //ブロックの中心座標を求める処理 2016/10/11 KazuakiTerabayashi 
        puzzlebackground = GameObject.Find("puzzleBackground");
        
        //puzzleの背景のwidth,heightを取得する
        float width;
        float height;
        width = puzzlebackground.GetComponent<SpriteRenderer>().bounds.size.x;
        height = puzzlebackground.GetComponent<SpriteRenderer>().bounds.size.y;

        ChangeNum = -1;
        

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
        intervalX = width / AreaWidth;
        intervalY = height / AreaHeight;

        //一番左上の位置
        Vector3 poLU = new Vector3(posBackground.x - (width / 2) + blockwidth, posBackground.y + (height/2) - blockwidth, 0);

        //位置情報代入
        for( int x = 0, y = 0;y < AreaHeight; x++)
        {

            a_Block[x, y].pos = new Vector3(poLU.x + (intervalX * x), poLU.y - (intervalY * y), 0);

            if (x == 5)
            {
                x = -1;
                y++;
            }
        }

        //ブロックの中心座標を求める処理 2016/10/11 KazuakiTerabayashi ここまで

        NumberLast = new int[4];
        // Border = 444;

        StageNum = StageSelectSystem.SelectStage;
        CharaNum = CharacterSelectSystem.SelectCharacter;

        Debug.Log(CharaNum);
        Debug.Log(StageNum);

        stageData = GameObject.Find("StageData").GetComponent<StageData>();

        stageData.StageLoad(CharaNum, StageNum, a_Block, ref Border, NumberLast);

        StageLoad();


        //数字の初期配置配置 追加　福岡　2016/10/11 ここから
        int nNum;
        for (int n = 0; n < AreaWidth; n++)
        {
            for (int m = 0; m < AreaHeight; m++)
            {
                a_Block[n, m].Block = Instantiate(NumberBlock) as GameObject;
                a_Block[n, m].Block.transform.SetParent(this.transform);
                a_Block[n, m].Block.transform.localPosition = a_Block[n, m].pos;
                a_Block[n, m].Block = a_Block[n, m].Block;

                if (a_Block[n, m].data < 0)
                {
                    a_Block[n, m].use = true;
                    deleteData(n, m);
                }
                else
                {
                    a_Block[n, m].Number = true;
                    addData(n, m, true, a_Block[n, m].data);
                }

                a_Block[n, m].Eff = Instantiate(EraseEffect) as GameObject;
                a_Block[n, m].Eff.transform.SetParent(this.transform);
                a_Block[n, m].Eff.transform.localPosition = a_Block[n, m].pos;
                a_Block[n, m].Eff.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                a_Block[n, m].Eff.SetActive(false);
            }
        }
        //数字のランダム配置 追加　福岡　2016/10/11 ここまで

        //スコアシステムの発見 2016/10/12
        Score = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreSystem>();
        Score.BorderSet(Border);
        nCount = 0;

        EraseFlag = false;

        ChangeNum = -1; //最初は何も起きない

        bEnd = false;

        LastBlock = new BLOCKBOX[6,6];
        LastSymbol = -1;
        LastScore = 0;

        EraseBlockX = 0;
        EraseBlockY = 0;
        EraseHorizontal = false;
        EraseVertical = false;

        ProductFlag = false;
        ProductNowFlag = false;

        ButtonName[0] = "MultiButton";
        ButtonName[1] = "PlusButton";
        ButtonName[2] = "MinusButton";
        ButtonName[3] = "SkillButton";

        borderProduct = GameObject.Find("BorderProduct").GetComponent<BorderProduct>();

        borderProduct.BorderIn();

		EnemyCharaBox = new int[4, 4];
		EnemyCharaBox[0,0] = 2;
		EnemyCharaBox[0,1] = 2;
		EnemyCharaBox[0,2] = 1;
		EnemyCharaBox[0,3] = 3;

		Player = GameObject.Find ("CharacterBox").GetComponent<Character_Box> ();
		Enemy = GameObject.Find ("CharacterBox_2").GetComponent<Character_Box> ();
		//anim = GameObject.Find ("AnimationManager").GetComponent<AnimationManager>();
		Player.CharacterChange (CharaNum);
		Enemy.CharacterChange (EnemyCharaBox[CharaNum,StageNum]);
		//anim.PlayerCharaChange (CharaNum);
		//anim.EnemyCharaChange (EnemyCharaBox[CharaNum,StageNum]);
    }

    void StageLoad()
    {
        /*string FileName;
        int Stage;
        TextAsset CSV;
        List<string[]> StageStatus = new List<string[]>();
        FileName = "stage_";
        Stage = 0;
        CSV = Resources.Load(FileName + Stage) as TextAsset;
        StringReader reader = new StringReader(CSV.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            StageStatus.Add(line.Split(','));
        }

        //EnemyPos = new EnemyBox[StageStatus.Count]; //ファイルの長さを取得し、それに合わせて長さを変更
        //ENEMY = new GameObject[StageStatus.Count];

        // int ListCount = 0;
        for (int n = 0; n < AreaWidth; n++)
        {
            for (int m = 0; m < AreaHeight; m++)
            {
                a_Block[n,m].Number = true;
                a_Block[n,m].data = int.Parse(StageStatus[m][n]);
            }
        }

        Border = int.Parse(StageStatus[AreaHeight][0]);

        for (int n = 0; n < NumberLast.Length; n++)
        {
            NumberLast[n] = int.Parse(StageStatus[AreaHeight][n + 1]);
        }*/

       /* Debug.Log(Border);
        Debug.Log(NumberLast[0]);
        Debug.Log(NumberLast[1]);
        Debug.Log(NumberLast[2]);
        Debug.Log(NumberLast[3]);*/
    }

    void Update()
    {
        CrearCheck();
        if (ChangeNum >= 0)
        {
            EraseCheck();
        }
        //計算処理　追加　福岡　2016/10/11 ここから
        if(Input.GetMouseButtonDown(0) && !EraseFlag)
        {
            //if分追加　2016/10/20 terabayashi
            if ((int)Time.timeScale == 1)
            {
                ChangeBlock();
                
            }
        }
        /*if (Input.GetMouseButtonDown(1) || EraseFlag) //計算フラグ成立
        
            CheckNumber();
            EraseFlag = false;
        }*/

        if(EraseFlag)
        {
            float smallsize = 2.0f * Time.deltaTime;
            int score = 0;
            if (ChangeNum != 3)
            {
                if (!ProductNowFlag)
                {
                    if (a_Block[EraseBlockX, EraseBlockY].Block.transform.localEulerAngles.y <= 180.0f)
                    {
                        a_Block[EraseBlockX, EraseBlockY].Block.transform.Rotate(0, (180 / RotateTime) * Time.deltaTime, 0);
                        DestScale = a_Block[EraseBlockX, EraseBlockY].Block.transform.localScale;
                    }

                    if (a_Block[EraseBlockX, EraseBlockY].Block.transform.localEulerAngles.y > 180.0f)
                    {
                        a_Block[EraseBlockX, EraseBlockY].Block.transform.localScale -= new Vector3(smallsize, smallsize, smallsize);
                        if (EraseHorizontal)
                        {
                            a_Block[EraseBlockX - 1, EraseBlockY].Block.transform.localScale -= new Vector3(smallsize, smallsize, smallsize);
                            a_Block[EraseBlockX + 1, EraseBlockY].Block.transform.localScale -= new Vector3(smallsize, smallsize, smallsize);
                        }
                        if (EraseVertical)
                        {
                            a_Block[EraseBlockX, EraseBlockY - 1].Block.transform.localScale -= new Vector3(smallsize, smallsize, smallsize);
                            a_Block[EraseBlockX, EraseBlockY + 1].Block.transform.localScale -= new Vector3(smallsize, smallsize, smallsize);
                        }

                        if (a_Block[EraseBlockX, EraseBlockY].Block.transform.localScale.x < 0.0f && !ProductNowFlag)
                        {
                            a_Block[EraseBlockX, EraseBlockY].Eff.SetActive(true);
                            if (EraseHorizontal)
                            {
                                switch (a_Block[EraseBlockX, EraseBlockY].data)
                                {
                                    case 0:
                                        score += (a_Block[EraseBlockX - 1, EraseBlockY].data * a_Block[EraseBlockX + 1, EraseBlockY].data);
                                        break;
                                    case 1:
                                        score += (a_Block[EraseBlockX - 1, EraseBlockY].data + a_Block[EraseBlockX + 1, EraseBlockY].data);
                                        break;
                                    case 2:
                                        score += (a_Block[EraseBlockX - 1, EraseBlockY].data - a_Block[EraseBlockX + 1, EraseBlockY].data);
                                        break;
                                }
                                a_Block[EraseBlockX - 1, EraseBlockY].Eff.SetActive(true);
                                a_Block[EraseBlockX + 1, EraseBlockY].Eff.SetActive(true);
                            }
                            if (EraseVertical)
                            {
                                switch (a_Block[EraseBlockX, EraseBlockY].data)
                                {
                                    case 0:
                                        score += (a_Block[EraseBlockX, EraseBlockY - 1].data * a_Block[EraseBlockX, EraseBlockY + 1].data);
                                        break;
                                    case 1:
                                        score += (a_Block[EraseBlockX, EraseBlockY - 1].data + a_Block[EraseBlockX, EraseBlockY + 1].data);
                                        break;
                                    case 2:
                                        score += (a_Block[EraseBlockX, EraseBlockY - 1].data - a_Block[EraseBlockX, EraseBlockY + 1].data);
                                        break;
                                }
                                a_Block[EraseBlockX, EraseBlockY - 1].Eff.SetActive(true);
                                a_Block[EraseBlockX, EraseBlockY + 1].Eff.SetActive(true);
                            }
                            //GameObject.Find("ScoreProduct").GetComponent<ScoreProduct>().Product(score, RectTransformUtility.WorldToScreenPoint(Camera.main, a_Block[EraseBlockX, EraseBlockY].Block.transform.position));
                            GameObject.Find("ScoreProduct").GetComponent<ScoreProduct>().Product(score,  a_Block[EraseBlockX, EraseBlockY].Block.transform.position);
                            Debug.Log("捕鯨");
                            //CheckNumber();
                            ProductNowFlag = true;
                        }
                        
                    }


                    else if (a_Block[EraseBlockX, EraseBlockY].Block.transform.localEulerAngles.y > 90.0f && a_Block[EraseBlockX, EraseBlockY].Number != false)
                    {
                        Debug.Log("hoge");
                        deleteData(EraseBlockX, EraseBlockY);
                        addData(EraseBlockX, EraseBlockY, false, ChangeNum);

                    }
                }
                else
                {
                    if (ProductFlag)
                    {
                        a_Block[EraseBlockX, EraseBlockY].Block.transform.localEulerAngles = Vector3.zero;
                        a_Block[EraseBlockX, EraseBlockY].Block.transform.localScale = DestScale;
                        if (EraseHorizontal)
                        {
                            a_Block[EraseBlockX - 1, EraseBlockY].Block.transform.localScale = DestScale;
                            a_Block[EraseBlockX + 1, EraseBlockY].Block.transform.localScale = DestScale;

                        }
                        if (EraseVertical)
                        {
                            a_Block[EraseBlockX, EraseBlockY - 1].Block.transform.localScale = DestScale;
                            a_Block[EraseBlockX, EraseBlockY + 1].Block.transform.localScale = DestScale;
                        }
                        EraseHorizontal = false;
                        EraseVertical = false;
                        ProductFlag = false;
                        EraseFlag = false;
                        ProductNowFlag = false;
                    }
                }
            }
            else
            {
                int LastNum = a_Block[EraseBlockX, EraseBlockY].data;
                a_Block[EraseBlockX, EraseBlockY].data = ChangeNum;
                a_Block[EraseBlockX, EraseBlockY].Number = false;
                CheckNumber();
                a_Block[EraseBlockX, EraseBlockY].data = LastNum;
                a_Block[EraseBlockX, EraseBlockY].Number = true;
                EraseFlag = false;

            }

            

        }

        for (int n = 0; n < NumberLast.Length - 1; n++)
        {
            if (NumberLast[n] <= 0)
            {
                ButtonBlack(n);
            }
            else
            {
                ButtonWhite(n);
            }
        }

        if(UndoFlag)
        {
            UndoWhite();
        }
        else
        {
            UndoBlack();
        }
        //計算処理　追加　福岡　2016/10/11 ここまで
    }

    void CrearCheck()
    {
        bool bLast = false;
        int Count = 0;
		int AllLast = 0;
        for (int n = 0; n < NumberLast.Length; n++)
        {
            if(NumberLast[n] == 0 || (CharaNum == 0 && n == NumberLast.Length - 1))
            {
                Count++;
            }
            if(Count >= NumberLast.Length)
            {
                bLast = true;
            }

			AllLast += NumberLast [n];
        }



        

		if (bLast && (Score.ScoreCheck () >= Border) && !bEnd && !EraseFlag) {
			Debug.Log ("おわた");
			stageData.StageClear (CharaNum, StageNum, true);
			borderProduct.BorderOut ();
			//StartCoroutine (ProductWaitCoroutine (true));
			if (borderProduct.EndCheck ()) {
				borderProduct.TextErase ();
				GameObject.Find ("GameSystem").GetComponent<Menu> ().Result (true);
				bEnd = true;
			}
			//リザルト呼び出し
			// fade.gameObject.GetComponent<Fade>().NextSceneName = "title";
		} else if (bLast && (Score.ScoreCheck () < Border) && !bEnd && !EraseFlag) {
			Debug.Log ("無念");
			borderProduct.BorderOut ();
			//StartCoroutine (ProductWaitCoroutine (false));
			if (borderProduct.EndCheck ()) {
				borderProduct.TextErase ();
				GameObject.Find ("GameSystem").GetComponent<Menu> ().Result (false);
				bEnd = true;
			}
		} else if(!bEnd) {
			if (Score.ScoreCheck() >= Border) {
				Player.AnimChange (1);
				Enemy.AnimChange (2);
			}

			/*if (Score.ScoreCheck() < Border && AllLast <= 2) {
				Player.AnimChange (2);
				Enemy.AnimChange (1);
			}*/
		}
    }

    void EraseCheck()
    {
        float fWidth = (a_Block[1, 0].pos.x - a_Block[0, 0].pos.x) / 2.0f;
        float fHeight = (a_Block[0, 0].pos.y - a_Block[0, 1].pos.y) / 2.0f;
        int WidthMax = AreaWidth - 1;
        int HeightMax = AreaHeight - 1;
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        a_Block[0, 0].Block.GetComponent<BlockSelector>().Black();
        a_Block[WidthMax, 0].Block.GetComponent<BlockSelector>().Black();
        a_Block[0, HeightMax].Block.GetComponent<BlockSelector>().Black();
        a_Block[WidthMax, HeightMax].Block.GetComponent<BlockSelector>().Black();
        for (int n = 1; n < WidthMax; n++)
        {
            if ((a_Block[n - 1, 0].use && a_Block[n + 1, 0].use) && ChangeNum != 3)
            {
                a_Block[n, 0].Block.GetComponent<BlockSelector>().White();
            }
            else
            {
                a_Block[n, 0].Block.GetComponent<BlockSelector>().Black();
            }

            if ((a_Block[n - 1, HeightMax].use && a_Block[n + 1, HeightMax].use) && ChangeNum != 3)
            {
                a_Block[n, HeightMax].Block.GetComponent<BlockSelector>().White();
            }
            else
            {
                a_Block[n, HeightMax].Block.GetComponent<BlockSelector>().Black();
            }
        }
        for (int m = 1; m < HeightMax; m++)
        {
            if ((a_Block[0, m - 1].use && a_Block[0, m + 1].use) && ChangeNum != 3)
            {
                a_Block[0, m].Block.GetComponent<BlockSelector>().White();
            }
            else
            {
                a_Block[0, m].Block.GetComponent<BlockSelector>().Black();
            }

            if ((a_Block[WidthMax, m - 1].use && a_Block[WidthMax, m + 1].use) && ChangeNum != 3)
            {
                a_Block[WidthMax, m].Block.GetComponent<BlockSelector>().White();
            }
            else
            {
                a_Block[WidthMax, m].Block.GetComponent<BlockSelector>().Black();
            }
        }
        for (int n = 1; n < WidthMax; n++)
        {
            for (int m = 1; m < HeightMax; m++)
            {
                //以下の５つの式が全て成立し、座標が合っている部分のブロックを変更する
                if (a_Block[n, m].use &&
                     ((a_Block[n, m - 1].use && a_Block[n, m + 1].use) || (a_Block[n - 1, m].use && a_Block[n + 1, m].use))) //座標判定
                {
                    a_Block[n, m].Block.GetComponent<BlockSelector>().White();
                }
                else
                {
                    a_Block[n, m].Block.GetComponent<BlockSelector>().Black();
                }
            }
        }

        
    }
    //計算処理　追加　福岡　2016/10/11 ここから
    public void CheckNumber()
    {
        int NowScore = Score.ScoreCheck();
        bool bSkill = false;
        int BlockNumX = 0;
        int BlockNumY = 0;
        //Debug.Log("Calling");
        for (int n = 0; n < AreaWidth; n++)
        {
            for (int m = 0; m < AreaHeight; m++)
            {
                if (a_Block[n, m].use)
                {
                    if (n < AreaWidth - 2 && a_Block[n + 1, m].use && a_Block[n + 2, m].use && ((a_Block[n, m].Number == true) && (a_Block[n + 1, m].Number == false)) && ((a_Block[n, m].Number == true) && (a_Block[n + 2, m].Number == true)))
                    {
                        a_Block[n, m].Delete = true;
                        a_Block[n + 1, m].Delete = true;
                        a_Block[n + 2, m].Delete = true;
                        switch(a_Block[n + 1, m].data)
                        {
                            case 0:
                                Debug.Log(a_Block[n, m].data + "*" + a_Block[n + 2, m].data);
                                Score.ScorePlus(a_Block[n, m].data * a_Block[n + 2, m].data);
                                break;
                            case 1:
                                Debug.Log(a_Block[n, m].data + "+" + a_Block[n + 2, m].data);
                                Score.ScorePlus(a_Block[n, m].data + a_Block[n + 2, m].data);
                                break;
                            case 2:
                                Debug.Log(a_Block[n, m].data + "-" + a_Block[n + 2, m].data);
                                Score.ScorePlus(a_Block[n, m].data - a_Block[n + 2, m].data);
                                break;
                            default:
                                 a_Block[n, m].Delete = false;
                               // a_Block[n + 1, m].Delete = false;
                                a_Block[n + 2, m].Delete = false;
                                bSkill = true;
                                BlockNumX = n + 1;
                                BlockNumY = m;
                                break;
                        }
                    }
                    if (m < AreaHeight - 2 && a_Block[n, m + 1].use && a_Block[n, m + 2].use && ((a_Block[n, m].Number == true) && (a_Block[n, m + 1].Number == false)) && ((a_Block[n, m].Number == true) && (a_Block[n, m + 2].Number == true)))
                    {
                        a_Block[n, m].Delete = true;
                        a_Block[n, m + 1].Delete = true;
                        a_Block[n, m + 2].Delete = true;
                        switch (a_Block[n, m + 1].data)
                        {
                            case 0:
                                Debug.Log(a_Block[n, m].data + "*" + a_Block[n, m + 2].data);
                                Score.ScorePlus(a_Block[n, m].data * a_Block[n, m + 2].data);
                                break;
                            case 1:
                                Debug.Log(a_Block[n, m].data + "+" + a_Block[n, m + 2].data);
                                Score.ScorePlus(a_Block[n, m].data + a_Block[n, m + 2].data);
                                break;
                            case 2:
                                Debug.Log(a_Block[n, m].data + "-" + a_Block[n, m + 2].data);
                                Score.ScorePlus(a_Block[n, m].data - a_Block[n, m + 2].data);
                                break;
                            default:
                                 a_Block[n, m].Delete = false;
                                //a_Block[n, m + 1].Delete = false;
                                a_Block[n, m + 2].Delete = false;
                                bSkill = true;
                                BlockNumX = n;
                                BlockNumY = m + 1;
                                break;
                        }
                    }
                }
            }

            
        }
        if (bSkill)
        {
            switch (CharaNum)
            {
                case 0:
                    //ヒント呼び出し（おそらくここには来ない）
                    Debug.Log("来ちゃダメ！");
                    break;
                case 1:
                    Debug.Log("リヴァース");
                    Reverce(BlockNumX, BlockNumY);
                    break;
                case 2:
                    Debug.Log("爆発");
                    Exproad(BlockNumX, BlockNumY);
                    break;
                case 3:
                    Debug.Log("ツイスト");
                    BlockRotation(BlockNumX, BlockNumY);
                    break;
                default:
                    Debug.Log("来ちゃダメ！");
                    break;
            }
        }

        LastScore = Score.ScoreCheck() - NowScore;

        for (int n = 0; n < AreaWidth; n++)
        {
            for (int m = 0; m < AreaHeight; m++)
            {
                if (a_Block[n, m].Delete)
                {
                    deleteData(n, m);
                }
            }
        }

        FallBlock();

        
        ProductFlag = true;
    }
    //計算処理　追加　福岡　2016/10/11 ここまで

    //スキル（アインシュタイン）：爆発
    private void Exproad(int BlockX, int BlockY)
    {
        a_Block[BlockX, BlockY].Delete = true;
        a_Block[BlockX + 1, BlockY].Delete = true;
        a_Block[BlockX - 1, BlockY].Delete = true;
        a_Block[BlockX, BlockY + 1].Delete = true;
        a_Block[BlockX, BlockY - 1].Delete = true;
    }

    //スキル（アルキメデス）：入れ替え
    private void Reverce(int BlockX, int BlockY)
    {
        a_Block[BlockX, BlockY].Delete = false;

        BLOCKBOX box;
        box = a_Block[BlockX - 1, BlockY];
        a_Block[BlockX - 1, BlockY].data = a_Block[BlockX + 1, BlockY].data;
        a_Block[BlockX + 1, BlockY].data = box.data;
        deleteData(BlockX - 1, BlockY);
        deleteData(BlockX + 1, BlockY);
        addData(BlockX - 1, BlockY, a_Block[BlockX - 1, BlockY].Number, a_Block[BlockX - 1, BlockY].data);
        addData(BlockX + 1, BlockY, a_Block[BlockX + 1, BlockY].Number, a_Block[BlockX + 1, BlockY].data);
    }

    //スキル（ニュートン）：回転
    private void BlockRotation(int BlockX, int BlockY)
    {
        int[] Box = new int[8];
        bool[] UseBox = new bool[8];
        int BoxNum = 0;
        a_Block[BlockX, BlockY].Delete = false;

        Box[0] = a_Block[BlockX - 1, BlockY].data;
        UseBox[0] = a_Block[BlockX - 1, BlockY].use;
        Box[1] = a_Block[BlockX - 1, BlockY - 1].data;
        UseBox[1] = a_Block[BlockX - 1, BlockY - 1].use;
        Box[2] = a_Block[BlockX, BlockY - 1].data;
        UseBox[2] = a_Block[BlockX, BlockY - 1].use;
        Box[3] = a_Block[BlockX - 1, BlockY + 1].data;
        UseBox[3] = a_Block[BlockX - 1, BlockY + 1].use;
        Box[4] = a_Block[BlockX + 1, BlockY - 1].data;
        UseBox[4] = a_Block[BlockX + 1, BlockY - 1].use;
        Box[5] = a_Block[BlockX, BlockY + 1].data;
        UseBox[5] = a_Block[BlockX, BlockY + 1].use;
        Box[6] = a_Block[BlockX + 1, BlockY + 1].data;
        UseBox[6] = a_Block[BlockX + 1, BlockY + 1].use;
        Box[7] = a_Block[BlockX + 1, BlockY].data;
        UseBox[7] = a_Block[BlockX + 1, BlockY].use;
        for (int n = -1; n < 2; n++)
        {
            for (int m = -1; m < 2; m++)
            {
                if (!(n == 0 && m == 0))
                {
                    if (UseBox[BoxNum])
                    {
                        a_Block[BlockX + m, BlockY + n].data = Box[BoxNum];
                        deleteData(BlockX + m, BlockY + n);
                        addData(BlockX + m, BlockY + n, true, a_Block[BlockX + m, BlockY + n].data);
                    }
                    else
                    {
                        deleteData(BlockX + m, BlockY + n);
                    }
                    BoxNum++;
                }
            }
        }
        //NumberBlock
    }


    //ブロックを上に詰める
    public void FallBlock()
    {
        int nNum = 0;
        /*for (int n = 0; n < 6; n++)
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
                        deleteData(n, nNum);
                        addData(n, m, a_Block[n, nNum].Number, a_Block[n, nNum].data);
                        
                    }
                }
            }
        }*/
        for (int n = 0; n < AreaWidth; n++)
        {
            for (int m = AreaHeight - 1; m >= 0; m--)
            {
                nNum = m - 1;
                if (!a_Block[n, m].use)
                {
                    while (nNum >= 0 && !a_Block[n, nNum].use)
                        nNum--;
                    if (nNum >= 0)
                    {
                        deleteData(n, nNum);
                        addData(n, m, a_Block[n, nNum].Number, a_Block[n, nNum].data);

                    }
                }
            }
        }
    }

    //ブロックせり出し
    public void BlockAdvance()
    {
        for (int n = AreaWidth - 1; n >= 0; n--)
        {
            for (int m = AreaHeight - 1; m > 0; m--)
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

        for (int n = 0; n < AreaWidth; n++)
        {
            int nNum;
            deleteData(n,0);
            if (Random.Range(0, 101) >= 75)
            {
                nNum = Random.Range(0, 3);
                addData(n, 0, false, nNum);

            }
            else
            {
                nNum = Random.Range(0, 10);
                addData(n, 0, true, nNum);
            }
        }
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

        //データを初期化させる
        a_Block[arrayposX, arrayposY].use = false;
       // a_Block[arrayposX, arrayposY].Number = false;
       // a_Block[arrayposX, arrayposY].data = 0;

        //削除項目追加　hukuoka
        a_Block[arrayposX, arrayposY].Delete = false;
        a_Block[arrayposX, arrayposY].Block.SetActive(false);

        return true;
    }


    //その配列の一番下のY座標を返す
    public int GetArrayY(int X)
    {
        for(int i = 0; i < AreaHeight; i ++)
        {
            if( a_Block[X,i].use == false)
            {
                return i - 1;
            }
        }
        return -1;//エラー
    }




    void ChangeBlock()
    {
        float fWidth = (a_Block[1, 0].pos.x - a_Block[0, 0].pos.x) / 2.0f;
        float fHeight = (a_Block[0, 0].pos.y- a_Block[0, 1].pos.y) / 2.0f;
        int WidthMax = AreaWidth - 1;
        int HeightMax = AreaHeight - 1;
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int BlockX = -1;
        int BlockY = -1;
        for(int n = 1; n < WidthMax; n++)
        {
            if((a_Block[n - 1, 0].use && a_Block[n + 1, 0].use) && (a_Block[n, 0].pos.x + fWidth > MousePos.x && a_Block[n, 0].pos.x - fWidth < MousePos.x) && (a_Block[n, 0].pos.y + fHeight > MousePos.y && a_Block[n, 0].pos.y - fHeight < MousePos.y))
            {
                if (ChangeNum >= 0 && NumberLast[ChangeNum] > 0 && ChangeNum != 3)
                {
                    BlockX = n;
                    BlockY = 0;
                    EraseHorizontal = true;
                }

            }

            if ((a_Block[n - 1, HeightMax].use && a_Block[n + 1, HeightMax].use) && (a_Block[n, HeightMax].pos.x + fWidth > MousePos.x && a_Block[n, HeightMax].pos.x - fWidth < MousePos.x) && (a_Block[n, HeightMax].pos.y + fHeight > MousePos.y && a_Block[n, HeightMax].pos.y - fHeight < MousePos.y))
            {
                if (ChangeNum >= 0 && NumberLast[ChangeNum] > 0 && ChangeNum != 3)
                {
                    BlockX = n;
                    BlockY = HeightMax;
                    EraseHorizontal = true;
                }
            }
        }
        for (int m = 1; m < HeightMax; m++)
        {
            if((a_Block[0, m - 1].use && a_Block[0, m + 1].use) && (a_Block[0, m].pos.x + fWidth > MousePos.x && a_Block[0, m].pos.x - fWidth < MousePos.x) && (a_Block[0, m].pos.y + fHeight > MousePos.y && a_Block[0, m].pos.y - fHeight < MousePos.y))
            {
                if (ChangeNum >= 0 && NumberLast[ChangeNum] > 0 && ChangeNum != 3)
                {
                    BlockX = 0;
                    BlockY = m;
                    EraseVertical = true;
                }
            }

            if ((a_Block[WidthMax, m - 1].use && a_Block[WidthMax, m + 1].use) && (a_Block[WidthMax, m].pos.x + fWidth > MousePos.x && a_Block[WidthMax, m].pos.x - fWidth < MousePos.x) && (a_Block[WidthMax, m].pos.y + fHeight > MousePos.y && a_Block[WidthMax, m].pos.y - fHeight < MousePos.y))
            {
                if (ChangeNum >= 0 && NumberLast[ChangeNum] > 0 && ChangeNum != 3)
                {
                    BlockX = WidthMax;
                    BlockY = m;
                    EraseVertical = true;
                }
            }
        }
        for (int n = 1; n < WidthMax; n++)
        {
            for (int m = 1; m < HeightMax; m++)
            {
                //以下の５つの式が全て成立し、座標が合っている部分のブロックを変更する
                if (a_Block[n, m].use &&
                     ((a_Block[n, m - 1].use && a_Block[n, m + 1].use) || (a_Block[n - 1, m].use && a_Block[n + 1, m].use)) &&                 //上下、もしくは左右に数字があるか
                    ( a_Block[n, m].pos.x + fWidth > MousePos.x && a_Block[n, m].pos.x - fWidth < MousePos.x) && (a_Block[n, m].pos.y + fHeight > MousePos.y && a_Block[n, m].pos.y - fHeight < MousePos.y)) //座標判定
                {
                    
                    if(ChangeNum >= 0 && NumberLast[ChangeNum] > 0)
                    {
                        BlockX = n;
                        BlockY = m;
                        if (a_Block[n - 1, m].use && a_Block[n + 1, m].use)
                        {
                            EraseHorizontal = true;
                        }
                        if (a_Block[n, m - 1].use && a_Block[n, m + 1].use)
                        {
                            EraseVertical = true;
                        }
                    }
                }
            }
        }

        if(BlockX >= 0)
        {
            UndoRecord(ChangeNum);
            EraseFlag = true;
            EraseBlockX = BlockX;
            EraseBlockY = BlockY;
            NumberLast[ChangeNum]--;
            Debug.Log(NumberLast[ChangeNum]);
        }

    }

    public void  ChangeNumberSet(int nNum)
    {
        if(nNum >= 0)
        {
            if (NumberLast[nNum] > 0)
            {
                ChangeNum = nNum;
            }
        }
    }

    //関数追加20161028 terabayashi
    public int GetChangeNum()
    {
        return ChangeNum;
    }

    public int NumberLastCheck(int nNum)
    {
        return NumberLast[nNum];
    }

    private void UndoRecord(int UndoNum)
    {
        UndoFlag = true;
        LastSymbol = UndoNum;

        for (int n = 0; n < AreaWidth; n++)
        {
            for (int m = 0; m < AreaHeight; m++)
            {
                LastBlock[n, m] = a_Block[n, m];
            }
        }
    }

    public void Undo()
    {
        bool[,] bFlag = new bool[6, 6];
        if(UndoFlag)
        {
            for (int n = 0; n < AreaWidth; n++)
            {
                for (int m = 0; m < AreaHeight; m++)
                {
                    deleteData(n, m);
                    a_Block[n, m] = LastBlock[n, m];
                    if (a_Block[n, m].use)
                    {
                        a_Block[n, m].use = false;
                    }
                    else
                    {
                        bFlag[n, m] = true;
                    }
                    addData(n, m, a_Block[n, m].Number, a_Block[n, m].data);

                    if(bFlag[n, m])
                    {
                        deleteData(n, m);
                    }
                }
            }

            NumberLast[LastSymbol]++;

            Score.ScoreUndo(LastScore);
            UndoFlag = false;
        }
    }

    public void ButtonBlack(int Num)
    {
        //if (CharaNum != 3)
        {
            GameObject.Find(ButtonName[Num]).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        }
    }
    public void ButtonWhite(int Num)
    {
        //if (ChangeNum != 3)
        {
            GameObject.Find(ButtonName[Num]).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
        }
    }

    public void UndoBlack()
    {
            GameObject.Find("undo").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
    }
    public void UndoWhite()
    {
            GameObject.Find("undo").GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
    }

	IEnumerator ProductWaitCoroutine (bool bFlag)
	{
		yield return new WaitForSeconds(3.5f);

		GameObject.Find ("GameSystem").GetComponent<Menu> ().Result (bFlag);
		bEnd = true;
	}
}


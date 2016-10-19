//プレイヤーのコントロールスクリプト

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    
   // public float Speed;
    GameObject arrayBlcokBox;//ブロック配列

    public GameObject BlockPrefab;//ブロックをひっぱる処理に使用。ブロックのプレファブを指定する
    GameObject block;//引っ張るときに表示させるオブジェクト

    public float CatchThrowAnimationTime;//ひっぱりアニメーション時間
    
    int PlayerX;//プレイヤーの現在位置（X）
    private BlockBox.BLOCKBOX BlockData;//ブロックデータ構造体
    bool bPlayerCatch;//プレイヤーがキャッチしているか投げているかの判定フラグ
    bool animStart;//アニメーションフラグ
    float StartTime;//補完処理に必要



    Vector3 bpos;//アニメーションの開始・終了予定座標


    void Start()
    {
        PlayerX = 3;//プレイヤー初期位置
        bPlayerCatch = false;//キャッチフラグOFF
        animStart = false;//アニメーション未開始
        StartTime = 0.0f;//タイム初期化
 
        bpos = Vector3.zero;//ベクトル初期化
        block = null;
    }

    void Update()
    {
        //ブロック配列のあるオブジェクトを取得
        arrayBlcokBox = GameObject.Find("BlockBoxArray");
        
        //
        BlockBox hoge = arrayBlcokBox.GetComponent<BlockBox>();

        //PC実行時のみキーボード入力での移動を受け付ける
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveLeft();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                MoveRight();
            }

        }


        //データが入っている
        if (animStart)
        {
            //キャッチ（下にブロックが移動）
            if (bPlayerCatch)
            {
                if(block != null)
                {
                    //線形補間

                    float diff = Time.timeSinceLevelLoad - StartTime;
                    if (diff > CatchThrowAnimationTime)
                    {
                        //補完完了してる場合は最終座標を代入
                        block.transform.position = arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, 8).pos;
                    }
                    float rate = diff / CatchThrowAnimationTime;

                    block.transform.position = Vector3.Lerp(bpos, arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, 8).pos, rate);

                }
             
            }

            //リリース（上へ移動）
            if (!bPlayerCatch)
            {
                if (block != null)
                {
                    //線形補間
                    float diff = Time.timeSinceLevelLoad - StartTime;
                    float rate = diff / CatchThrowAnimationTime;

                    block.transform.position = Vector3.Lerp(arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, 8).pos, bpos, rate);


                    if (diff > CatchThrowAnimationTime)
                    {
                        //補完完了している場合

                        block.transform.position = bpos;
                        //演出用プレファブをデストロイ
                        Destroy(block);
                        //ぬる
                        block = null;
                        //アニメーションフラグ無効
                        animStart = false;
                        //ここでやっとデータ挿入
                        NumberThrow();

                    }
                  

                }
            }


        }


  

        //プレイヤーの移動処理
        transform.position = new Vector3(hoge.GetData(PlayerX, 8).pos.x, hoge.GetData(PlayerX, 8).pos.y + (this.GetComponent<SpriteRenderer>().bounds.size.y * 0.2f), 0);
      
    }



    public void MoveRight()
    {
        if (PlayerX < 5)
        {
            PlayerX++;
        }
    }

    public void MoveLeft()
    {
        if (PlayerX > 0)
        {
            PlayerX--;
        }
    }

    //ブロックを取得する関数
     void NumberCatch()
    {
        if (!BlockData.use)
        {
        
            //ブロックの情報を取得
            BlockData = arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, arrayBlcokBox.GetComponent<BlockBox>().GetArrayY(PlayerX));
            if (BlockData.use)
            {
                //そのブロっくにデータがある場合

                //実データを破棄（プレイヤー側でデータ自体は保持）
                arrayBlcokBox.GetComponent<BlockBox>().deleteData(PlayerX, arrayBlcokBox.GetComponent<BlockBox>().GetArrayY(PlayerX));
                //アニメーション
                GetComponent<Character_Box>().AnimChange(1);
            }
        }
    }

    //ブロックを投げる関数
     void NumberThrow()
    {

        if (BlockData.use)
        {
            //ブロック情報がある

            //到達予定場所にデータがない場合
            if (!arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, arrayBlcokBox.GetComponent<BlockBox>().GetArrayY(PlayerX) + 1).use)
            {
                //データを挿入
                arrayBlcokBox.GetComponent<BlockBox>().addData(PlayerX, arrayBlcokBox.GetComponent<BlockBox>().GetArrayY(PlayerX) + 1, BlockData.Number, BlockData.data);
                //プレイヤー側でのデータを破棄
                BlockData.use = false;
                //アニメーション
                GetComponent<Character_Box>().AnimChange(0);
            }

        }
    }


    //キャッチしてる状態だと投げる・キャッチしてない状態だと受け取る
    public void CatchAndRelease()
    {

        if( bPlayerCatch)
        {
            //投げる場合

            //最終的な目的座標を入手
            bpos = arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, arrayBlcokBox.GetComponent<BlockBox>().GetArrayY(PlayerX)+1).pos;

            //線形補間に使用するため時間情報を入れる
            StartTime = Time.time;
           
            //プレイヤーのキャッチフラグをFalse
            bPlayerCatch = false;
        }
        else
        {
            //キャッチする場合
          
            //消す予定のブロックの位置を取得
            bpos = arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, arrayBlcokBox.GetComponent<BlockBox>().GetArrayY(PlayerX)).pos;
            //番号か記号かどうかのフラグを取得
            bool numflg = arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, arrayBlcokBox.GetComponent<BlockBox>().GetArrayY(PlayerX)).Number;
            //データの数を取得
            int datanum = arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, arrayBlcokBox.GetComponent<BlockBox>().GetArrayY(PlayerX)).data;

            //ブロックのプレファブを生成

            block = Instantiate(BlockPrefab);
            //位置情報代入
            block.transform.position = bpos;
            //ブロックの見た目を反映
            block.GetComponent<BlockSelector>().NumberChanger(numflg, datanum);

            //線形補間に使用するため現在時刻を入手
            StartTime = Time.time;
            //データ情報入手
            NumberCatch();
            //キャッチフラグをTrue
            bPlayerCatch = true;

           

        }
        animStart = true;
    }
  

}
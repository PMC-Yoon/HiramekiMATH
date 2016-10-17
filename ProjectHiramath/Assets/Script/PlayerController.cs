using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float Speed;
    GameObject arrayBlcokBox;

    public GameObject BlockPrefab;

    int PlayerX;
    private BlockBox.BLOCKBOX BlockData;
    bool bPlayerCatch;
    bool animStart;
    float StartTime;



    Vector3 bpos;


    void Start()
    {
        PlayerX = 3;
        bPlayerCatch = false;
        animStart = false;
        StartTime = 0.0f;
        //   transform.position = new Vector3( transform.position.z);

    }

    void Update()
    {
        arrayBlcokBox = GameObject.Find("BlockBoxArray");
        BlockBox hoge = arrayBlcokBox.GetComponent<BlockBox>();

        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }




        //データが入っている
        if (animStart)
        {
            
            if (bPlayerCatch)
            {

             
            }

            //リリース（上へ移動？）
            if (!bPlayerCatch)
            {
       
            }


        }


  

        //位置情報をあげる
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
        
            
            BlockData = arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, arrayBlcokBox.GetComponent<BlockBox>().GetArrayY(PlayerX));
            if (BlockData.use)
            {
                arrayBlcokBox.GetComponent<BlockBox>().deleteData(PlayerX, arrayBlcokBox.GetComponent<BlockBox>().GetArrayY(PlayerX));
                GetComponent<Character_Box>().AnimChange(1);
            }
        }
    }

    //ブロックを投げる関数
     void NumberThrow()
    {
        if (BlockData.use)
        {
            if (!arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, arrayBlcokBox.GetComponent<BlockBox>().GetArrayY(PlayerX) + 1).use)
            {
                arrayBlcokBox.GetComponent<BlockBox>().addData(PlayerX, arrayBlcokBox.GetComponent<BlockBox>().GetArrayY(PlayerX) + 1, BlockData.Number, BlockData.data);
                BlockData.use = false;
                GetComponent<Character_Box>().AnimChange(0);
            }

        }
    }

    public void CatchAndRelease()
    {
        if( bPlayerCatch)
        {
          
            NumberThrow();
            bPlayerCatch = false;
        }
        else
        {
            
            //bpos = arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, 8).pos;
            //bool numflg  = arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, arrayBlcokBox.GetComponent<BlockBox>().GetArrayY(PlayerX)).Number;
            //int datanum =  arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, arrayBlcokBox.GetComponent<BlockBox>().GetArrayY(PlayerX)).data;

            //BlockData.Block = Instantiate(BlockPrefab);

            //BlockData.Block.transform.SetParent(this.transform);

            //BlockData.Block.transform.position = bpos;
            //BlockData.Block.GetComponent<BlockSelector>().NumberChanger(numflg, datanum);
            
    

     //       BlockData.Block.transform.position = new Vector3(0,-4,0);
    //      StartTime = Time.timeSinceLevelLoad;
          //  BlockData.Block.transform.Translate(0, 1, 0);

            // arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, arrayBlcokBox.GetComponent<BlockBox>().GetArrayY(PlayerX)).Number;
            //addData(n, m, true, nNum);
            //キャッチ（下へ移動？）







            NumberCatch();
            bPlayerCatch = true;

           

        }
        animStart = true;
    }
  

}
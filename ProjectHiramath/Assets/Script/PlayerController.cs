using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float Speed;
    GameObject arrayBlcokBox;
    int PlayerX;
    private BlockBox.BLOCKBOX BlockData;
    bool bPlayerCatch;

    void Start()
    {
        PlayerX = 3;
        bPlayerCatch = false;


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

            }
            GetComponent<Character_Box>().AnimChange(1); //アニメーション再生　持ち上げなう　
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
            }
            GetComponent<Character_Box>().AnimChange(0); //アニメーション再生　平常運転
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
            NumberCatch();
            bPlayerCatch = true;

        }

        
    }
  

}
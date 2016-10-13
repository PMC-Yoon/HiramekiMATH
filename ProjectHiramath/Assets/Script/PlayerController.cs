using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float Speed;
    GameObject arrayBlcokBox;
    int PlayerX;
    private BlockBox.BLOCKBOX BlockData;
    
    void Start()
    {
        PlayerX = 3;
      


     //   transform.position = new Vector3( transform.position.z);

    }
    
    void Update()
    {
        arrayBlcokBox = GameObject.Find("BlockBoxArray");
        BlockBox hoge = arrayBlcokBox.GetComponent<BlockBox>();

        if( Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }
        if( Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }

        transform.position = new Vector3(hoge.GetData(PlayerX, 8).pos.x, hoge.GetData(PlayerX, 8).pos.y +(this.GetComponent<SpriteRenderer>().bounds.size.y*0.2f) , 0);

        
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
    public void NumberCatch()
    {
        if (!BlockData.use)
        {
            BlockData = arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, 3);
            if (BlockData.use)
            {
                arrayBlcokBox.GetComponent<BlockBox>().deleteData(PlayerX, 3);

            }
        }
    }

    //ブロックを投げる関数
    public void NumberThrow()
    {
       if(BlockData.use)
        {
            if (!arrayBlcokBox.GetComponent<BlockBox>().GetData(PlayerX, 3).use)
            {
                arrayBlcokBox.GetComponent<BlockBox>().addData(PlayerX, 3, BlockData.Number, BlockData.data);
                BlockData.use = false;
            }
            
        }
    }

}

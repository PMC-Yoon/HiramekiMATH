using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float Speed;
    GameObject arrayBlcokBox;
    int PlayerX;
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
    
    
    /*
    public float Speed;

    const int LaneMin = -3;
    const int LaneMax = 2;

    public float LaneWidth;
    Vector3 move;
   public int PlayerLane;

    CharacterController controller;

    int laneData;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update() {

        //move.x = Input.GetAxis("Horizontal") * Speed;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown("left"))
        {
            MoveLeft();
        }
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown("right"))
        {
            MoveRight();
        }

        float x = (PlayerLane * LaneWidth - transform.position.x) / LaneWidth;

        move.x = x * Speed;//* Time.deltaTime;

       // Vector3 globalDirection = transform.TransformDirection(move);//移動ベクトルをグローバルなベクトルに変換

        controller.Move(move * Time.deltaTime);


        // transform.Translate(new Vector3(move.x , 0, 0));

    }


   public void MoveLeft()
    {
        if (PlayerLane > LaneMin)
        {
            PlayerLane--;
        }
    }
  public  void MoveRight()
    {
        if (PlayerLane < LaneMax)
        {
            PlayerLane++;
        }
    }
    */


}

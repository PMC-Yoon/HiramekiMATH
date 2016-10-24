using UnityEngine;
using System.Collections;

public class StageSelectSystem : MonoBehaviour {

    public static int SelectStage;
    public GameObject ConfirmMenuPrefab;

    GameObject[] Stages;
    void Start()
    {
        SelectStage = 0;
 //       GameObject.FindGameObjectsWithTag("Stage");
        
    }

    



    //ステージ番号を設定する関数
    public void SetStageNum(int StageNum)
    {
        SelectStage = StageNum;
    }

    //確認メニューの表示
    public void ConfirmMenuSet()
    {
        ConfirmMenuPrefab.SetActive(true);
    }
    //確認メニューの非表示
    public void ConfirmMenuRelease()
    {
        ConfirmMenuPrefab.SetActive(false);
    }
	
}

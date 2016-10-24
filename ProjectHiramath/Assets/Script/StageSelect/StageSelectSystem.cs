using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StageSelectSystem : MonoBehaviour {

    public static int SelectStage;
    public GameObject ConfirmMenuPrefab;
    GameObject stagedata;
    public GameObject[] stagelist;
    void Start()
    {
        SelectStage = 0;
        int selectChara;
        selectChara = CharacterSelectSystem.SelectCharacter;
        stagedata = GameObject.Find("StageData");

        stagedata.GetComponent<StageData>().StageClear(selectChara, 0, true);
      
        for (int i = 0; i < 4; i++)
        {
            bool draw = stagedata.GetComponent<StageData>().ClearCheck(selectChara, i);
            stagelist[i].SetActive(draw);
        }
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

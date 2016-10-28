using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {
    public GameObject PauseMenuPrefab;
    public GameObject ResultPrefab; //追加しました　by福岡
    GameObject fade;
	// Use this for initialization
	void Start () {
        fade = GameObject.Find("Fade");
	}
	
	// Update is called once per frame
	void Update () {
	
	}



    //ポーズを有効にする関数
    void PauseSet()
    {
        Time.timeScale = 0.0f;
    }

    //ポーズを無効化する関数
    void PauseRelease()
    {
        Time.timeScale = 1.0f;
    }

    //ポーズ中なら解除、ポーズじゃない場合ポーズをセット
    public void PauseAndRelease()
    {
        if( (int)Time.timeScale == 0)
        {
            PauseRelease();
            PauseMenuPrefab.SetActive(false);
            return;
        }
        if( (int)Time.timeScale == 1)
        {
            PauseSet();
            PauseMenuPrefab.SetActive(true);
            return;
        }
    }

    public void PauseMenuRestart()
    {
        PauseRelease();
        fade.gameObject.GetComponent<Fade>().NextSceneName = "game";
        fade.gameObject.GetComponent<Fade>().FadeStart();

    }

    public void PauseMenuStageSelect()
    {
        PauseRelease();
        fade.gameObject.GetComponent<Fade>().NextSceneName = "StageSelect";
        fade.gameObject.GetComponent<Fade>().FadeStart();
    }

    public void PauseMenuCharacterSelect()
    {
        PauseRelease();
        fade.gameObject.GetComponent<Fade>().NextSceneName = "CharacterSelect";
        fade.gameObject.GetComponent<Fade>().FadeStart();
    }


    //リザルト出す関数 追加しました　by福岡
    public void Result(bool Clear)
    {
        GameObject PlBox = GameObject.Find("CharacterBox");
        GameObject EnBox = GameObject.Find("CharacterBox_2");
        float ResultScale = 200.0f;

        PlBox.transform.SetParent(GameObject.Find("Canvas").transform);
        PlBox.transform.localPosition = new Vector3(-330.0f,0);
        PlBox.transform.localScale = new Vector3(ResultScale, ResultScale, ResultScale);
        PlBox.transform.SetSiblingIndex(ResultPrefab.transform.GetSiblingIndex() + 1);
        PlBox.GetComponent<Character_Box>().PauseAnime();
        PlBox.GetComponent<Character_Box>().ChangeMode();

        EnBox.transform.SetParent(GameObject.Find("Canvas").transform);
        EnBox.transform.localPosition = new Vector3(330.0f, 0);
        EnBox.transform.localScale = new Vector3(ResultScale, ResultScale, ResultScale);
        EnBox.transform.SetSiblingIndex(ResultPrefab.transform.GetSiblingIndex() + 1);
        EnBox.GetComponent<Character_Box>().PauseAnime();
        EnBox.GetComponent<Character_Box>().ChangeMode();

        if (Clear)
        {
            ResultPrefab.transform.GetChild(4).gameObject.SetActive(false);
            PlBox.GetComponent<Character_Box>().AnimChange(3);
            EnBox.GetComponent<Character_Box>().AnimChange(4);
        }
        else
        {
            ResultPrefab.transform.GetChild(3).gameObject.SetActive(false);
            PlBox.GetComponent<Character_Box>().AnimChange(4);
            EnBox.GetComponent<Character_Box>().AnimChange(3);
        }



        /*if ((int)Time.timeScale == 0)
        {
            PauseRelease();
            ResultPrefab.SetActive(false);
            return;
        }
        if ((int)Time.timeScale == 1)
        {
            PauseSet();
           
            return;
        }*/
        ResultPrefab.SetActive(true);
    }

}

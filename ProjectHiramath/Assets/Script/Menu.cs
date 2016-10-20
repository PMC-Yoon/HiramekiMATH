using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {
    public GameObject PauseMenuPrefab;

	// Use this for initialization
	void Start () {
	
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



}

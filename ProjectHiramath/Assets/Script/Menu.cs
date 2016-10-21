﻿using UnityEngine;
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

    public void PauseMenuTitleBack()
    {
        PauseRelease();
        fade.gameObject.GetComponent<Fade>().NextSceneName = "title";
        fade.gameObject.GetComponent<Fade>().FadeStart();
    }


    //リザルト出す関数 追加しました　by福岡
    public void Result()
    {
        if ((int)Time.timeScale == 0)
        {
            PauseRelease();
            ResultPrefab.SetActive(false);
            return;
        }
        if ((int)Time.timeScale == 1)
        {
            PauseSet();
            ResultPrefab.SetActive(true);
            return;
        }
    }

}

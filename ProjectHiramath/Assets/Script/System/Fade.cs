using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




public class Fade : MonoBehaviour
{

    //  public GameObject scenemanager;
    public string NextSceneName;
    GameObject FadeObject;
    float startTime;
    public float FadeTime;
    Color alpha;
    string fadeStart;



    // Use this for initialization
    void Start()
    {
        //Fadeオブジェクトの取得

        startTime = Time.time;
        fadeStart = "FadeIn";

    }

    // Update is called once per frame
    void Update()
    {

        switch (fadeStart)
        {
            case "FadeIn":
                alpha.a = 1.0f - (Time.time - startTime) / FadeTime;
                this.GetComponent<Image>().color = new Color(0, 0, 0, alpha.a);
                break;


            case "FadeOut":
                alpha.a = (Time.time - startTime) / FadeTime;
                this.GetComponent<Image>().color = new Color(0, 0, 0, alpha.a);

                break;
        }





    }

    void Load()
    {
        SceneManager.LoadScene(NextSceneName);
    }

    public void FadeStart()
    {
        
        fadeStart = "FadeOut";
        startTime = Time.time;
        Invoke("Load", FadeTime);
    }


}

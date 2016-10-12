using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {

    public  string SceneName;

    void Start ()
    {
      
    }

   public  void SceneLoad()
    {
        SceneManager.LoadScene(SceneName);
    }
}

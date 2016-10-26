using UnityEngine;
using System.Collections;

public class ClickCheck : MonoBehaviour {
    private Vector3 MousePos;
    private ParticleSystem particle;
    // Use this for initialization
    void Start () {
        //particle = transform.GetChild(0).GetComponent<ParticleSystem>();
        particle = GetComponent<ParticleSystem>();
        DontDestroyOnLoad(this);
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0))
        {
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MousePos.z = -8;
            particle.transform.position = MousePos;
            particle.Play();
        }
	}
}

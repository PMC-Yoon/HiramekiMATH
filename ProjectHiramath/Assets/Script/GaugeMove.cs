using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GaugeMove : MonoBehaviour {
    private Image image;
    private float fGauge;
    private float fNum;
	// Use this for initialization
	void Start () {
        fGauge = 0f;
        fNum = 0.01f;
        image = this.gameObject.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        fGauge += fNum;
        image.fillAmount = fGauge;
        if (fGauge >= 1.0f || fGauge < 0f)
            fNum *= -1f;
    }
}

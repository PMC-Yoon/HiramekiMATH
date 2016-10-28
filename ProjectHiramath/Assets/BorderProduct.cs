using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BorderProduct : MonoBehaviour {
    private Text Title;
    private Text ProText;
    private float Alpha;
    private bool[] bProduct;
	// Use this for initialization
	void Start () {
        Title = transform.GetChild(0).gameObject.GetComponent<Text>();
        ProText = transform.GetChild(1).gameObject.GetComponent<Text>();
        bProduct = new bool[2];
        Alpha = 1.0f;
        
    }
	
	// Update is called once per frame
	void Update () {
        ProText.text = string.Format("{0,2}", GameObject.Find("Score").GetComponent<ScoreSystem>().GetBorderValue());
        if (bProduct[0])
        {
            Title.color = new Color(Title.color.r, Title.color.g, Title.color.b, Alpha);
            ProText.color = new Color(Title.color.r, Title.color.g, Title.color.b, Alpha);
            Alpha -= 0.3f * Time.deltaTime;
            if(Alpha < 0.0f)
            {
               // bProduct[0] = false;
                Alpha = 0.0f;
            }
        }

        if(bProduct[1])
        {
            Title.color = new Color(Title.color.r, Title.color.g, Title.color.b, Alpha);
            ProText.color = new Color(Title.color.r, Title.color.g, Title.color.b, Alpha);
            Alpha += 0.5f * Time.deltaTime;
            if (Alpha > 1.0f)
            {
               // bProduct[0] = false;
               // bProduct[1] = true;
                Alpha = 1.0f;
            }
        }
	}

    public void BorderIn()
    {
        bProduct[0] = true;
    }

    public void BorderOut()
    {
        bProduct[1] = true;
    }
}

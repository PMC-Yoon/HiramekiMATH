using UnityEngine;
using System.Collections;

public class Character_Box : MonoBehaviour
{
    public GameObject[] Chara_Box;
    private GameObject Chara;
    private int CharacterNum;
    private float ScaleSize;
    // Use this for initialization
    void Start()
    {
        CharacterNum = 0;
        ScaleSize = 0.5f;
        Chara = Instantiate(Chara_Box[CharacterNum]);
        Chara.transform.position = this.gameObject.transform.position;
        Chara.transform.localScale = new Vector3(ScaleSize, ScaleSize, ScaleSize);
        Chara.transform.SetParent(this.gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CharacterChange(int nNum)
    {
        CharacterNum = nNum;
        Destroy(Chara);
        Chara = Instantiate(Chara_Box[CharacterNum]);
        Chara.transform.position = this.gameObject.transform.position;
        Chara.transform.localScale = new Vector3(ScaleSize, ScaleSize, ScaleSize);
        Chara.transform.SetParent(this.gameObject.transform);
    }

    public void AnimChange(int nNum)
    {
        Chara.GetComponent<SpriteAnimator>().ChangeAnim(nNum);

    }
}

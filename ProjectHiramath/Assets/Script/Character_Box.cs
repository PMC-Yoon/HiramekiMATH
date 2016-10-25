using UnityEngine;
using System.Collections;

public class Character_Box : MonoBehaviour
{
    public GameObject[] Chara_Box;
    private GameObject Chara;
    private int CharacterNum;
    public float ScaleSize;
    // Use this for initialization
    void Start()
    {
        CharacterNum = 0;
        Chara = Instantiate(Chara_Box[CharacterNum]);
        Chara.transform.position = this.gameObject.transform.position;
        Chara.transform.localScale = new Vector3(ScaleSize, ScaleSize, ScaleSize);
        Chara.transform.SetParent(this.gameObject.transform);
        Chara.transform.localEulerAngles = (Vector3.zero);
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
        Chara.transform.localEulerAngles = (Vector3.zero);
    }

    public void AnimChange(int nNum)
    {
        Chara.GetComponent<SpriteAnimator>().ChangeAnim(nNum);

    }

    public void PauseAnime()
    {
        Chara.GetComponent<SpriteAnimator>().PauseAnim();
    }

    public void NormalAnime()
    {
        Chara.GetComponent<SpriteAnimator>().NormalAnim();
    }

    public void ChangeMode()
    {
        Chara.GetComponent<SpriteAnimator>().ChangeMode();
    }
}

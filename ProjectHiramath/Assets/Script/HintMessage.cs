using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HintMessage : MonoBehaviour {

    
	// Use this for initialization
	void OnEnable() {

        Text text = this.GetComponent<Text>();

        if( CharacterSelectSystem.SelectCharacter ==0 )//Galileoちゃん
        {
            //ステージで処理を分ける
            switch(StageSelectSystem.SelectStage)
            {
                
                case 0:
                    {
                        //ステージ１の場合
                        text.text = "＋に変えたブロックの左右を足したのがスコアに足されるぞ！";
                        break;
                    }
                case 1:
                    {
                        //ステージ２
                        text.text = "ブロックの落下をうまくつかうんだぞ。ここはすこし０がじゃまだな？";
                        break;
                    }
                case 2:
                    {
                        //ステージ３
                        text.text = "×は０にぶつけないように注意だ！\nいくら大きい数字でも０になってしまうからな！";
                        break;
                    }
                case 3:
                    {
                        //ステージ４
                        text.text = "大きい数字からできるだけ\n小さい数字を引くことでマイナス\nでもスコアがゲットできるぞ！";
                        break;
                    }
                default:
                    {
                        text.text = "貴様見ているな？";
                        break;
                    }
            }
        }
        else
        {
            //Galileo以外は何も表示しない
            text.text = "";

        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

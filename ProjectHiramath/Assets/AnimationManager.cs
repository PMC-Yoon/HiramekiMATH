using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {
	private GameObject Player;
	private GameObject Enemy;
	// Use this for initialization
	void Start () {
		Player = GameObject.Find ("CharacterBox");
		Enemy = GameObject.Find ("CharacterBox_2");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayerCharaChange(int nNum)
	{
//		Player.GetComponent<Character_Box>().CharacterChange(nNum);
	}

	public void EnemyCharaChange(int nNum)
	{
	//	Enemy.GetComponent<Character_Box> ().CharacterChange (nNum);
	}

	public void PlayerAnimChange(int nNum)
	{
	//	Player.GetComponent<Character_Box> ().AnimChange (nNum);
	}

	public void EnemyAnimChange(int nNum)
	{
//Enemy.GetComponent<Character_Box> ().AnimChange (nNum);
	}
}

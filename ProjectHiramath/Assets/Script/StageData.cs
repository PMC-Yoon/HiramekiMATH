using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class StageData : MonoBehaviour {
    public struct STAGEBOX
    {
        public int[,] FieldData; //ステージデータ格納
        public int Border;
        public int[] NumberLast;
    }

    private int StageMax;
    private int CharacterMax;
    private STAGEBOX[,] StageBox;
    private Vector2 FieldSize;
	// Use this for initialization
	void Start () {
        StageMax = 16;
        CharacterMax = 4;
        FieldSize = new Vector2(6, 6);
        StageBox = new STAGEBOX[CharacterMax, StageMax / CharacterMax];
        //StageBox = new int[CharacterMax, StageMax / CharacterMax];
       // NumberLastBox = new int[CharacterMax, StageMax / CharacterMax, 4];
        DontDestroyOnLoad(this);

        string FileName;
        int Stage;
        int Chara;
        TextAsset CSV;
        List<string[]> StageStatus = new List<string[]>();
        FileName = "stage_";
        Chara = 0;
        Stage = 1;
        StringReader reader;

        for (int Loop = 0; Loop < CharacterMax; Loop++, Chara++)
        {
            for (int Loop2 = 0; Loop2 < StageMax / CharacterMax; Loop2++, Stage++)
            {
                CSV = Resources.Load(FileName + Chara + "_" + Stage) as TextAsset;
                reader = new StringReader(CSV.text);

                StageBox[Loop, Loop2].FieldData = new int[(int)(FieldSize.x), (int)(FieldSize.y)];
                StageBox[Loop, Loop2].NumberLast = new int[4];

                while (reader.Peek() > -1)
                {
                    string line = reader.ReadLine();
                    StageStatus.Add(line.Split(','));
                }

                // ステージの中に読み込み
                for (int n = 0; n < (int)(FieldSize.x); n++)
                {
                    for (int m = 0; m < (int)(FieldSize.y); m++)
                    {
                        //StageBox[Loop,Loop2].FieldData[n, m].Number = true;
                        StageBox[Loop, Loop2].FieldData[n,m] = int.Parse(StageStatus[m][n]);
                    }
                }

                StageBox[Loop, Loop2].Border = int.Parse(StageStatus[(int)(FieldSize.y)][0]);

                StageBox[Loop, Loop2].NumberLast[0] = int.Parse(StageStatus[(int)(FieldSize.y)][3]);
                StageBox[Loop, Loop2].NumberLast[1] = int.Parse(StageStatus[(int)(FieldSize.y)][1]);
                StageBox[Loop, Loop2].NumberLast[2] = int.Parse(StageStatus[(int)(FieldSize.y)][2]);
                StageBox[Loop, Loop2].NumberLast[3] = int.Parse(StageStatus[(int)(FieldSize.y)][4]);
            }
            Stage = 1;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StageLoad(int CharaNum, int StageNum, BlockBox.BLOCKBOX[,] Box,ref int border, int[] numberlast)
    {
        for (int n = 0; n < (int)(FieldSize.x); n++)
        {
            for (int m = 0; m < (int)(FieldSize.y); m++)
            {
                Box[n, m].data = StageBox[CharaNum, StageNum].FieldData[n, m];
            }
        }

        border = StageBox[CharaNum, StageNum].Border;

        Debug.Log(StageBox[CharaNum, StageNum].Border);

        for (int n = 0; n < StageBox[CharaNum, StageNum].NumberLast.Length; n++)
        {
            numberlast[n] = StageBox[CharaNum, StageNum].NumberLast[n];
        }
    }
}

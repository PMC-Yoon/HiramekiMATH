using UnityEngine;
using System.Collections;
using Novel;
using UnityEngine.UI;

public class SceneInit : MonoBehaviour
{

    private GameManager gameManager;
    private GameView gameView;

    public float messageSpeed = 0.02f;

    private string currentMessage = ""; //現在表示中の文字列
    public string messageForSaveTitle = ""; //セーブのタイトル用に保持する文字列.

    public SceneInit()
    {

    }

    // Use this for initialization
    void Start()
    {

        //すべてクリアする。
        NovelSingleton.clearSingleton();
        StatusManager.initScene();

        Debug.Log("GameStart");

        //ドキュメント作製用
        //DocGenerator.start ();
        //return;

        //NovelSingleton.clearSingleton ();

        this.gameManager = NovelSingleton.GameManager;
        this.gameView = NovelSingleton.GameView;

        this.gameManager.setScene(this);

        //シナリオの読み込み
        this.gameManager.loadConfig();

        //font タグを使って指定だな。
        //メッセージフォントカラー設定
        Color c = ColorX.HexToRGB(this.gameManager.getConfig("messageFontColor"));
        c.a = 0f;
        this.gameView.messageArea.GetComponent<Text>().color = c;
        this.gameView.messageArea.GetComponent<Text>().fontSize = int.Parse(this.gameManager.getConfig("messageFontSize"));


        //グローバルコンフィグ読み込み
        this.gameManager.saveManager.loadGlobal();

        if (StatusManager.nextLoad != "")
        {

            string next_load = StatusManager.nextLoad;

            //ロードの場合、画面を再現する必要がある。
            StatusManager.nextLoad = "";

            this.gameManager.loadData(next_load);

        }
        else if (StatusManager.nextFileName != "")
        {

            //scene new でジャンプしてきた後。variable は引き継がないとだめ。
            string file = StatusManager.nextFileName;
            string target = StatusManager.nextTargetName;

            StatusManager.nextFileName = "";
            StatusManager.nextTargetName = "";

            //この２つを元にその位置へジャンプした上で実行
            string tag_str = "[jump file='" + file + "' target='" + target + "' ]";

            //タグを実行
            AbstractComponent cmp = this.gameManager.parser.makeTag(tag_str);
            cmp.start();

        }
        else
        {
            //初回起動時
            this.messageSpeed = float.Parse(this.gameManager.getConfig("messageSpeed"));

            StatusManager.variable.replaceAll("global", this.gameManager.globalSetting.globalVar);

            string scenario_file = this.gameManager.getConfig("g_first_scenario");

            //ここに選んだキャラクターとステージの番号入れてください。
            int characterNumb = CharacterSelectSystem.SelectCharacter;
            int stageNumb = StageSelectSystem.SelectStage;

            switch (characterNumb)
            {
                case 0: //ガリレオ
                    switch (stageNumb)
                    {
                        case 0:
                            scenario_file = this.gameManager.getConfig("g_first_scenario");
                            break;

                        case 1:
                            scenario_file = this.gameManager.getConfig("g_second_scenario");
                            break;

                        case 2:
                            scenario_file = this.gameManager.getConfig("g_third_scenario");
                            break;

                        case 3:
                            scenario_file = this.gameManager.getConfig("g_fourth_scenario");
                            break;
                    }
                    break;

                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    break;
            }

            this.gameManager.loadScenario(scenario_file);

            this.gameManager.nextOrder();


           
        }
    }

    void OnGUI()
    {

    }


    public void wait(float time)
    {

        //処理を止める
        StartCoroutine("startWait", time);
    }


    public void coroutineShowMessage(string message)
    {
        StatusManager.isMessageShowing = true;
        StartCoroutine("showMessage", message);
    }

    //一定時間処理を停止するためのコルーチン
    private IEnumerator startWait(float time)
    {

        yield return new WaitForSeconds(time);

        StatusManager.enableEventClick = true;
        StatusManager.enableClickOrder = true;
        StatusManager.enableNextOrder = true;

        this.gameManager.nextOrder();

        yield return null;
    }

    public float MessageSpeed
    {
        get
        {
            return this.messageSpeed;
        }
        set
        {
            this.messageSpeed = value;
        }
    }

    //現在のメッセージをクリアする
    public void clearCurrentMessage()
    {
        this.messageForSaveTitle = this.currentMessage;

        GameObject obj = GameObject.Find("Canvas/_sp_chara_name");

        Color c;
        string name = "";

        if (obj == null)
        {
            c = Color.white;

        }
        else
        {
            c = obj.GetComponent<Text>().color;
            name = obj.GetComponent<Text>().text;
        }
        //バックログ用
        string color = ColorX.RGBToHex(c);
        this.gameManager.logManager.addLog(name, color, this.currentMessage);

        this.currentMessage = "";

    }

    public string CurrentMessage
    {
        get
        {
            return this.currentMessage;
        }
    }

    //メッセージ表示制御用のコルーチン
    private IEnumerator showMessage(string message)
    {

        this.messageForSaveTitle = message;

        //スキップモードの場合は速度アップ
        if (StatusManager.FlagSkiiping == true)
        {

            this.gameManager.scene.MessageSpeed = 0.001f;

        }

        for (int i = 0; i < message.Length; i++)
        {

            //スキップモードの場合は一度に複数の文字列を表示する

            if (StatusManager.FlagSkiiping == true)
            {

                for (var k = 0; (k < 5 && i < message.Length); k++)
                {
                    this.currentMessage += message[i];
                    i++;
                }
                i--;

            }
            else
            {
                this.currentMessage += message[i];
            }

            this.gameView.messageArea.text = this.currentMessage;
            yield return new WaitForSeconds(this.messageSpeed);
        }

        StatusManager.isMessageShowing = false;
        this.messageSpeed = float.Parse(this.gameManager.getConfig("messageSpeed"));

        this.gameManager.nextOrder();

        yield return null;
    }

    public void coroutineAnimation(Animation a, CompleteDelegate completeDeletgate)
    {

        object[] parameters = new object[2] { a, completeDeletgate };

        StatusManager.enableNextOrder = false;
        StartCoroutine("animationWait", parameters);

    }

    private IEnumerator animationWait(object[] param)
    {

        Animation a = (Animation)param[0];
        CompleteDelegate completeDeletgate = (CompleteDelegate)param[1];

        //アニメーションの終了を待つ

        while (a.isPlaying)
        {
            // childのisComplete変数がtrueになるまで待機
            yield return new WaitForEndOfFrame();

        }

        completeDeletgate();
    }


    // Update is called once per frame
    void Update()
    {

        //this.gameManager.check ();
        if (Input.GetMouseButtonUp(0))
        {

            StartCoroutine("ClickButton");
        }
    }

    private IEnumerator ClickButton()
    {

        yield return new WaitForSeconds(0.01f);

        Vector3 aTapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D aCollider2d = Physics2D.OverlapPoint(aTapPoint);

        //Debug.Log ("====EVENT");
        //Debug.Log (aCollider2d);

        if (StatusManager.isEventButtonStop == true)
        {
            StatusManager.isEventButtonStop = false;
            yield break;
        }


        if (StatusManager.isEventStop == false && aCollider2d)
        {

            GameObject obj = aCollider2d.transform.gameObject;

            //特別な機能を持つ
            string name = obj.name;

            gameManager.eventManager.checkEvent(obj.name, "click");

        }
        else
        {

            if (StatusManager.inUiClick == true)
            {
                StatusManager.inUiClick = false;
                yield break;
            }

            GameManager gameManager = NovelSingleton.GameManager;

            //skip中にクリックされた場合、Skipを止める
            if (StatusManager.FlagSkiiping == true)
            {
                StatusManager.FlagSkiiping = false;
            }

            //Auto中にクリックされた場合、Autoを止める
            if (StatusManager.FlagAuto == true)
            {
                StatusManager.FlagAuto = false;
            }

            //ステータスマネージャみたいなの持たせてもいいよね
            if (StatusManager.isMessageShowing == true)
            {
                //速度を上げる
                gameManager.scene.MessageSpeed = 0.001f;
                yield break;
            }

            if (StatusManager.enableClickOrder == true)
            {

                gameManager.clickNextOrder();
            }
        }
    }


    //スキップをスタートさせる
    public void startSkip()
    {

        StatusManager.FlagSkiiping = true;
        StartCoroutine("Loop", 0.1f);

    }

    //文字速度とかも変更しないと
    public void stopSkip()
    {
        StatusManager.FlagSkiiping = false;
        this.MessageSpeed = float.Parse(this.gameManager.getConfig("messageSpeed"));

    }

    //オートを開始させる
    public void startAuto(float time)
    {

        StatusManager.FlagAuto = true;
        StartCoroutine("Loop", time);

    }

    //オートが停止される
    public void stopAuto()
    {
        StatusManager.FlagAuto = false;
        //StartCoroutine("Loop",3.0f);

    }

    public IEnumerator Loop(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            this.gameManager.clickNextOrder();

            if (StatusManager.FlagSkiiping == false && StatusManager.FlagAuto == false)
            {
                break;
            }
        }

        StopCoroutine("Loop");
    }

    //アプリ終了前
    void OnApplicationQuit()
    {

        //gameManager.saveManager.saveFromSnap("autosave"); 

    }
}

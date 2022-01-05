using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviour
{
    // 定数--------------------------------
    // 状態
    enum eSTATE
    {
        NONE = -1,

        FADE_IN,
        READY,
        PLAY,
        RESULT,
        FADE_OUT,

        MAX
    }

    // ターゲットの最大数
    const int TARGET_MAX_NUM = 10;
    // クリア判定を確認する間隔
    const float CHECK_SPAN = 1.0f;
    // 待機時間
    const float READY_WAIT_TIME = 3.5f;

    // 変数--------------------------------
    // 各ターゲット
    [SerializeField] GameObject[] targets = new GameObject[TARGET_MAX_NUM];
    // リザルト用テキスト
    [SerializeField] GameObject resultText, readyText;

    // シーン遷移時の演出
    GameObject fader;

    // 現在の状態
    eSTATE state;

    // クリア判定
    bool clear;

    // 弾の状態
    bool bulletAlive;

    // クリア判定を確認する間隔を計るタイマー Readyテキストを表示する時間を計るタイマー
    float checkTimer, readyTimer;

    // Start is called before the first frame update
    void Start()
    {
        // 状態の設定
        state = eSTATE.FADE_IN;
        // 演出用オブジェクトを生成する
        fader = gameObject.GetComponent<GenerateFader>().Generate();
        // 未クリア状態
        clear = false;
        // 弾が消えてない状態
        bulletAlive = true;
        // タイマーリセット
        checkTimer = 0;
        readyTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(state);
        switch(state)
        {
            case eSTATE.FADE_IN:    FadeIn();   break;
            case eSTATE.READY:      Ready();    break;
            case eSTATE.PLAY:       Play();     break;
            case eSTATE.RESULT:     Result();   break;
            case eSTATE.FADE_OUT:   FadeOut();  break;
        }
    }

    void FadeIn()
    {
        if (fader.GetComponent<Fader>().Fading()) state = eSTATE.READY;
    }

    void Ready()
    {
        // タイマーの計測
        readyTimer += Time.deltaTime;
        // Readyテキストを表示する
        readyText.SetActive(true);
        // 一定時間経過したら次の状態に移行
        if (readyTimer >= READY_WAIT_TIME) state = eSTATE.PLAY;
    }

    void Play()
    {
        // タイマーカウント
        checkTimer += Time.deltaTime;

        // ターゲットが全滅しているか確認し、全滅しているならクリアフラグを立てる
        if (TargetCheck())
        {
            clear = true;           // クリアフラグを立てる
            state = eSTATE.RESULT;  // リザルト状態に移行
        }

        // 弾が消えたらリザルト状態に移行する
        if (!bulletAlive) state = eSTATE.RESULT;
    }

    void Result()
    {
        // Resultテキストを表示する
        resultText.SetActive(true);

        // 特定キーが押されたら次の状態に移行する
        if (Input.GetKeyDown(KeyCode.Space)) state = eSTATE.FADE_OUT;
    }

    void FadeOut()
    {
        if (fader.GetComponent<Fader>().Fading()) SceneManager.LoadScene("StageSelectScene");
    }

    bool TargetCheck()
    {
        // タイマーが一定時間経過してない場合falseを返す
        if (checkTimer < CHECK_SPAN) return false;

        // タイマーリセット
        checkTimer = 0;

        // ターゲットが生存しているか判定
        for (int i = 0; i < targets.Length; i++)
        {
            // ターゲットが一つでも生存している場合、falseを返す
            if (targets[i] != null) return false;
        }
        
        // ターゲットが全滅している場合、trueを返す
        return true;
    }

    // クリアフラグを渡す
    public bool GetClearFlag()
    {
        return clear;
    }

    public void BulletLost()
    {
        bulletAlive = false;
    }
}

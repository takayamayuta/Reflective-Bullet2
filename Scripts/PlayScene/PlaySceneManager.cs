using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviour
{
    // 定数--------------------------------
    // 状態
    public enum eSTATE
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
    // 弾が消えてからの待機時間
    const float WAIT_TIME = 2.0f;

    // 変数--------------------------------
    // 各ターゲット
    [SerializeField] GameObject[] targets = new GameObject[TARGET_MAX_NUM];
    // リザルト用テキスト
    [SerializeField] GameObject resultText, readyText;
    // 成功時、失敗時のSE
    [SerializeField] AudioClip clearSE, failedSE;

    // シーン遷移時の演出
    GameObject fader;

    // 現在の状態
    eSTATE state;

    // クリア判定
    bool clear;

    // 弾の状態
    bool bulletAlive;

    // クリア判定を確認する間隔を計るタイマー 待機時間を計測するタイマー
    float checkTimer, waitTimer;

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
        waitTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
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
        waitTimer += Time.deltaTime;
        // Readyテキストを表示する
        readyText.SetActive(true);
        // 一定時間経過したら
        if (waitTimer >= READY_WAIT_TIME)
        {
            // 次の状態に移行
            state = eSTATE.PLAY;
            // タイマーリセット
            waitTimer = 0;
        }
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

        // 弾が消えたらタイマー計測
        if (!bulletAlive)　waitTimer += Time.deltaTime;
        // 一定時間経過でリザルト状態に移行
        if (waitTimer >= WAIT_TIME) state = eSTATE.RESULT;
    }

    void Result()
    {
        // クリア判定によって流すSEを変える
        if (!resultText.activeSelf)
        {
            if (clear) GetComponent<AudioSource>().PlayOneShot(clearSE);
            else GetComponent<AudioSource>().PlayOneShot(failedSE);
        }

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

    public eSTATE GetState()
    {
        return state;
    }
}

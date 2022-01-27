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

    public enum eSCENE
    {
        NONE = -1,

        TITLE,
        STAGE_SELECT,
        PLAY,

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
    // リザルト用テキスト、プレイ開始前テキスト、ポーズ画面
    [SerializeField] GameObject resultText, readyText, pauseText;
    // 成功時、失敗時のSE、ポーズ画面を開閉した時のSE、次のシーンに行くときのSE
    [SerializeField] AudioClip clearSE, failedSE, pauseSE, enterSE;

    // シーン遷移時の演出
    GameObject fader;

    // 現在の状態
    eSTATE state;

    // クリア判定、弾の状態、ポーズ状態
    bool clear, bulletAlive, pause;

    // クリア判定を確認する間隔を計るタイマー 待機時間を計測するタイマー
    float checkTimer, waitTimer;

    // 選択されているシーン先
    eSCENE nextScene;

    // Start is called before the first frame update
    void Start()
    {
        // 状態の設定
        state = eSTATE.FADE_IN;
        // 演出用オブジェクトを生成する
        fader = gameObject.GetComponent<GenerateFader>().Generate();
        fader.transform.SetSiblingIndex(pauseText.transform.GetSiblingIndex());
        // 未クリア状態
        clear = false;
        // 弾が消えてない状態
        bulletAlive = true;
        // ポーズ状態じゃない
        pause = false;
        // タイマーリセット
        checkTimer = 0;
        waitTimer = 0;

        nextScene = eSCENE.STAGE_SELECT;
    }

    // Update is called once per frame
    void Update()
    {
        // 各状態毎の処理を行う
        switch(state)
        {
            case eSTATE.FADE_IN:    FadeIn();   break;
            case eSTATE.READY:      Ready();    break;
            case eSTATE.PLAY:       Play();     break;
            case eSTATE.RESULT:     Result();   break;
            case eSTATE.FADE_OUT:   FadeOut();  break;
        }

        // ポーズ処理
        Pause();
    }

    // フェードイン
    void FadeIn()
    {
        if (fader.GetComponent<Fader>().Fading()) state = eSTATE.READY;
    }

    // 準備
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

    // プレイ
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

    // ポーズ
    void Pause()
    {
        // 特定キーが押されたら以下の処理を行う
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause)
            {
                // ゲーム内進行時間を１にする
                Time.timeScale = 1.0f;
                // ポーズ画面を非表示する
                pauseText.SetActive(false);
            }
            else
            {
                // ゲーム内進行時間を０にする
                Time.timeScale = 0;
                // ポーズ画面を表示する
                pauseText.SetActive(true);
            }
            // ポーズ状態の切り替え
            pause = !pause;
            // SEを流す
            GetComponent<AudioSource>().PlayOneShot(pauseSE);
        }
    }

    // リザルト
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = eSTATE.FADE_OUT;
            // SEを流す
            GetComponent<AudioSource>().PlayOneShot(enterSE);
        }
    }

    // フェードアウト
    void FadeOut()
    {
        // フェードアウトが完了するまで以下の処理を飛ばす
        if (fader.GetComponent<Fader>().Fading())
        {
            // 選択された各シーンに遷移する
            if (nextScene == eSCENE.STAGE_SELECT) SceneManager.LoadScene("StageSelectScene");
            if (nextScene == eSCENE.PLAY) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // ターゲットが全滅しているか確認する
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

    // 弾の生存フラグをfalseにする
    public void BulletLost()
    {
        bulletAlive = false;
    }

    // 状態情報を渡す
    public eSTATE GetState()
    {
        return state;
    }
    
    // ターゲットの数を渡す
    public int GetTargetNum()
    {
        return targets.Length;
    }

    // ボタンを選択した
    public void SelectButton(eSCENE scene)
    {
        // 状態の移行
        state = eSTATE.FADE_OUT;
        // 選択されたシーンを記憶する
        nextScene = scene;
        // ゲームの進行速度を戻す
        Time.timeScale = 1.0f;
        // フェーダーを一番手前にもってくる
        fader.transform.SetAsLastSibling();
    }
}

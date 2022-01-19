using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectSceneManager : MonoBehaviour
{
    // 定数--------------------------------
    // 各ステージ
    public enum eSELECT
    {
        NONE = -1,

        STAGE1,
        STAGE2,
        STAGE3,
        STAGE4,
        STAGE5,

        MAX
    }

    // 状態
    enum eSTATE
    {
        NONE = -1,

        FADE_IN,
        SELECT,
        FADE_OUT,

        MAX
    }

    // 変数--------------------------------
    // 選択時のSEと決定時のSE
    [SerializeField] AudioClip selectSE, decisionSE;

    // フェードイン・フェードアウトするオブジェクト
    GameObject fader;
    // 選択ステージ
    eSELECT select;
    // 現在の状態
    eSTATE state;
    // カメラが動いてる状態
    bool moving;        

    // Start is called before the first frame update
    void Start()
    {
        // 最初に選択されているステージ
        select = eSELECT.STAGE1;
        // フェードイン状態に設定
        state = eSTATE.FADE_IN;
        // 演出用オブジェクトを生成する
        fader = gameObject.GetComponent<GenerateFader>().Generate();
        // カメラが動いていない状態
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        // それぞれの状態時の処理
        switch (state)
        {
            // フェードイン
            case eSTATE.FADE_IN: FadeIn(); break;
            // ステージ選択
            case eSTATE.SELECT: Select(); break;
            // フェードアウト
            case eSTATE.FADE_OUT: FadeOut(); break;
        }
    }

    // ステージを選択
    void Select()
    {
        // 右ボタンが押されたら右隣のステージを選択する
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // 選択用SEを流す
            GetComponent<AudioSource>().PlayOneShot(selectSE);

            // 選択ステージの変更
            select++;
            // 右隣に選択できるステージがないなら選択ステージの変更をしない
            if (select >= eSELECT.MAX)
            {
                // 一番右のステージを選択している状態にする
                select = eSELECT.MAX - 1;

                return;
            }
            // カメラが動いている状態
            moving = true;
        }
        //左ボタンが押されたら左隣のステージを選択する
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // 選択用SEを流す
            GetComponent<AudioSource>().PlayOneShot(selectSE);

            // 選択ステージの変更
            select--;
            // 左隣に選択できるステージがないなら選択ステージの変更をしない
            if (select <= eSELECT.NONE)
            {
                // 一番左のステージを選択している状態にする
                select = eSELECT.NONE + 1;

                return;
            }
            // カメラが動いている状態
            moving = true;
        }

        // 特定キーが押されたらフェードアウト状態に移行する
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 決定用SEを流す
            GetComponent<AudioSource>().PlayOneShot(decisionSE);

            state = eSTATE.FADE_OUT;
        }
    }

    // カメラが動いている状態か判定フラグを渡す
    public bool GetMoving()
    {
        return moving;
    }

    // 選択しているステージの情報を渡す
    public eSELECT GetSelectStage()
    {
        return select;
    }

    // 移動済
    public void Moved()
    {
        moving = false;
    }

    // 選択されたステージに遷移する
    void NextScene()
    {        
        // 選択されたステージに遷移するために値の調整
        int stageNum = (int)select + 1;
        // 選択されたステージの読み込み
        SceneManager.LoadScene("Stage" + stageNum.ToString() + "Scene");        
    }

    // フェードイン
    void FadeIn()
    {
        // フェードインしたら選択状態に移行する
        if (fader.GetComponent<Fader>().Fading()) state = eSTATE.SELECT;
    }

    // フェードアウト
    void FadeOut()
    {
        // フェードアウトしたら選択されたシーンに遷移する
        if (fader.GetComponent<Fader>().Fading()) NextScene();
    }
}

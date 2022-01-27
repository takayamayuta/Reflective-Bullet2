using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    // 定数--------------------------------
    enum eSTATE
    {
        NONE = -1,

        FADE_IN,
        CAMERA_MOVE,
        WAIT,
        FADE_OUT,

        MAX
    }

    // 変数--------------------------------
    [SerializeField] GameObject camera, titleInitial, titleWord, tapToStart;
    // タップして次のシーンに行くときのSE
    [SerializeField] AudioClip enterSE;

    // 現在の状態を保持
    eSTATE state;
    // フェードイン・フェードアウトするオブジェクト
    GameObject fader;    

    // Start is called before the first frame update
    void Start()
    {
        // フェードインから開始するよう設定
        state = eSTATE.FADE_IN;
        // フェーダーを生成
        fader = gameObject.GetComponent<GenerateFader>().Generate();
        // 各文字を非表示にしておく
        titleInitial.SetActive(false);
        titleWord.SetActive(false);
        tapToStart.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 状態毎に処理を行う
        switch (state)
        {
            case eSTATE.FADE_IN: FadeIn(); break;
            case eSTATE.CAMERA_MOVE: CameraMove(); break;
            case eSTATE.WAIT: Wait(); break;
            case eSTATE.FADE_OUT: FadeOut(); break;
        }
    }

    void FadeIn()
    {
        // フェードインし終わったら次の状態に移行
        if (fader.GetComponent<Fader>().Fading()) state = eSTATE.CAMERA_MOVE;
    }

    void CameraMove()
    {
        // カメラの移動とズームアウトが終わったら次の状態に移行する
        if (camera.GetComponent<TitleCamera>().Moved() && camera.GetComponent<TitleCamera>().ZoomedOut())
        {
            // 状態の移行
            state = eSTATE.WAIT;
            // タイトルのイニシャル文字を表示する
            titleInitial.SetActive(true);
            // TAP TO STARTの文字を表示する
            tapToStart.SetActive(true);
        }
    }

    void Wait()
    {
        // 特定キーが押されたら次の状態に移行する
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = eSTATE.FADE_OUT;
            // SEを流す
            GetComponent<AudioSource>().PlayOneShot(enterSE);
        }

        // タイトルのイニシャル文字の演出が終わったら他の文字も表示する
        if (!titleInitial.GetComponent<TypefaceAnimator>().isPlaying) titleWord.SetActive(true);
    }

    void FadeOut()
    {
        // フェードアウトし終わったら次のシーンに遷移する
        if (fader.GetComponent<Fader>().Fading()) SceneManager.LoadScene("StageSelectScene");
    }
}

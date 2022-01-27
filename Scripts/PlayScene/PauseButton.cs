using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    // シーンマネージャー
    [SerializeField] GameObject sceneManager;

    // リトライをクリックされたときの処理
    public void RetryOnClick()
    {
        sceneManager = GameObject.Find("SceneManager");
        sceneManager.GetComponent<PlaySceneManager>().SelectButton(PlaySceneManager.eSCENE.PLAY);
    }

    // ステージセレクトをクリックされたときの処理
    public void StageSelectOnClick()
    {
        sceneManager = GameObject.Find("SceneManager");
        sceneManager.GetComponent<PlaySceneManager>().SelectButton(PlaySceneManager.eSCENE.STAGE_SELECT);
    }
}

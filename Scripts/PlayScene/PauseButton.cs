using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    // �V�[���}�l�[�W���[
    [SerializeField] GameObject sceneManager;

    // ���g���C���N���b�N���ꂽ�Ƃ��̏���
    public void RetryOnClick()
    {
        sceneManager = GameObject.Find("SceneManager");
        sceneManager.GetComponent<PlaySceneManager>().SelectButton(PlaySceneManager.eSCENE.PLAY);
    }

    // �X�e�[�W�Z���N�g���N���b�N���ꂽ�Ƃ��̏���
    public void StageSelectOnClick()
    {
        sceneManager = GameObject.Find("SceneManager");
        sceneManager.GetComponent<PlaySceneManager>().SelectButton(PlaySceneManager.eSCENE.STAGE_SELECT);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    // �萔--------------------------------
    enum eSTATE
    {
        NONE = -1,

        FADE_IN,
        CAMERA_MOVE,
        WAIT,
        FADE_OUT,

        MAX
    }

    // �ϐ�--------------------------------
    [SerializeField] GameObject camera, titleInitial, titleWord, tapToStart;
    // �^�b�v���Ď��̃V�[���ɍs���Ƃ���SE
    [SerializeField] AudioClip enterSE;

    // ���݂̏�Ԃ�ێ�
    eSTATE state;
    // �t�F�[�h�C���E�t�F�[�h�A�E�g����I�u�W�F�N�g
    GameObject fader;    

    // Start is called before the first frame update
    void Start()
    {
        // �t�F�[�h�C������J�n����悤�ݒ�
        state = eSTATE.FADE_IN;
        // �t�F�[�_�[�𐶐�
        fader = gameObject.GetComponent<GenerateFader>().Generate();
        // �e�������\���ɂ��Ă���
        titleInitial.SetActive(false);
        titleWord.SetActive(false);
        tapToStart.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // ��Ԗ��ɏ������s��
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
        // �t�F�[�h�C�����I������玟�̏�ԂɈڍs
        if (fader.GetComponent<Fader>().Fading()) state = eSTATE.CAMERA_MOVE;
    }

    void CameraMove()
    {
        // �J�����̈ړ��ƃY�[���A�E�g���I������玟�̏�ԂɈڍs����
        if (camera.GetComponent<TitleCamera>().Moved() && camera.GetComponent<TitleCamera>().ZoomedOut())
        {
            // ��Ԃ̈ڍs
            state = eSTATE.WAIT;
            // �^�C�g���̃C�j�V����������\������
            titleInitial.SetActive(true);
            // TAP TO START�̕�����\������
            tapToStart.SetActive(true);
        }
    }

    void Wait()
    {
        // ����L�[�������ꂽ�玟�̏�ԂɈڍs����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = eSTATE.FADE_OUT;
            // SE�𗬂�
            GetComponent<AudioSource>().PlayOneShot(enterSE);
        }

        // �^�C�g���̃C�j�V���������̉��o���I������瑼�̕������\������
        if (!titleInitial.GetComponent<TypefaceAnimator>().isPlaying) titleWord.SetActive(true);
    }

    void FadeOut()
    {
        // �t�F�[�h�A�E�g���I������玟�̃V�[���ɑJ�ڂ���
        if (fader.GetComponent<Fader>().Fading()) SceneManager.LoadScene("StageSelectScene");
    }
}

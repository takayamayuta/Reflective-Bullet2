using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviour
{
    // �萔--------------------------------
    // ���
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

    // �^�[�Q�b�g�̍ő吔
    const int TARGET_MAX_NUM = 10;
    // �N���A������m�F����Ԋu
    const float CHECK_SPAN = 1.0f;
    // �ҋ@����
    const float READY_WAIT_TIME = 3.5f;
    // �e�������Ă���̑ҋ@����
    const float WAIT_TIME = 2.0f;

    // �ϐ�--------------------------------
    // �e�^�[�Q�b�g
    [SerializeField] GameObject[] targets = new GameObject[TARGET_MAX_NUM];
    // ���U���g�p�e�L�X�g�A�v���C�J�n�O�e�L�X�g�A�|�[�Y���
    [SerializeField] GameObject resultText, readyText, pauseText;
    // �������A���s����SE�A�|�[�Y��ʂ��J��������SE�A���̃V�[���ɍs���Ƃ���SE
    [SerializeField] AudioClip clearSE, failedSE, pauseSE, enterSE;

    // �V�[���J�ڎ��̉��o
    GameObject fader;

    // ���݂̏��
    eSTATE state;

    // �N���A����A�e�̏�ԁA�|�[�Y���
    bool clear, bulletAlive, pause;

    // �N���A������m�F����Ԋu���v��^�C�}�[ �ҋ@���Ԃ��v������^�C�}�[
    float checkTimer, waitTimer;

    // �I������Ă���V�[����
    eSCENE nextScene;

    // Start is called before the first frame update
    void Start()
    {
        // ��Ԃ̐ݒ�
        state = eSTATE.FADE_IN;
        // ���o�p�I�u�W�F�N�g�𐶐�����
        fader = gameObject.GetComponent<GenerateFader>().Generate();
        fader.transform.SetSiblingIndex(pauseText.transform.GetSiblingIndex());
        // ���N���A���
        clear = false;
        // �e�������ĂȂ����
        bulletAlive = true;
        // �|�[�Y��Ԃ���Ȃ�
        pause = false;
        // �^�C�}�[���Z�b�g
        checkTimer = 0;
        waitTimer = 0;

        nextScene = eSCENE.STAGE_SELECT;
    }

    // Update is called once per frame
    void Update()
    {
        // �e��Ԗ��̏������s��
        switch(state)
        {
            case eSTATE.FADE_IN:    FadeIn();   break;
            case eSTATE.READY:      Ready();    break;
            case eSTATE.PLAY:       Play();     break;
            case eSTATE.RESULT:     Result();   break;
            case eSTATE.FADE_OUT:   FadeOut();  break;
        }

        // �|�[�Y����
        Pause();
    }

    // �t�F�[�h�C��
    void FadeIn()
    {
        if (fader.GetComponent<Fader>().Fading()) state = eSTATE.READY;
    }

    // ����
    void Ready()
    {
        // �^�C�}�[�̌v��
        waitTimer += Time.deltaTime;
        // Ready�e�L�X�g��\������
        readyText.SetActive(true);
        // ��莞�Ԍo�߂�����
        if (waitTimer >= READY_WAIT_TIME)
        {
            // ���̏�ԂɈڍs
            state = eSTATE.PLAY;
            // �^�C�}�[���Z�b�g
            waitTimer = 0;
        }
    }

    // �v���C
    void Play()
    {
        // �^�C�}�[�J�E���g
        checkTimer += Time.deltaTime;

        // �^�[�Q�b�g���S�ł��Ă��邩�m�F���A�S�ł��Ă���Ȃ�N���A�t���O�𗧂Ă�
        if (TargetCheck())
        {
            clear = true;           // �N���A�t���O�𗧂Ă�
            state = eSTATE.RESULT;  // ���U���g��ԂɈڍs
        }

        // �e����������^�C�}�[�v��
        if (!bulletAlive)�@waitTimer += Time.deltaTime;
        // ��莞�Ԍo�߂Ń��U���g��ԂɈڍs
        if (waitTimer >= WAIT_TIME) state = eSTATE.RESULT;
    }

    // �|�[�Y
    void Pause()
    {
        // ����L�[�������ꂽ��ȉ��̏������s��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause)
            {
                // �Q�[�����i�s���Ԃ��P�ɂ���
                Time.timeScale = 1.0f;
                // �|�[�Y��ʂ��\������
                pauseText.SetActive(false);
            }
            else
            {
                // �Q�[�����i�s���Ԃ��O�ɂ���
                Time.timeScale = 0;
                // �|�[�Y��ʂ�\������
                pauseText.SetActive(true);
            }
            // �|�[�Y��Ԃ̐؂�ւ�
            pause = !pause;
            // SE�𗬂�
            GetComponent<AudioSource>().PlayOneShot(pauseSE);
        }
    }

    // ���U���g
    void Result()
    {
        // �N���A����ɂ���ė���SE��ς���
        if (!resultText.activeSelf)
        {
            if (clear) GetComponent<AudioSource>().PlayOneShot(clearSE);
            else GetComponent<AudioSource>().PlayOneShot(failedSE);
        }

        // Result�e�L�X�g��\������
        resultText.SetActive(true);

        // ����L�[�������ꂽ�玟�̏�ԂɈڍs����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = eSTATE.FADE_OUT;
            // SE�𗬂�
            GetComponent<AudioSource>().PlayOneShot(enterSE);
        }
    }

    // �t�F�[�h�A�E�g
    void FadeOut()
    {
        // �t�F�[�h�A�E�g����������܂ňȉ��̏������΂�
        if (fader.GetComponent<Fader>().Fading())
        {
            // �I�����ꂽ�e�V�[���ɑJ�ڂ���
            if (nextScene == eSCENE.STAGE_SELECT) SceneManager.LoadScene("StageSelectScene");
            if (nextScene == eSCENE.PLAY) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // �^�[�Q�b�g���S�ł��Ă��邩�m�F����
    bool TargetCheck()
    {
        // �^�C�}�[����莞�Ԍo�߂��ĂȂ��ꍇfalse��Ԃ�
        if (checkTimer < CHECK_SPAN) return false;

        // �^�C�}�[���Z�b�g
        checkTimer = 0;

        // �^�[�Q�b�g���������Ă��邩����
        for (int i = 0; i < targets.Length; i++)
        {
            // �^�[�Q�b�g����ł��������Ă���ꍇ�Afalse��Ԃ�
            if (targets[i] != null) return false;
        }
        
        // �^�[�Q�b�g���S�ł��Ă���ꍇ�Atrue��Ԃ�
        return true;
    }

    // �N���A�t���O��n��
    public bool GetClearFlag()
    {
        return clear;
    }

    // �e�̐����t���O��false�ɂ���
    public void BulletLost()
    {
        bulletAlive = false;
    }

    // ��ԏ���n��
    public eSTATE GetState()
    {
        return state;
    }
    
    // �^�[�Q�b�g�̐���n��
    public int GetTargetNum()
    {
        return targets.Length;
    }

    // �{�^����I������
    public void SelectButton(eSCENE scene)
    {
        // ��Ԃ̈ڍs
        state = eSTATE.FADE_OUT;
        // �I�����ꂽ�V�[�����L������
        nextScene = scene;
        // �Q�[���̐i�s���x��߂�
        Time.timeScale = 1.0f;
        // �t�F�[�_�[����Ԏ�O�ɂ����Ă���
        fader.transform.SetAsLastSibling();
    }
}

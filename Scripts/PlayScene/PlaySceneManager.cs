using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviour
{
    // �萔--------------------------------
    // ���
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

    // �^�[�Q�b�g�̍ő吔
    const int TARGET_MAX_NUM = 10;
    // �N���A������m�F����Ԋu
    const float CHECK_SPAN = 1.0f;
    // �ҋ@����
    const float READY_WAIT_TIME = 3.5f;

    // �ϐ�--------------------------------
    // �e�^�[�Q�b�g
    [SerializeField] GameObject[] targets = new GameObject[TARGET_MAX_NUM];
    // ���U���g�p�e�L�X�g
    [SerializeField] GameObject resultText, readyText;

    // �V�[���J�ڎ��̉��o
    GameObject fader;

    // ���݂̏��
    eSTATE state;

    // �N���A����
    bool clear;

    // �e�̏��
    bool bulletAlive;

    // �N���A������m�F����Ԋu���v��^�C�}�[ Ready�e�L�X�g��\�����鎞�Ԃ��v��^�C�}�[
    float checkTimer, readyTimer;

    // Start is called before the first frame update
    void Start()
    {
        // ��Ԃ̐ݒ�
        state = eSTATE.FADE_IN;
        // ���o�p�I�u�W�F�N�g�𐶐�����
        fader = gameObject.GetComponent<GenerateFader>().Generate();
        // ���N���A���
        clear = false;
        // �e�������ĂȂ����
        bulletAlive = true;
        // �^�C�}�[���Z�b�g
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
        // �^�C�}�[�̌v��
        readyTimer += Time.deltaTime;
        // Ready�e�L�X�g��\������
        readyText.SetActive(true);
        // ��莞�Ԍo�߂����玟�̏�ԂɈڍs
        if (readyTimer >= READY_WAIT_TIME) state = eSTATE.PLAY;
    }

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

        // �e���������烊�U���g��ԂɈڍs����
        if (!bulletAlive) state = eSTATE.RESULT;
    }

    void Result()
    {
        // Result�e�L�X�g��\������
        resultText.SetActive(true);

        // ����L�[�������ꂽ�玟�̏�ԂɈڍs����
        if (Input.GetKeyDown(KeyCode.Space)) state = eSTATE.FADE_OUT;
    }

    void FadeOut()
    {
        if (fader.GetComponent<Fader>().Fading()) SceneManager.LoadScene("StageSelectScene");
    }

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

    public void BulletLost()
    {
        bulletAlive = false;
    }
}
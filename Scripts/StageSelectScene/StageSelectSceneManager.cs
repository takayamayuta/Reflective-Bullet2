using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectSceneManager : MonoBehaviour
{
    // �萔--------------------------------
    // �e�X�e�[�W
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

    // ���
    enum eSTATE
    {
        NONE = -1,

        FADE_IN,
        SELECT,
        FADE_OUT,

        MAX
    }

    // �ϐ�--------------------------------
    // �I������SE�ƌ��莞��SE
    [SerializeField] AudioClip selectSE, decisionSE;

    // �t�F�[�h�C���E�t�F�[�h�A�E�g����I�u�W�F�N�g
    GameObject fader;
    // �I���X�e�[�W
    eSELECT select;
    // ���݂̏��
    eSTATE state;
    // �J�����������Ă���
    bool moving;        

    // Start is called before the first frame update
    void Start()
    {
        // �ŏ��ɑI������Ă���X�e�[�W
        select = eSELECT.STAGE1;
        // �t�F�[�h�C����Ԃɐݒ�
        state = eSTATE.FADE_IN;
        // ���o�p�I�u�W�F�N�g�𐶐�����
        fader = gameObject.GetComponent<GenerateFader>().Generate();
        // �J�����������Ă��Ȃ����
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ���ꂼ��̏�Ԏ��̏���
        switch (state)
        {
            // �t�F�[�h�C��
            case eSTATE.FADE_IN: FadeIn(); break;
            // �X�e�[�W�I��
            case eSTATE.SELECT: Select(); break;
            // �t�F�[�h�A�E�g
            case eSTATE.FADE_OUT: FadeOut(); break;
        }
    }

    // �X�e�[�W��I��
    void Select()
    {
        // �E�{�^���������ꂽ��E�ׂ̃X�e�[�W��I������
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // �I��pSE�𗬂�
            GetComponent<AudioSource>().PlayOneShot(selectSE);

            // �I���X�e�[�W�̕ύX
            select++;
            // �E�ׂɑI���ł���X�e�[�W���Ȃ��Ȃ�I���X�e�[�W�̕ύX�����Ȃ�
            if (select >= eSELECT.MAX)
            {
                // ��ԉE�̃X�e�[�W��I�����Ă����Ԃɂ���
                select = eSELECT.MAX - 1;

                return;
            }
            // �J�����������Ă�����
            moving = true;
        }
        //���{�^���������ꂽ�獶�ׂ̃X�e�[�W��I������
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // �I��pSE�𗬂�
            GetComponent<AudioSource>().PlayOneShot(selectSE);

            // �I���X�e�[�W�̕ύX
            select--;
            // ���ׂɑI���ł���X�e�[�W���Ȃ��Ȃ�I���X�e�[�W�̕ύX�����Ȃ�
            if (select <= eSELECT.NONE)
            {
                // ��ԍ��̃X�e�[�W��I�����Ă����Ԃɂ���
                select = eSELECT.NONE + 1;

                return;
            }
            // �J�����������Ă�����
            moving = true;
        }

        // ����L�[�������ꂽ��t�F�[�h�A�E�g��ԂɈڍs����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ����pSE�𗬂�
            GetComponent<AudioSource>().PlayOneShot(decisionSE);

            state = eSTATE.FADE_OUT;
        }
    }

    // �J�����������Ă����Ԃ�����t���O��n��
    public bool GetMoving()
    {
        return moving;
    }

    // �I�����Ă���X�e�[�W�̏���n��
    public eSELECT GetSelectStage()
    {
        return select;
    }

    // �ړ���
    public void Moved()
    {
        moving = false;
    }

    // �I�����ꂽ�X�e�[�W�ɑJ�ڂ���
    void NextScene()
    {        
        // �I�����ꂽ�X�e�[�W�ɑJ�ڂ��邽�߂ɒl�̒���
        int stageNum = (int)select + 1;
        // �I�����ꂽ�X�e�[�W�̓ǂݍ���
        SceneManager.LoadScene("Stage" + stageNum.ToString() + "Scene");        
    }

    // �t�F�[�h�C��
    void FadeIn()
    {
        // �t�F�[�h�C��������I����ԂɈڍs����
        if (fader.GetComponent<Fader>().Fading()) state = eSTATE.SELECT;
    }

    // �t�F�[�h�A�E�g
    void FadeOut()
    {
        // �t�F�[�h�A�E�g������I�����ꂽ�V�[���ɑJ�ڂ���
        if (fader.GetComponent<Fader>().Fading()) NextScene();
    }
}

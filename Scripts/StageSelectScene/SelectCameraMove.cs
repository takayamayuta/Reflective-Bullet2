using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCameraMove : MonoBehaviour
{
    // �萔--------------------------------
    enum eSTATE
    {
        NONE = -1,

        WAIT,           // �ҋ@
        ACCELERATION,   // ����
        DECELERATE      // ����
    }

    const float ACCELE = 1.1f;          // �����x
    const float DECELE = 0.8f;          // �����x
    const float POS_DIFFERENCE = 18.0f; // �X�e�[�W�Ԃ̍��W�̍�
    const float START_SPEED = 0.5f;     // �ŏ��̑��x
    const float CHANGE_STATE_POS = 5.0f;// �I�����ꂽ�X�e�[�W�̍��W����w�肳�ꂽ�l�ȓ��ɋ߂Â����猸����Ԃɐ؂�ւ���
    const float CAMERA_POS_Z = -10.0f;  // �J������Z���W

    // �ϐ�--------------------------------
    // �V�[���}�l�[�W���[
    [SerializeField] GameObject sceneManager;

    // �I�����Ă����X�e�[�W�̍��W
    Vector3 selectedPos;

    // �ړ����x
    float vel;

    // ���
    eSTATE state;                              

    // Start is called before the first frame update
    void Start()
    {
        selectedPos = Vector3.zero;     // ���W�����Z�b�g
        vel = START_SPEED;              // �ړ����x�̏�����
        state = eSTATE.WAIT;            // �ҋ@���
    }

    // Update is called once per frame
    void Update()
    {
        // ��Ԗ��̏������s��
        switch (state)
        {
            // �ҋ@
            case eSTATE.WAIT: Wait(); break;
            // ����
            case eSTATE.ACCELERATION: Acceleration(); break;
            // ����
            case eSTATE.DECELERATE: Decelerate(); break;
        }
    }

    // �ҋ@
    void Wait()
    {
        // �ړ���ԂȂ������ԂɈڍs
        if (sceneManager.GetComponent<StageSelectSceneManager>().GetMoving())
        {
            state = eSTATE.ACCELERATION;
            vel = START_SPEED;
        }
    }

    // ����
    void Acceleration()
    {
        // �ړ����x�̏㏸
        vel *= ACCELE;

        // �ړI�n��X���W
        float destination = (int)sceneManager.GetComponent<StageSelectSceneManager>().GetSelectStage() * POS_DIFFERENCE;

        // �I�������X�e�[�W���E�ɃJ����������Ȃ獶�Ɉړ�������
        if (transform.position.x > destination)
        {
            // ���Ɉړ�
            gameObject.transform.Translate(-vel, 0, 0);

            // ���ʒu�܂ňړ������猸����ԂɈڍs
            if (transform.position.x <= destination + CHANGE_STATE_POS)
            {
                state = eSTATE.DECELERATE;
            }
        }
        // �I�������X�e�[�W��荶�ɃJ����������Ȃ�E�Ɉړ�������
        else if (transform.position.x < destination)
        {
            // �E�Ɉړ�
            gameObject.transform.Translate(vel, 0, 0);

            // ���ʒu�܂ňړ������猸����ԂɈڍs
            if (transform.position.x >= destination - CHANGE_STATE_POS)
            {
                state = eSTATE.DECELERATE;
            }
        }
    }

    // ����
    void Decelerate()
    {
        // �ړ����x�̏㏸
        vel *= DECELE;

        // �ړI�n��X���W
        float destination = (int)sceneManager.GetComponent<StageSelectSceneManager>().GetSelectStage() * POS_DIFFERENCE;  

        // �I�������X�e�[�W���E�ɃJ����������Ȃ獶�Ɉړ�������
        if (transform.position.x > destination)
        {
            // �I�����Ă�X�e�[�W���ς���ĖړI�n���ύX���ꂽ���Ԃ�������Ԃɖ߂�
            if (transform.position.x > destination + CHANGE_STATE_POS) state = eSTATE.ACCELERATION;

            // ���Ɉړ�
            gameObject.transform.Translate(-vel, 0, 0);

            // ���ʒu�܂ňړ���������W�𒲐߂��đҋ@��ԂɈڍs
            if (transform.position.x <= destination)
            {
                // ���W�̒���
                transform.position = new Vector3(destination, 0, CAMERA_POS_Z);
                // �ҋ@��ԂɈڍs
                state = eSTATE.WAIT;
                // �ړ��ςɂ���
                sceneManager.GetComponent<StageSelectSceneManager>().Moved();
            }
        }
        // �I�������X�e�[�W��荶�ɃJ����������Ȃ�E�Ɉړ�������
        else if (transform.position.x < destination)
        {
            // �I�����Ă�X�e�[�W���ς���ĖړI�n���ύX���ꂽ���Ԃ�������Ԃɖ߂�
            if (transform.position.x < destination - CHANGE_STATE_POS) state = eSTATE.ACCELERATION;

            // �E�Ɉړ�
            gameObject.transform.Translate(vel, 0, 0);

            // ���ʒu�܂ňړ���������W�𒲐߂��đҋ@��ԂɈڍs
            if (transform.position.x >= destination)
            {
                // ���W�̒���
                transform.position = new Vector3(destination, 0, CAMERA_POS_Z);
                // �ҋ@��ԂɈڍs
                state = eSTATE.WAIT;
                // �ړ��ςɂ���
                sceneManager.GetComponent<StageSelectSceneManager>().Moved();
            }
        }
    }
}

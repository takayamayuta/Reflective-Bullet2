using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCamera : MonoBehaviour
{
    // �萔--------------------------------
    // �J�����̃Y�[���A�E�g���Ă����{��
    const float CAMERA_ZOOM_OUT_NUM = 1.1f;
    // �J�����̃Y�[���A�E�g��������܂ł̒l
    const float CAMERA_ZOOM_SIZE = 5.0f;
    // �J�����̈ړ���
    const float CAMERA_MOVED_POS_Y = 0;
    // �J�����̈ړ��X�s�[�h
    const float CAMERA_MOVE_SPEED = 0.1f;
    // ����Y���W
    const float START_POS_Y = -0.25f;
    // �����Y�[���T�C�Y
    const float START_ZOOM_SIZE = 0.3f;

    // �ϐ�--------------------------------
    bool cameraMoved, cameraSized;

    // Start is called before the first frame update
    void Start()
    {
        // �ړ��t���O�̐ݒ�
        cameraMoved = false;
        // �Y�[���A�E�g�t���O�̐ݒ�
        cameraSized = false;

        // �������W�̐ݒ�
        transform.position = new Vector3(transform.position.x, START_POS_Y, transform.position.z);
        // �����Y�[���T�C�Y�̐ݒ�
        gameObject.GetComponent<Camera>().orthographicSize = START_ZOOM_SIZE;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �ړ�
    public bool Moved()
    {
        // �ړ����I����Ă���Ȃ珈�������Ȃ�
        if (cameraMoved) return true;

        // �J�����̈ړ�
        transform.Translate(0, CAMERA_MOVE_SPEED, 0);
        // ���W�����ꂽ��ʒu�𒲐�����
        if (transform.position.y <= CAMERA_MOVED_POS_Y)
        {
            transform.position = new Vector3(transform.position.x, CAMERA_MOVED_POS_Y, transform.position.z);
            cameraMoved = true;
        }

        return false;
    }

    // �Y�[���A�E�g
    public bool ZoomedOut()
    {
        // �Y�[���A�E�g���I����Ă���Ȃ珈�������Ȃ�
        if (cameraSized) return true;

        // �J�����̃Y�[���l���擾
        float size = gameObject.GetComponent<Camera>().orthographicSize;

        // �J�����̃Y�[���A�E�g
        gameObject.GetComponent<Camera>().orthographicSize = size * CAMERA_ZOOM_OUT_NUM;

        // ���l�𒴂����ꍇ�A��������
        if (size >= CAMERA_ZOOM_SIZE)
        {
            GetComponent<Camera>().orthographicSize = CAMERA_ZOOM_SIZE;
            cameraSized = true;
        }

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedData : MonoBehaviour
{
    // �萔--------------------------------
    public const int TARGET_MAX_NUM = 10;
    const int STAGE_NUM = 5;

    // �ϐ�--------------------------------
    struct TargetData
    {
        public Vector3[] pos;      // ���W
        public int generateNum;    // �������鐔
    }

    // �e�X�e�[�W���ƃ`���[�g���A���X�e�[�W�̐������p�ӂ���
    static TargetData[] targetData = new TargetData[STAGE_NUM + 1];

    
    void Start()
    {
        // �`���[�g���A��
        targetData[0].pos[0] = new Vector3(0, 0, 0);
    }

}

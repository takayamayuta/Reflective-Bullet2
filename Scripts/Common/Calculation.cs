using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculation : MonoBehaviour
{
    // ���[���h���W��UI���[�J�����W�ɕϊ�����
    public static Vector2 WorldPosToUILocalPos(Vector2 _pos)
    {
        GameObject camera = GameObject.Find("Main Camera");
        GameObject canvas = GameObject.Find("Canvas");

        // ���[���h���W���X�N���[�����W�ɕϊ�
        Vector3 screenPos = camera.GetComponent<Camera>().WorldToScreenPoint(_pos);
        // RectTransform�̃��[�J�����W���󂯎��ϐ�
        Vector2 localPos = Vector2.zero;
        // �X�N���[�����W���烍�[�J��UI���W�ɕϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPos, null, out localPos);

        return localPos;
    }

    // �Q�_�Ԃ̊p�x���v�Z����
    public static float GetAngle(Vector2 start, Vector2 target)
    {
        Vector2 dt = target - start;
        float rad = Mathf.Atan2(dt.y, dt.x);
        float degree = rad * Mathf.Rad2Deg;
        return degree;
    }
}

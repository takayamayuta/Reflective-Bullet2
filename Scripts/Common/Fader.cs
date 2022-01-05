using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    // �萔--------------------------------
    const float SPEED = 0.01f;          // �����x���ς�鑬��
    const float TRANSPARENT = 0.0f;     // �������̒l
    const float OPACITY = 1.0f;         // �s�������̒l

    // �ϐ�--------------------------------
    Image image;
    float speed;    // �����x��ς��鑬�x

    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<Image>();
        image.color = Color.black;
        speed = SPEED;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Fading()
    {
        // �����x��ς���
        image.color = new Color(0, 0, 0, image.color.a - speed);

        // ���l�܂œ����x���ς������ȉ��̏���
        if (image.color.a <= TRANSPARENT || image.color.a >= OPACITY)
        {
            speed *= -1;    // �����x�̑�����؂�ւ���
            return true;
        }

        return false;
    }
}

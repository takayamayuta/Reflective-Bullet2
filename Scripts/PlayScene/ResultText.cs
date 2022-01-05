using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour
{
    // �萔--------------------------------
    // �n�_�I�_���WX
    const int START_END_POS_X = 2000;
    // �X�s�[�h���ς����W
    const int SPEED_CHANGE_POS = 300;
    // �����Ƃ��̑��x
    const int FAST_SPEED = 15;
    // �x���Ƃ��̑��x
    const int SLOW_SPEED = 6;

    // �ϐ�--------------------------------
    [SerializeField] GameObject sceneManager;
    // Text�R���|�[�l���g
    Text text;
    // TypefaceAnimator
    TypefaceAnimator typefaceanimator;
    // ���x
    float vel;
    // �A�j���[�V�����t���O
    bool textAnimation;
    

    // Start is called before the first frame update
    void Start()
    {
        // ���g��Text�����擾����
        text = gameObject.GetComponent<Text>();
        // ���g��TypefaceAnimator�����擾����
        typefaceanimator = gameObject.GetComponent<TypefaceAnimator>();
        // �A�j���[�V�������ĂȂ����
        textAnimation = true;

        // �N���A�t���O�ɉ����ăe�L�X�g���e��ύX����
        // ������
        if (sceneManager.GetComponent<PlaySceneManager>().GetClearFlag())
        {
            text.text = "CLEAR";
            text.color = Color.yellow;
        }
        // ���s��
        else
        {
            text.text = "FAILED";
            text.color = Color.blue;
        }

        // �������W�̐ݒ�
        gameObject.transform.localPosition = new Vector3(-START_END_POS_X, transform.localPosition.y, transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        // �����͈͊O�̎��͑����X�s�[�h�ňړ�
        if (transform.localPosition.x <= -SPEED_CHANGE_POS || transform.localPosition.x >= SPEED_CHANGE_POS)
        {
            transform.Translate(FAST_SPEED, 0, 0);
        }
        // �����͈͓��̏ꍇ�A�x���X�s�[�h�ňړ�
        else
        {
            transform.Translate(SLOW_SPEED, 0, 0);
            // �t���O��true�Ȃ�
            if (textAnimation)
            {
                // �A�j���[�V�����̏���
                typefaceanimator.Play();
                // �t���O��false�ɂ���
                textAnimation = false;
            }
        }

        // �����W�܂ňړ�������
        if (transform.localPosition.x >= START_END_POS_X)
        {
            // �������W�Ɉړ�
            transform.localPosition = new Vector3(-START_END_POS_X, transform.localPosition.y, transform.localPosition.z);
            // �A�j���[�V�����t���O��߂�
            textAnimation = true;
        }
    }
}

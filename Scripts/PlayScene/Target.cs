using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // �萔--------------------------------
    const float DEFAULT_ROT_Y = 180.0f;     // �����p�xY
    const float ADJ_ROT_Y = 35.0f;          // �v���C���[�̕����Ɍ������߂̊p�x�̒���
    const float WAIT_TIME = 0.5f;           // �A�j���[�V���������Ă���Ԃ̑ҋ@����
    const float SHRINK = 0.9f;              // ���k�l
    const float DELETE_SCALE_SIZE = 0.1f;   // ���ȉ��̑傫���ō폜����
    const float EFFECT_SCALE = 3.0f;        // ���S�G�t�F�N�g�̑傫��

    // �ϐ�--------------------------------
    [SerializeField] GameObject deadEffect; // ���S�G�t�F�N�g

    Animator animator;                      // Animator�R���|�[�l���g�����i�[����
    float timer;                            // �ҋ@���Ԃ��v������^�C�}�[
    bool hit;                               // �e���������������肷��

    // Start is called before the first frame update
    void Start()
    {
        // ���g��Animator�R���|�[�l���g�̏����擾
        animator = gameObject.GetComponent<Animator>();
        // �^�C�}�[���Z�b�g
        timer = 0;
        // �e���������Ă��Ȃ����
        hit = false;

        // �p�x�ϐ��𐶐�
        float rot = DEFAULT_ROT_Y;
        // ���W�ɉ����Ċp�x��ύX����
        if (transform.position.x > 0) rot = DEFAULT_ROT_Y + ADJ_ROT_Y;
        else if (transform.position.x < 0) rot = DEFAULT_ROT_Y - ADJ_ROT_Y;

        // �v���C���[�̕����������悤�ɕύX�����p�x�𔽉f������
        transform.rotation = Quaternion.Euler(transform.rotation.x, rot, transform.rotation.z);
    }

    // Update is called once per frame
    void Update()
    {
        Dead();
    }

    // ���񂾂Ƃ��̏���
    void Dead()
    {
        // �e���������Ă��Ȃ��Ȃ珈�����΂�
        if (!hit) return;

        // �^�C�}�[�̃J�E���g
        timer += Time.deltaTime;
        // ��莞�Ԍo�߂���܂ŏ������΂�
        if (timer < WAIT_TIME) return;

        // ���g������������
        transform.localScale = new Vector3(transform.localScale.x * SHRINK, transform.localScale.y * SHRINK, transform.localScale.z * SHRINK);

        // ���ȉ��̑傫���ɂȂ�����ȉ��̏������s��
        if (transform.localScale.x <= DELETE_SCALE_SIZE)
        {
            // ���g��j�󂷂�
            Destroy(gameObject);
            // ���S�G�t�F�N�g�𔭐�������
            Effect.EffectAdd(Calculation.WorldPosToUILocalPos(transform.position), "Darkness_7_Effect", "Effects", new Vector3(EFFECT_SCALE, EFFECT_SCALE, EFFECT_SCALE));
        }
    }

    public void GetHit()
    {
        // die�A�j���[�V�������J�n
        animator.SetBool("die", true);
        // hit�t���O�𗧂Ă�
        hit = true;
    }
}

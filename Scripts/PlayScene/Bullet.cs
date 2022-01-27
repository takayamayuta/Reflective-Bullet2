using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // �萔--------------------------------
    // �������Ԃ̏����l
    const float DEFAULT_ALIVE_TIME = 5.0f;
    // �^�[�Q�b�g�̐��ɉ����Đ������Ԃ𑝂₷�l
    const float ADD_ALIVE_TIME = 2.0f;
    // �^�[�Q�b�g�ɓ����������ɕ\������G�t�F�N�g�傫��
    const float HIT_TARGET_EFFECT_SCALE = 3.0f;
    // �u���b�N�ɓ����������ɕ\������G�t�F�N�g�傫��
    const float HIT_BLOCK_EFFECT_SCALE = 3.0f;

    // �ϐ�--------------------------------
    // Renderer�R���|�[�l���g�̏��
    Renderer renderer;
    // �������Ԃ��v������^�C�}�[
    float aliveTimer;
    // �V�[���}�l�[�W���[�A�J�����A�L�����o�X
    GameObject sceneManager, camera, canvas;
    // �G�t�F�N�g�\�����W�ƃI�u�W�F�N�g���m���ڐG�������W
    Vector2 hitPos, effectPos;
    // �^�[�Q�b�g�Ɠ�����������SE�A�u���b�N�Ɠ�����������SE
    [SerializeField] AudioClip hitTargetSE, hitBlockSE;
    // ��������
    float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        // �e�I�u�W�F�N�g��������
        sceneManager = GameObject.Find("SceneManager");
        camera = GameObject.Find("Main Camera");
        canvas = GameObject.Find("Canvas");

        // ���g��Renderer�����擾����
        renderer = gameObject.GetComponent<Renderer>();
        // ���g�������ł��鎞�Ԃ����Z�b�g����
        aliveTimer = 0;
        // ��������
        waitTime = DEFAULT_ALIVE_TIME + sceneManager.GetComponent<PlaySceneManager>().GetTargetNum() * ADD_ALIVE_TIME;
    }

    // Update is called once per frame
    void Update()
    {
        // �^�C�}�[�̌v��
        aliveTimer += Time.deltaTime;
        // ��ʊO�ɂ������A�܂��͈�莞�Ԍo�߂����A���U���g��ԂɈڍs�����ꍇ�A�ȉ��̏������s��
        if (!renderer.isVisible || aliveTimer >= waitTime || sceneManager.GetComponent<PlaySceneManager>().GetState() == PlaySceneManager.eSTATE.RESULT)
        {
            // ���g��j�󂷂�
            Destroy(gameObject);
            // ���g���j�󂳂ꂽ���Ƃ��V�[���}�l�[�W���[�ɓ`����
            sceneManager.GetComponent<PlaySceneManager>().BulletLost();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �u���b�N�Ɠ��������ꍇ
        if (collision.transform.tag == "Block")
        {
            // �u���b�N�ɓ�����������SE�𗬂�
            GetComponent<AudioSource>().PlayOneShot(hitBlockSE);
            // �u���b�N�Ɠ��������ʒu�̍��W���擾
            foreach (ContactPoint2D point in collision.contacts)
            {
                hitPos = point.point;
            }
            // �G�t�F�N�g�̕\��������W�̐ݒ�
            effectPos = Calculation.WorldPosToUILocalPos(hitPos);
            // �G�t�F�N�g�̕\��
            Effect.EffectAdd(effectPos, "Darkness_5_Effect", "Effects", new Vector3(HIT_BLOCK_EFFECT_SCALE, HIT_BLOCK_EFFECT_SCALE, HIT_BLOCK_EFFECT_SCALE));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �^�[�Q�b�g�Ɠ��������ꍇ
        if (collision.transform.tag == "Target")
        {
            // �u���b�N�ɓ�����������SE�𗬂�
            GetComponent<AudioSource>().PlayOneShot(hitBlockSE);
            // ���������I�u�W�F�N�g��Hit�������s��
            collision.GetComponent<Target>().GetHit();
            // �Փ˂������W���擾
            hitPos = collision.ClosestPoint(transform.position);
            // ���[���h���W��UI���[�J�����W�ɕϊ�����
            effectPos = Calculation.WorldPosToUILocalPos(hitPos);
            // �G�t�F�N�g�̕\��
            Effect.EffectAdd(effectPos, "Impact_6_Effect", "Effects", new Vector3(HIT_TARGET_EFFECT_SCALE, HIT_TARGET_EFFECT_SCALE, HIT_TARGET_EFFECT_SCALE));
        }
    }
}

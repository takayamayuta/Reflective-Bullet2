using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // �萔--------------------------------
    // ���g�������ł��鎞��
    const float ALIVE_TIME = 10.0f;
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

    // Start is called before the first frame update
    void Start()
    {
        // ���g��Renderer�����擾����
        renderer = gameObject.GetComponent<Renderer>();
        // ���g�������ł��鎞�Ԃ����Z�b�g����
        aliveTimer = 0;
        // �e�I�u�W�F�N�g��������
        sceneManager = GameObject.Find("SceneManager");
        camera = GameObject.Find("Main Camera");
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        // �^�C�}�[�̌v��
        aliveTimer += Time.deltaTime;
        // ��ʊO�ɂ������A�܂��͈�莞�Ԍo�߂����A���U���g��ԂɈڍs�����ꍇ�A�ȉ��̏������s��
        if (!renderer.isVisible || aliveTimer >= ALIVE_TIME || sceneManager.GetComponent<PlaySceneManager>().GetState() == PlaySceneManager.eSTATE.RESULT)
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
            effectPos = WorldPosToUILocalPos(hitPos);
            // �G�t�F�N�g�̕\��
            Effect.EffectAdd(effectPos, "Darkness_7_Effect", "Canvas", new Vector3(HIT_BLOCK_EFFECT_SCALE, HIT_BLOCK_EFFECT_SCALE, HIT_BLOCK_EFFECT_SCALE));
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
            effectPos = WorldPosToUILocalPos(hitPos);
            // �G�t�F�N�g�̕\��
            Effect.EffectAdd(effectPos, "Impact_6_Effect", "Canvas", new Vector3(HIT_TARGET_EFFECT_SCALE, HIT_TARGET_EFFECT_SCALE, HIT_TARGET_EFFECT_SCALE));
        }
    }

    // ���[���h���W��UI���[�J�����W�ɕϊ�����
    Vector2 WorldPosToUILocalPos(Vector2 hitPos)
    {
        // ���[���h���W���X�N���[�����W�ɕϊ�
        Vector3 screenPos = camera.GetComponent<Camera>().WorldToScreenPoint(hitPos);
        // RectTransform�̃��[�J�����W���󂯎��ϐ�
        Vector2 localPos = Vector2.zero;
        // �X�N���[�����W���烍�[�J��UI���W�ɕϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPos, null, out localPos);

        return localPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // �萔--------------------------------
    const float BULLET_DIS = 1.0f;

    // �ϐ�--------------------------------
    // �e �T�C�g
    [SerializeField] GameObject gun, site;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // �}�E�X���{�^���������ꂽ��
        if (Input.GetMouseButtonDown(0))
        {
            // �T�C�g��\������
            site.SetActive(true);
        }

        // �}�E�X���{�^���������ꑱ���Ă���Ȃ�
        if (Input.GetMouseButton(0))
        {
            // �N���b�N���Ă�����W�̎擾
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // �T�C�g�̍��W��ύX����
            site.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);

            // ���g�ƃT�C�g�̊p�x���v�Z����
            float angle = GetAngle(transform.position, mouseWorldPos);
            float rad = angle * Mathf.Deg2Rad;
            // ���W�A������i�s�������Z�o
            Vector3 direction = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
            // �ړ��x�N�g��
            Vector3 vec = direction * BULLET_DIS;
            gun.transform.position = transform.position + vec;

            Debug.Log(angle);
        }

        // �}�E�X���{�^����������Ȃ��Ȃ�����
        if (Input.GetMouseButtonUp(0))
        {
            // �e�𔭎˂���
            gun.GetComponent<ShotBullet>().Shot();
            // �T�C�g���\���ɂ���
            site.SetActive(false);
        }
    }

    // �Q�_�Ԃ̊p�x���v�Z����
    float GetAngle(Vector2 start, Vector2 target)
    {
        Vector2 dt = target - start;
        float rad = Mathf.Atan2(dt.y, dt.x);
        float degree = rad * Mathf.Rad2Deg;
        return degree;
    }
}

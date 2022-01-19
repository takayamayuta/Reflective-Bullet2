using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBullet : MonoBehaviour
{
    // �萔--------------------------------
    const float BULLET_SPEED = 10.0f;

    // �ϐ�--------------------------------
    // ���˂���e
    [SerializeField] GameObject bulletPrefab;

    // ���˂���������
    bool shot;                                  

    // Start is called before the first frame update
    void Start()
    {
        // �����ˏ��
        shot = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shot()
    {
        // ��̃I�u�W�F�N�g�𐶐�
        GameObject go = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        // �N���b�N�������W�̎擾
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // �����̐���
        Vector3 shotForward = Vector3.Scale((mouseWorldPos - transform.position), new Vector3(1, 1, 0)).normalized;
        // �e�ɑ��x��^����
        go.GetComponent<Rigidbody2D>().velocity = shotForward * BULLET_SPEED;
        // ���ˍςɂ���
        shot = true;
    }

    public bool GetShot()
    {
        return shot;
    }
}

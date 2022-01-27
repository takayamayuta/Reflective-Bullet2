using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // �萔--------------------------------
    const float GUN_DIS = 1.0f;                                             // �e�ƃv���C���[�̋���
    const float INVERSION = 180.0f;                                         // �e�𔻒肳����p�x
    static readonly Vector3 PLAYER_DEFAULT_POS = new Vector3(0, -1.0f, 0);  // �v���C���[�̍��W����
    static readonly Vector3 GUN_ADJ_POS = new Vector3(0, 1.0f, 0);          // �v���C���[�Əe�̋���

    // �ϐ�--------------------------------
    // �e �T�C�g
    [SerializeField] GameObject gun, site, sceneManager, pauseText;
    [SerializeField] AudioClip gunReadySE, gunShotSE;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (sceneManager.GetComponent<PlaySceneManager>().GetState() != PlaySceneManager.eSTATE.PLAY || gun.GetComponent<ShotBullet>().GetShot()) return;
        
        // �T�C�g��\������
        site.SetActive(true);
        // �N���b�N���Ă�����W�̎擾
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // �T�C�g�̍��W��ύX����
        site.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, site.transform.position.z);
        // ���g�ƃT�C�g�̊p�x���v�Z����
        float angle = Calculation.GetAngle(PLAYER_DEFAULT_POS, mouseWorldPos);
        float rad = angle * Mathf.Deg2Rad;

        if (angle > 90 || angle < -90) transform.rotation = Quaternion.Euler(0, 180, 0);
        else transform.rotation = Quaternion.identity;

        // �e�̍��W�̒���
        GunPositionUpdate(rad);
        // �e�̊p�x�̒���
        GunAngleUpdate(angle);


        // �}�E�X���{�^���������ꂽ��A����
        if (Input.GetMouseButtonDown(0) && !pauseText.activeSelf)
        {
            // �e�𔭎˂���
            gun.GetComponent<ShotBullet>().Shot();
            // �T�C�g���\���ɂ���
            site.SetActive(false);
            // �e�����������𗬂�
            GetComponent<AudioSource>().PlayOneShot(gunShotSE);
        }
    }

    void GunAngleUpdate(float _angle)
    {
        // �_�������ɏe�ƌX����悤�ɂ���
        gun.transform.rotation = Quaternion.Euler(0, 0, _angle);
        // ���ȏ�ȉ��̊p�x�̏ꍇ�e�𔽓]�����Č����𐳂�������
        if (_angle >= 90 || _angle <= -90) gun.transform.rotation = Quaternion.Euler(INVERSION, 0, -_angle);
    }

    void GunPositionUpdate(float _rad)
    {
        // ���W�A������i�s�������Z�o
        Vector3 direction = new Vector3(Mathf.Cos(_rad), Mathf.Sin(_rad), 0);
        // �ړ��x�N�g��
        Vector3 vec = direction * GUN_DIS;
        // �ړ��x�N�g�����A�e�ƃv���C���[�̋����𗣂�
        gun.transform.position = transform.position + vec + GUN_ADJ_POS;
    }
}

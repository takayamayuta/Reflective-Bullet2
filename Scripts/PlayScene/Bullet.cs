using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // �萔--------------------------------
    const float ALIVE_TIME = 10.0f;

    // �ϐ�--------------------------------
    // Renderer�R���|�[�l���g�̏��
    Renderer renderer;
    // �������Ԃ��v������^�C�}�[
    float aliveTimer;
    // �V�[���}�l�[�W���[
    GameObject sceneManager;

    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        aliveTimer = 0;
        sceneManager = GameObject.Find("SceneManager");
    }

    // Update is called once per frame
    void Update()
    {
        // �^�C�}�[�̌v��
        aliveTimer += Time.deltaTime;
        // ��ʊO�ɂ������A�܂��͈�莞�Ԍo�߂����玩�g��j�󂷂�
        if (!renderer.isVisible || aliveTimer >= ALIVE_TIME)
        {
            Destroy(gameObject);
            sceneManager.GetComponent<PlaySceneManager>().BulletLost();
        }
    }
}

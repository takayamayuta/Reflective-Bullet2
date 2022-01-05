using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 定数--------------------------------
    const float ALIVE_TIME = 10.0f;

    // 変数--------------------------------
    // Rendererコンポーネントの情報
    Renderer renderer;
    // 生存時間を計測するタイマー
    float aliveTimer;
    // シーンマネージャー
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
        // タイマーの計測
        aliveTimer += Time.deltaTime;
        // 画面外にいった、または一定時間経過したら自身を破壊する
        if (!renderer.isVisible || aliveTimer >= ALIVE_TIME)
        {
            Destroy(gameObject);
            sceneManager.GetComponent<PlaySceneManager>().BulletLost();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 定数--------------------------------
    // 自身が生存できる時間
    const float ALIVE_TIME = 10.0f;
    // ターゲットに当たった時に表示するエフェクト大きさ
    const float HIT_TARGET_EFFECT_SCALE = 3.0f;
    // ブロックに当たった時に表示するエフェクト大きさ
    const float HIT_BLOCK_EFFECT_SCALE = 3.0f;

    // 変数--------------------------------
    // Rendererコンポーネントの情報
    Renderer renderer;
    // 生存時間を計測するタイマー
    float aliveTimer;
    // シーンマネージャー、カメラ、キャンバス
    GameObject sceneManager, camera, canvas;
    // エフェクト表示座標とオブジェクト同士が接触した座標
    Vector2 hitPos, effectPos;
    // ターゲットと当たった時のSE、ブロックと当たった時のSE
    [SerializeField] AudioClip hitTargetSE, hitBlockSE;

    // Start is called before the first frame update
    void Start()
    {
        // 自身のRenderer情報を取得する
        renderer = gameObject.GetComponent<Renderer>();
        // 自身が生存できる時間をリセットする
        aliveTimer = 0;
        // 各オブジェクトを見つける
        sceneManager = GameObject.Find("SceneManager");
        camera = GameObject.Find("Main Camera");
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        // タイマーの計測
        aliveTimer += Time.deltaTime;
        // 画面外にいった、または一定時間経過した、リザルト状態に移行した場合、以下の処理を行う
        if (!renderer.isVisible || aliveTimer >= ALIVE_TIME || sceneManager.GetComponent<PlaySceneManager>().GetState() == PlaySceneManager.eSTATE.RESULT)
        {
            // 自身を破壊する
            Destroy(gameObject);
            // 自身が破壊されたことをシーンマネージャーに伝える
            sceneManager.GetComponent<PlaySceneManager>().BulletLost();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ブロックと当たった場合
        if (collision.transform.tag == "Block")
        {
            // ブロックに当たった時のSEを流す
            GetComponent<AudioSource>().PlayOneShot(hitBlockSE);
            // ブロックと当たった位置の座標を取得
            foreach (ContactPoint2D point in collision.contacts)
            {
                hitPos = point.point;
            }
            // エフェクトの表示する座標の設定
            effectPos = WorldPosToUILocalPos(hitPos);
            // エフェクトの表示
            Effect.EffectAdd(effectPos, "Darkness_7_Effect", "Canvas", new Vector3(HIT_BLOCK_EFFECT_SCALE, HIT_BLOCK_EFFECT_SCALE, HIT_BLOCK_EFFECT_SCALE));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ターゲットと当たった場合
        if (collision.transform.tag == "Target")
        {
            // ブロックに当たった時のSEを流す
            GetComponent<AudioSource>().PlayOneShot(hitBlockSE);
            // 当たったオブジェクトのHit処理を行う
            collision.GetComponent<Target>().GetHit();
            // 衝突した座標を取得
            hitPos = collision.ClosestPoint(transform.position);
            // ワールド座標をUIローカル座標に変換する
            effectPos = WorldPosToUILocalPos(hitPos);
            // エフェクトの表示
            Effect.EffectAdd(effectPos, "Impact_6_Effect", "Canvas", new Vector3(HIT_TARGET_EFFECT_SCALE, HIT_TARGET_EFFECT_SCALE, HIT_TARGET_EFFECT_SCALE));
        }
    }

    // ワールド座標をUIローカル座標に変換する
    Vector2 WorldPosToUILocalPos(Vector2 hitPos)
    {
        // ワールド座標をスクリーン座標に変換
        Vector3 screenPos = camera.GetComponent<Camera>().WorldToScreenPoint(hitPos);
        // RectTransformのローカル座標を受け取る変数
        Vector2 localPos = Vector2.zero;
        // スクリーン座標からローカルUI座標に変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPos, null, out localPos);

        return localPos;
    }
}

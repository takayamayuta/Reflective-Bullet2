using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // 定数--------------------------------
    const float DEFAULT_ROT_Y = 180.0f;     // 初期角度Y
    const float ADJ_ROT_Y = 35.0f;          // プレイヤーの方向に向くための角度の調整
    const float WAIT_TIME = 0.5f;           // アニメーションをしている間の待機時間
    const float SHRINK = 0.9f;              // 収縮値
    const float DELETE_SCALE_SIZE = 0.1f;   // 一定以下の大きさで削除する
    const float EFFECT_SCALE = 3.0f;        // 死亡エフェクトの大きさ

    // 変数--------------------------------
    [SerializeField] GameObject deadEffect; // 死亡エフェクト

    Animator animator;                      // Animatorコンポーネント情報を格納する
    float timer;                            // 待機時間を計測するタイマー
    bool hit;                               // 弾が当たったか判定する

    // Start is called before the first frame update
    void Start()
    {
        // 自身のAnimatorコンポーネントの情報を取得
        animator = gameObject.GetComponent<Animator>();
        // タイマーリセット
        timer = 0;
        // 弾が当たっていない状態
        hit = false;

        // 角度変数を生成
        float rot = DEFAULT_ROT_Y;
        // 座標に応じて角度を変更する
        if (transform.position.x > 0) rot = DEFAULT_ROT_Y + ADJ_ROT_Y;
        else if (transform.position.x < 0) rot = DEFAULT_ROT_Y - ADJ_ROT_Y;

        // プレイヤーの方向を向くように変更した角度を反映させる
        transform.rotation = Quaternion.Euler(transform.rotation.x, rot, transform.rotation.z);
    }

    // Update is called once per frame
    void Update()
    {
        Dead();
    }

    // 死んだときの処理
    void Dead()
    {
        // 弾が当たっていないなら処理を飛ばす
        if (!hit) return;

        // タイマーのカウント
        timer += Time.deltaTime;
        // 一定時間経過するまで処理を飛ばす
        if (timer < WAIT_TIME) return;

        // 自身を小さくする
        transform.localScale = new Vector3(transform.localScale.x * SHRINK, transform.localScale.y * SHRINK, transform.localScale.z * SHRINK);

        // 一定以下の大きさになったら以下の処理を行う
        if (transform.localScale.x <= DELETE_SCALE_SIZE)
        {
            // 自身を破壊する
            Destroy(gameObject);
            // 死亡エフェクトを発生させる
            Effect.EffectAdd(Calculation.WorldPosToUILocalPos(transform.position), "Darkness_7_Effect", "Effects", new Vector3(EFFECT_SCALE, EFFECT_SCALE, EFFECT_SCALE));
        }
    }

    public void GetHit()
    {
        // dieアニメーションを開始
        animator.SetBool("die", true);
        // hitフラグを立てる
        hit = true;
    }
}

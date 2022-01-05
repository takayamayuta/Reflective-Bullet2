using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour
{
    // 定数--------------------------------
    // 始点終点座標X
    const int START_END_POS_X = 2000;
    // スピードが変わる座標
    const int SPEED_CHANGE_POS = 300;
    // 速いときの速度
    const int FAST_SPEED = 15;
    // 遅いときの速度
    const int SLOW_SPEED = 6;

    // 変数--------------------------------
    [SerializeField] GameObject sceneManager;
    // Textコンポーネント
    Text text;
    // TypefaceAnimator
    TypefaceAnimator typefaceanimator;
    // 速度
    float vel;
    // アニメーションフラグ
    bool textAnimation;
    

    // Start is called before the first frame update
    void Start()
    {
        // 自身のText情報を取得する
        text = gameObject.GetComponent<Text>();
        // 自身のTypefaceAnimator情報を取得する
        typefaceanimator = gameObject.GetComponent<TypefaceAnimator>();
        // アニメーションしてない状態
        textAnimation = true;

        // クリアフラグに応じてテキスト内容を変更する
        // 成功時
        if (sceneManager.GetComponent<PlaySceneManager>().GetClearFlag())
        {
            text.text = "CLEAR";
            text.color = Color.yellow;
        }
        // 失敗時
        else
        {
            text.text = "FAILED";
            text.color = Color.blue;
        }

        // 初期座標の設定
        gameObject.transform.localPosition = new Vector3(-START_END_POS_X, transform.localPosition.y, transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        // 減速範囲外の時は速いスピードで移動
        if (transform.localPosition.x <= -SPEED_CHANGE_POS || transform.localPosition.x >= SPEED_CHANGE_POS)
        {
            transform.Translate(FAST_SPEED, 0, 0);
        }
        // 減速範囲内の場合、遅いスピードで移動
        else
        {
            transform.Translate(SLOW_SPEED, 0, 0);
            // フラグがtrueなら
            if (textAnimation)
            {
                // アニメーションの処理
                typefaceanimator.Play();
                // フラグをfalseにする
                textAnimation = false;
            }
        }

        // 一定座標まで移動したら
        if (transform.localPosition.x >= START_END_POS_X)
        {
            // 初期座標に移動
            transform.localPosition = new Vector3(-START_END_POS_X, transform.localPosition.y, transform.localPosition.z);
            // アニメーションフラグを戻す
            textAnimation = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    // 定数--------------------------------
    const float SPEED = 0.01f;          // 透明度が変わる速さ
    const float TRANSPARENT = 0.0f;     // 透明時の値
    const float OPACITY = 1.0f;         // 不透明時の値

    // 変数--------------------------------
    Image image;
    float speed;    // 透明度を変える速度

    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<Image>();
        image.color = Color.black;
        speed = SPEED;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Fading()
    {
        // 透明度を変える
        image.color = new Color(0, 0, 0, image.color.a - speed);

        // 一定値まで透明度が変わったら以下の処理
        if (image.color.a <= TRANSPARENT || image.color.a >= OPACITY)
        {
            speed *= -1;    // 透明度の増減を切り替える
            return true;
        }

        return false;
    }
}

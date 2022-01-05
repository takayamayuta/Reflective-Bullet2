using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 定数--------------------------------
    const float BULLET_DIS = 1.0f;

    // 変数--------------------------------
    // 銃 サイト
    [SerializeField] GameObject gun, site;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // マウス左ボタンが押されたら
        if (Input.GetMouseButtonDown(0))
        {
            // サイトを表示する
            site.SetActive(true);
        }

        // マウス左ボタンが押され続けているなら
        if (Input.GetMouseButton(0))
        {
            // クリックしている座標の取得
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // サイトの座標を変更する
            site.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);

            // 自身とサイトの角度を計算する
            float angle = GetAngle(transform.position, mouseWorldPos);
            float rad = angle * Mathf.Deg2Rad;
            // ラジアンから進行方向を算出
            Vector3 direction = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
            // 移動ベクトル
            Vector3 vec = direction * BULLET_DIS;
            gun.transform.position = transform.position + vec;

            Debug.Log(angle);
        }

        // マウス左ボタンが押されなくなったら
        if (Input.GetMouseButtonUp(0))
        {
            // 弾を発射する
            gun.GetComponent<ShotBullet>().Shot();
            // サイトを非表示にする
            site.SetActive(false);
        }
    }

    // ２点間の角度を計算する
    float GetAngle(Vector2 start, Vector2 target)
    {
        Vector2 dt = target - start;
        float rad = Mathf.Atan2(dt.y, dt.x);
        float degree = rad * Mathf.Rad2Deg;
        return degree;
    }
}

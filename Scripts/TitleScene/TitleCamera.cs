using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCamera : MonoBehaviour
{
    // 定数--------------------------------
    // カメラのズームアウトしていく倍率
    const float CAMERA_ZOOM_OUT_NUM = 1.1f;
    // カメラのズームアウトし続けるまでの値
    const float CAMERA_ZOOM_SIZE = 5.0f;
    // カメラの移動先
    const float CAMERA_MOVED_POS_Y = 0;
    // カメラの移動スピード
    const float CAMERA_MOVE_SPEED = 0.1f;
    // 初期Y座標
    const float START_POS_Y = -0.25f;
    // 初期ズームサイズ
    const float START_ZOOM_SIZE = 0.3f;

    // 変数--------------------------------
    bool cameraMoved, cameraSized;

    // Start is called before the first frame update
    void Start()
    {
        // 移動フラグの設定
        cameraMoved = false;
        // ズームアウトフラグの設定
        cameraSized = false;

        // 初期座標の設定
        transform.position = new Vector3(transform.position.x, START_POS_Y, transform.position.z);
        // 初期ズームサイズの設定
        gameObject.GetComponent<Camera>().orthographicSize = START_ZOOM_SIZE;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 移動
    public bool Moved()
    {
        // 移動し終わっているなら処理をしない
        if (cameraMoved) return true;

        // カメラの移動
        transform.Translate(0, CAMERA_MOVE_SPEED, 0);
        // 座標がずれたら位置を調整する
        if (transform.position.y <= CAMERA_MOVED_POS_Y)
        {
            transform.position = new Vector3(transform.position.x, CAMERA_MOVED_POS_Y, transform.position.z);
            cameraMoved = true;
        }

        return false;
    }

    // ズームアウト
    public bool ZoomedOut()
    {
        // ズームアウトし終わっているなら処理をしない
        if (cameraSized) return true;

        // カメラのズーム値を取得
        float size = gameObject.GetComponent<Camera>().orthographicSize;

        // カメラのズームアウト
        gameObject.GetComponent<Camera>().orthographicSize = size * CAMERA_ZOOM_OUT_NUM;

        // 一定値を超えた場合、調整する
        if (size >= CAMERA_ZOOM_SIZE)
        {
            GetComponent<Camera>().orthographicSize = CAMERA_ZOOM_SIZE;
            cameraSized = true;
        }

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCameraMove : MonoBehaviour
{
    // 定数--------------------------------
    enum eSTATE
    {
        NONE = -1,

        WAIT,           // 待機
        ACCELERATION,   // 加速
        DECELERATE      // 減速
    }

    const float ACCELE = 1.1f;          // 加速度
    const float DECELE = 0.8f;          // 減速度
    const float POS_DIFFERENCE = 18.0f; // ステージ間の座標の差
    const float START_SPEED = 0.5f;     // 最初の速度
    const float CHANGE_STATE_POS = 5.0f;// 選択されたステージの座標から指定された値以内に近づいたら減速状態に切り替える
    const float CAMERA_POS_Z = -10.0f;  // カメラのZ座標

    // 変数--------------------------------
    // シーンマネージャー
    [SerializeField] GameObject sceneManager;

    // 選択していたステージの座標
    Vector3 selectedPos;

    // 移動速度
    float vel;

    // 状態
    eSTATE state;                              

    // Start is called before the first frame update
    void Start()
    {
        selectedPos = Vector3.zero;     // 座標をリセット
        vel = START_SPEED;              // 移動速度の初期化
        state = eSTATE.WAIT;            // 待機状態
    }

    // Update is called once per frame
    void Update()
    {
        // 状態毎の処理を行う
        switch (state)
        {
            // 待機
            case eSTATE.WAIT: Wait(); break;
            // 加速
            case eSTATE.ACCELERATION: Acceleration(); break;
            // 減速
            case eSTATE.DECELERATE: Decelerate(); break;
        }
    }

    // 待機
    void Wait()
    {
        // 移動状態なら加速状態に移行
        if (sceneManager.GetComponent<StageSelectSceneManager>().GetMoving())
        {
            state = eSTATE.ACCELERATION;
            vel = START_SPEED;
        }
    }

    // 加速
    void Acceleration()
    {
        // 移動速度の上昇
        vel *= ACCELE;

        // 目的地のX座標
        float destination = (int)sceneManager.GetComponent<StageSelectSceneManager>().GetSelectStage() * POS_DIFFERENCE;

        // 選択したステージより右にカメラがあるなら左に移動させる
        if (transform.position.x > destination)
        {
            // 左に移動
            gameObject.transform.Translate(-vel, 0, 0);

            // 一定位置まで移動したら減速状態に移行
            if (transform.position.x <= destination + CHANGE_STATE_POS)
            {
                state = eSTATE.DECELERATE;
            }
        }
        // 選択したステージより左にカメラがあるなら右に移動させる
        else if (transform.position.x < destination)
        {
            // 右に移動
            gameObject.transform.Translate(vel, 0, 0);

            // 一定位置まで移動したら減速状態に移行
            if (transform.position.x >= destination - CHANGE_STATE_POS)
            {
                state = eSTATE.DECELERATE;
            }
        }
    }

    // 減速
    void Decelerate()
    {
        // 移動速度の上昇
        vel *= DECELE;

        // 目的地のX座標
        float destination = (int)sceneManager.GetComponent<StageSelectSceneManager>().GetSelectStage() * POS_DIFFERENCE;  

        // 選択したステージより右にカメラがあるなら左に移動させる
        if (transform.position.x > destination)
        {
            // 選択してるステージが変わって目的地が変更されたら状態を加速状態に戻す
            if (transform.position.x > destination + CHANGE_STATE_POS) state = eSTATE.ACCELERATION;

            // 左に移動
            gameObject.transform.Translate(-vel, 0, 0);

            // 一定位置まで移動したら座標を調節して待機状態に移行
            if (transform.position.x <= destination)
            {
                // 座標の調整
                transform.position = new Vector3(destination, 0, CAMERA_POS_Z);
                // 待機状態に移行
                state = eSTATE.WAIT;
                // 移動済にする
                sceneManager.GetComponent<StageSelectSceneManager>().Moved();
            }
        }
        // 選択したステージより左にカメラがあるなら右に移動させる
        else if (transform.position.x < destination)
        {
            // 選択してるステージが変わって目的地が変更されたら状態を加速状態に戻す
            if (transform.position.x < destination - CHANGE_STATE_POS) state = eSTATE.ACCELERATION;

            // 右に移動
            gameObject.transform.Translate(vel, 0, 0);

            // 一定位置まで移動したら座標を調節して待機状態に移行
            if (transform.position.x >= destination)
            {
                // 座標の調整
                transform.position = new Vector3(destination, 0, CAMERA_POS_Z);
                // 待機状態に移行
                state = eSTATE.WAIT;
                // 移動済にする
                sceneManager.GetComponent<StageSelectSceneManager>().Moved();
            }
        }
    }
}

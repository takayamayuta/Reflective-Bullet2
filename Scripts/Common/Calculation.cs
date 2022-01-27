using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculation : MonoBehaviour
{
    // ワールド座標をUIローカル座標に変換する
    public static Vector2 WorldPosToUILocalPos(Vector2 _pos)
    {
        GameObject camera = GameObject.Find("Main Camera");
        GameObject canvas = GameObject.Find("Canvas");

        // ワールド座標をスクリーン座標に変換
        Vector3 screenPos = camera.GetComponent<Camera>().WorldToScreenPoint(_pos);
        // RectTransformのローカル座標を受け取る変数
        Vector2 localPos = Vector2.zero;
        // スクリーン座標からローカルUI座標に変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPos, null, out localPos);

        return localPos;
    }

    // ２点間の角度を計算する
    public static float GetAngle(Vector2 start, Vector2 target)
    {
        Vector2 dt = target - start;
        float rad = Mathf.Atan2(dt.y, dt.x);
        float degree = rad * Mathf.Rad2Deg;
        return degree;
    }
}

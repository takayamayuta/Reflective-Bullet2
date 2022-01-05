using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedData : MonoBehaviour
{
    // 定数--------------------------------
    public const int TARGET_MAX_NUM = 10;
    const int STAGE_NUM = 5;

    // 変数--------------------------------
    struct TargetData
    {
        public Vector3[] pos;      // 座標
        public int generateNum;    // 生成する数
    }

    // 各ステージ分とチュートリアルステージの数だけ用意する
    static TargetData[] targetData = new TargetData[STAGE_NUM + 1];

    
    void Start()
    {
        // チュートリアル
        targetData[0].pos[0] = new Vector3(0, 0, 0);
    }

}

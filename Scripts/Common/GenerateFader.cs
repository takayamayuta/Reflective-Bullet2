using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFader : MonoBehaviour
{
    // 生成するオブジェクト
    [SerializeField] GameObject fader;

    // オブジェクトの生成
    public GameObject Generate()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject go = Instantiate(fader, canvas.transform.position, Quaternion.identity, canvas.transform);
        return go;
    }
}

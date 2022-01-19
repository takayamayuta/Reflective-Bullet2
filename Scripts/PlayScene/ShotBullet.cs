using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBullet : MonoBehaviour
{
    // 定数--------------------------------
    const float BULLET_SPEED = 10.0f;

    // 変数--------------------------------
    // 発射する弾
    [SerializeField] GameObject bulletPrefab;

    // 発射したか判定
    bool shot;                                  

    // Start is called before the first frame update
    void Start()
    {
        // 未発射状態
        shot = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shot()
    {
        // 空のオブジェクトを生成
        GameObject go = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        // クリックした座標の取得
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 向きの生成
        Vector3 shotForward = Vector3.Scale((mouseWorldPos - transform.position), new Vector3(1, 1, 0)).normalized;
        // 弾に速度を与える
        go.GetComponent<Rigidbody2D>().velocity = shotForward * BULLET_SPEED;
        // 発射済にする
        shot = true;
    }

    public bool GetShot()
    {
        return shot;
    }
}

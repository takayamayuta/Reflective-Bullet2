using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 定数--------------------------------
    const float GUN_DIS = 1.0f;                                             // 銃とプレイヤーの距離
    const float INVERSION = 180.0f;                                         // 銃を判定させる角度
    static readonly Vector3 PLAYER_DEFAULT_POS = new Vector3(0, -1.0f, 0);  // プレイヤーの座標調整
    static readonly Vector3 GUN_ADJ_POS = new Vector3(0, 1.0f, 0);          // プレイヤーと銃の距離

    // 変数--------------------------------
    // 銃 サイト
    [SerializeField] GameObject gun, site, sceneManager, pauseText;
    [SerializeField] AudioClip gunReadySE, gunShotSE;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (sceneManager.GetComponent<PlaySceneManager>().GetState() != PlaySceneManager.eSTATE.PLAY || gun.GetComponent<ShotBullet>().GetShot()) return;
        
        // サイトを表示する
        site.SetActive(true);
        // クリックしている座標の取得
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // サイトの座標を変更する
        site.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, site.transform.position.z);
        // 自身とサイトの角度を計算する
        float angle = Calculation.GetAngle(PLAYER_DEFAULT_POS, mouseWorldPos);
        float rad = angle * Mathf.Deg2Rad;

        if (angle > 90 || angle < -90) transform.rotation = Quaternion.Euler(0, 180, 0);
        else transform.rotation = Quaternion.identity;

        // 銃の座標の調整
        GunPositionUpdate(rad);
        // 銃の角度の調整
        GunAngleUpdate(angle);


        // マウス左ボタンが押されたら、撃つ
        if (Input.GetMouseButtonDown(0) && !pauseText.activeSelf)
        {
            // 弾を発射する
            gun.GetComponent<ShotBullet>().Shot();
            // サイトを非表示にする
            site.SetActive(false);
            // 銃を撃った音を流す
            GetComponent<AudioSource>().PlayOneShot(gunShotSE);
        }
    }

    void GunAngleUpdate(float _angle)
    {
        // 狙う方向に銃と傾けるようにする
        gun.transform.rotation = Quaternion.Euler(0, 0, _angle);
        // 一定以上以下の角度の場合銃を反転させて向きを正しくする
        if (_angle >= 90 || _angle <= -90) gun.transform.rotation = Quaternion.Euler(INVERSION, 0, -_angle);
    }

    void GunPositionUpdate(float _rad)
    {
        // ラジアンから進行方向を算出
        Vector3 direction = new Vector3(Mathf.Cos(_rad), Mathf.Sin(_rad), 0);
        // 移動ベクトル
        Vector3 vec = direction * GUN_DIS;
        // 移動ベクトル分、銃とプレイヤーの距離を離す
        gun.transform.position = transform.position + vec + GUN_ADJ_POS;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // 定数--------------------------------

    // 変数--------------------------------

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 弾と当たったらフラグを立てる
        if (collision.transform.tag == "Bullet")
        {
            Destroy(gameObject);
        }
    }
}

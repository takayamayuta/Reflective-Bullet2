using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // �萔--------------------------------

    // �ϐ�--------------------------------

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
        // �e�Ɠ���������t���O�𗧂Ă�
        if (collision.transform.tag == "Bullet")
        {
            Destroy(gameObject);
        }
    }
}

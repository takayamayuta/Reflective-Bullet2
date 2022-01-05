using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // ’è”--------------------------------

    // •Ï”--------------------------------

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
        // ’e‚Æ“–‚½‚Á‚½‚çƒtƒ‰ƒO‚ğ—§‚Ä‚é
        if (collision.transform.tag == "Bullet")
        {
            Destroy(gameObject);
        }
    }
}

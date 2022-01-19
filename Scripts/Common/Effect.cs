using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    // íËêî
    readonly Vector2 DEFAULT_SCALE = new Vector2(1.0f, 1.0f);

    // ïœêî
    static GameObject prefab = null;
    public Sprite[] sprites;
    public float speed;
    private Image image;
    private float current;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = sprites[0];
        current = 0;
        IEnumerator coroutine = UpdateImg();
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator UpdateImg()
    {
        int idx = 0;
        while (idx < sprites.Length - 1)
        {
            current += Time.deltaTime * speed;
            idx = (int)(current) % sprites.Length;
            if (idx > sprites.Length - 1) idx = sprites.Length - 1;
            image.sprite = sprites[idx];
            yield return new WaitForSeconds(0.01f);
        }

        Destroy(gameObject);
    }

    public static void EffectAdd(Vector3 pos, string name, string parent = null, Vector3? scale = null)
    {
        if (scale == null) scale = new Vector3(1.0f, 1.0f, 1.0f);

        GameObject prefab = Resources.Load("Prefabs/Effects/" + name) as GameObject;
        GameObject canvas = GameObject.Find(parent);
        GameObject g = Instantiate(prefab, pos, Quaternion.identity) as GameObject;
        g.transform.localScale = (Vector3)scale;
        g.transform.SetParent(canvas.transform, false);
    }
}

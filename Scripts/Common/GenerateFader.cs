using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFader : MonoBehaviour
{
    // ��������I�u�W�F�N�g
    [SerializeField] GameObject fader;

    // �I�u�W�F�N�g�̐���
    public GameObject Generate()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject go = Instantiate(fader, canvas.transform.position, Quaternion.identity, canvas.transform);
        return go;
    }
}

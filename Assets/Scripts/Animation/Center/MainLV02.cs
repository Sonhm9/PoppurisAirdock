using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLV02 : MonoBehaviour
{
    public GameObject obj01;
    public GameObject obj02;

    private float rotateSpeed = 20f;

    void Update()
    {
        obj01.transform.Rotate(Vector3.one * Time.deltaTime * rotateSpeed);
        obj02.transform.Rotate(Vector3.one * Time.deltaTime * rotateSpeed * 3f);
    }
}

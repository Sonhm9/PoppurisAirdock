using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingObject : MonoBehaviour
{
    public GameObject obj01;

    private float rotateSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        obj01.transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
    }
}

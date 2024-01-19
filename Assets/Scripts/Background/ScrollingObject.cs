using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private float speed = 0.1f; // 구름 이동속도

    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (transform.position.x < -BackgroundLoop.backgroundWidth)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        transform.Translate(2 * BackgroundLoop.backgroundWidth, 0, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayoutAnimation : MonoBehaviour
{
    public float blinkSpeed = 1.0f; // ������ �ӵ� ����
    public float minAlpha = 0.0f;   // �ּ� ����
    public float maxAlpha = 1.0f;   // �ִ� ����

    private SpriteRenderer spriteRenderer;
    private bool isIncreasingAlpha = true;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            // ������ ������ �����Ͽ� �����̴� ȿ�� ����
            float alpha = spriteRenderer.color.a;

            if (isIncreasingAlpha)
            {
                alpha += Time.deltaTime * blinkSpeed;
                if (alpha >= maxAlpha)
                {
                    alpha = maxAlpha;
                    isIncreasingAlpha = false;
                }
            }
            else
            {
                alpha -= Time.deltaTime * blinkSpeed;
                if (alpha <= minAlpha)
                {
                    alpha = minAlpha;
                    isIncreasingAlpha = true;
                }
            }

            Color newColor = spriteRenderer.color;
            newColor.a = alpha;
            spriteRenderer.color = newColor;

            yield return null;
        }
    }
}

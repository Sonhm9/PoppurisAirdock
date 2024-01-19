using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayoutAnimation : MonoBehaviour
{
    public float blinkSpeed = 1.0f; // 깜빡임 속도 조절
    public float minAlpha = 0.0f;   // 최소 투명도
    public float maxAlpha = 1.0f;   // 최대 투명도

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
            // 투명도를 서서히 변경하여 깜빡이는 효과 생성
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

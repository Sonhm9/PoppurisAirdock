using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainmapScaleController : MonoBehaviour
{
    [SerializeField] SpriteRenderer mainMap; // ∏ﬁ¿Œ ∏ 
    private BoxCollider mainMapCollider; // ∏ﬁ¿Œ ∏  ƒ›∂Û¿Ã¥ı

    void Start()
    {
        mainMap = GetComponent<SpriteRenderer>();
        mainMapCollider = GetComponent<BoxCollider>();
    }

    public void WidthUpgrade()
    {
        Vector2 scale = mainMap.size;
        Vector2 colliderScale = mainMapCollider.size;

        scale.x += 5f;
        colliderScale.x += 5f;

        mainMap.size = scale;
        mainMapCollider.size = colliderScale;
    }
    public void HeightUpgrade()
    {
        Vector2 scale = mainMap.size;
        Vector2 colliderScale = mainMapCollider.size;

        scale.y += 6f;
        colliderScale.y += 6f;

        mainMap.size = scale;
        mainMapCollider.size = colliderScale;
    }

}

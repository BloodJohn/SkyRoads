using UnityEngine;

public class BridgeView : MonoBehaviour
{
    public void SetAlpha(float a)
    {
        var sprite = GetComponentInChildren<SpriteRenderer>();
        var color = sprite.color;
        color.a = a;
        sprite.color = color;
    }
}
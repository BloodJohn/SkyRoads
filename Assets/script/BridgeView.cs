using UnityEngine;

public class BridgeView : MonoBehaviour
{
    public GameObject prefabScore;

    public void SetAlpha(float a)
    {
        var sprite = GetComponentInChildren<SpriteRenderer>();
        var color = sprite.color;
        color.a = a;
        sprite.color = color;
    }

    public void ShowScore(float score)
    {
        if (Mathf.Abs(score) < 0.2f) return;

        if (prefabScore == null) return;
        var scoreFly = Instantiate(prefabScore);
        scoreFly.transform.position = transform.position;

        var flyText = scoreFly.GetComponentInChildren<TextMesh>();
        flyText.text = Mathf.Abs(score).ToString("0.#");
        flyText.color = score > 0f ? Color.green : Color.red;

        Destroy(scoreFly, 1f);
    }
}
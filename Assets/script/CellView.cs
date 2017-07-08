using UnityEngine;

public class CellView : MonoBehaviour
{
    public int id;
    public int x;
    public int y;
    public bool hasBridge;
    public int roadTest;
    public GameObject prefabScore;
    private GameObject body;
    public bool IsTown { get { return id >= 5; } }
    public float speed;
    private Vector3 pos = new Vector3();

    private void Awake()
    {
        body = GetComponentInChildren<BoxCollider2D>().gameObject;
    }

    public void Fly(float time)
    {
        pos.x = (speed + Mathf.Sign(speed) * time * 10f) * time;
        body.transform.localPosition = pos;
    }

    public void FlyDown(float time)
    {
        pos.y = -(Mathf.Abs(speed) + 10f * time) * time;
        body.transform.localPosition = pos;
    }


    public void ShowScore(float score)
    {
        //Debug.LogFormat("score: {0}", score);
        if (Mathf.Abs(score)<0.2f) return;

        if (prefabScore==null) return;
        var scoreFly = Instantiate(prefabScore, transform);

        var flyText = scoreFly.GetComponentInChildren<TextMesh>();
        flyText.text = Mathf.Abs(score).ToString("0.#");
        flyText.color = score>0f ? Color.green : Color.red;

        Destroy(scoreFly, 1f);
    }
}

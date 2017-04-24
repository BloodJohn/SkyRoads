using UnityEngine;

public class CellView : MonoBehaviour
{
    public int id;
    public int x;
    public int y;
    public bool hasBridge;
    public int roadTest;
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

}

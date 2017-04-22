using System.Collections.Generic;
using UnityEngine;

public class GridLayer : MonoBehaviour
{
    public GameObject cellPrefab;
    public float dx = 0.737f;
    public float dy = 0.426f;

    public int sizeX = 3;
    public int sizeY = 3;

    public int trimX = 1;
    public int trimY = 1;

    // Use this for initialization
    void Start()
    {
        BuildGrid();
    }

    private void BuildGrid()
    {
        var shiftX = new Vector3(dx, -dy);
        var shiftY = new Vector3(-dx, -dy);
        var topShift = new Vector3(0f, dy * (sizeX + sizeY) / 2);

        var cellList = new List<int>(sizeX * sizeY);

        for (var y = 0; y < sizeY; y++)
            for (var x = 0; x < sizeX; x++)
            {
                if (x + y < trimX)
                {
                    cellList.Add(0);
                }
                else if (sizeX-1-x + sizeY-1-y < trimX)
                {
                    cellList.Add(0);
                }
                else if (sizeX - 1 - x + y < trimY)
                {
                    cellList.Add(0);
                }
                else if (x + sizeY - 1 - y < trimY)
                {
                    cellList.Add(0);
                }
                else
                {
                    cellList.Add(1);
                }
            }




        for (var x = 0; x < sizeX; x++)
        {
            for (var y = 0; y < sizeY; y++)
            {
                if (cellList[y*sizeX + x] == 0) continue;

                var newCell = Instantiate(cellPrefab, transform);
                var pos = new Vector3();
                pos = topShift;
                pos += shiftX * x + shiftY * y;
                pos.z = -(x + y) * 0.01f;
                newCell.transform.localPosition = pos;
                newCell.name = string.Format("cell_{0}_{1}", x, y);
            }
        }
    }
}

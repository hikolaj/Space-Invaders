using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    public float TopOfGrid { get { return _topOfGrid; } }
    public float TopOfMap { get; private set; }
    public float BottomOfMap { get; private set; }
    public float LeftBoundary { get; private set; }
    public float RightBoundary { get; private set; }

    public float CellSize = 0.5f;
    public int TotalCollumnAmount = 18;
    public int TotalRowAmount = 14;

    public bool DebugOn;

    private const float _topOfGrid = 3.5f;

    public void Initialize()
    {
        LeftBoundary = XPositionAtIndex(0) - CellSize / 2;
        RightBoundary = XPositionAtIndex(TotalCollumnAmount - 1) + CellSize / 2;
        TopOfMap = YPositionAtIndex(0) + CellSize + CellSize / 2;
        BottomOfMap = YPositionAtIndex(TotalRowAmount) - CellSize/2;
    }

    public Vector2 PositionAtIndex(float x, float y)
    {
        return new Vector2(XPositionAtIndex(x), YPositionAtIndex(y));
    }

    public float XPositionAtIndex(float xIndex)
    {
        return -TotalCollumnAmount / 2f * CellSize + CellSize / 2f + xIndex * CellSize;
    }

    public float YPositionAtIndex(float yIndex)
    {
        return _topOfGrid - CellSize / 2 - yIndex * CellSize;
    }

    void OnDrawGizmos()
    {
        if (DebugOn)
        {
            //draw map
            Gizmos.color = Color.white;
            for (int x = 0; x < TotalCollumnAmount; x++)
            {
                for (int y = 0; y < TotalRowAmount; y++)
                {
                    GizmosExtensions.DrawLineQuad(PositionAtIndex(x, y), CellSize);
                }
            }
        }
    }
}

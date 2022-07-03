using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmosExtensions
{
    public static void DrawLineQuad(Vector2 position, float size)
    {
        float halfSize = size / 2;
        //corners
        Vector2 bl = new Vector2(position.x - halfSize, position.y - halfSize);
        Vector2 br = new Vector2(position.x + halfSize, position.y - halfSize);
        Vector2 ul = new Vector2(position.x - halfSize, position.y + halfSize);
        Vector2 ur = new Vector2(position.x + halfSize, position.y + halfSize);

        Gizmos.DrawLine(bl, br);
        Gizmos.DrawLine(ul, ur);
        Gizmos.DrawLine(bl, ul);
        Gizmos.DrawLine(br, ur);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasRenderer))]
public class UILineRenderer : MaskableGraphic
{
    public float lineThickness = 10f;

    [SerializeField]
    private List<Vector2> points = new List<Vector2>();

    public void SetPoints(List<Vector2> pointList)
    {
        // Create a copy to avoid reference issues
        points = new List<Vector2>(pointList);
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points == null || points.Count < 2)
            return;

        for (int i = 0; i < points.Count; i++)
        {
            Vector2 point = points[i];
            Vector2 dir;

            // Calculate direction (tangent) at this point
            if (i == 0)
            {
                // Start point: Look at next
                dir = (points[i + 1] - point).normalized;
            }
            else if (i == points.Count - 1)
            {
                // End point: Look at prev
                dir = (point - points[i - 1]).normalized;
            }
            else
            {
                // Middle points: Average the direction of incoming and outgoing
                Vector2 dirPrev = (point - points[i - 1]).normalized;
                Vector2 dirNext = (points[i + 1] - point).normalized;
                dir = (dirPrev + dirNext).normalized;
            }

            // Calculate perpendicular vector (Normal)
            // (-y, x) rotates the vector 90 degrees
            Vector2 normal = new Vector2(-dir.y, dir.x) * (lineThickness / 2f);

            // Add two vertices (left and right side of the line)
            vh.AddVert(point - normal, color, Vector2.zero);
            vh.AddVert(point + normal, color, Vector2.zero);

            // Add triangles to connect this segment to the previous one
            // We skip the very first point because it has no previous point to connect to
            if (i > 0)
            {
                int currentVertIndex = i * 2;
                int prevVertIndex = (i - 1) * 2;

                // Connect Left-Prev, Left-Curr, Right-Prev
                vh.AddTriangle(prevVertIndex, currentVertIndex, prevVertIndex + 1);
                // Connect Right-Prev, Left-Curr, Right-Curr
                vh.AddTriangle(prevVertIndex + 1, currentVertIndex, currentVertIndex + 1);
            }
        }
    }
}
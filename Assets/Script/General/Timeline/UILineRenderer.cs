using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasRenderer))]
public class UILineRenderer : MaskableGraphic
{
    public float lineThickness = 10f;
    public bool relativeSize = false; // If true, UVs stretch. If false, they tile.
    public float tiling = 1.0f;       // How often the texture repeats

    public Texture lineTexture;
    [SerializeField] private List<Vector2> points = new List<Vector2>();

    // Override mainTexture to allow custom textures (dotted lines, etc)
    public override Texture mainTexture
    {
        get
        {
            return lineTexture == null ? s_WhiteTexture : lineTexture;
        }
    }

    public void SetPoints(List<Vector2> pointList)
    {
        points = new List<Vector2>(pointList);
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points == null || points.Count < 2)
            return;

        float distance = 0f;

        for (int i = 0; i < points.Count; i++)
        {
            Vector2 point = points[i];
            Vector2 dir;

            // Calculate direction
            if (i == 0) dir = (points[i + 1] - point).normalized;
            else if (i == points.Count - 1) dir = (point - points[i - 1]).normalized;
            else
            {
                Vector2 dirPrev = (point - points[i - 1]).normalized;
                Vector2 dirNext = (points[i + 1] - point).normalized;
                dir = (dirPrev + dirNext).normalized;
            }

            // Calculate distance for UV tiling
            if (i > 0)
            {
                distance += Vector2.Distance(points[i], points[i - 1]);
            }

            // Calculate UV Y coordinate
            // If relativeSize is true, we map 0..1 across the whole line
            // If false, we map based on pixels (good for repeating patterns)
            float uvY = relativeSize ? (float)i / (points.Count - 1) : distance / (lineThickness * tiling);

            // Calculate Normals
            Vector2 normal = new Vector2(-dir.y, dir.x) * (lineThickness / 2f);

            // Add Vertices with UVs
            // UV X: 0 = Top of line, 1 = Bottom of line
            vh.AddVert(point - normal, color, new Vector2(0, uvY));
            vh.AddVert(point + normal, color, new Vector2(1, uvY));

            // Add Triangles
            if (i > 0)
            {
                int currentVertIndex = i * 2;
                int prevVertIndex = (i - 1) * 2;

                vh.AddTriangle(prevVertIndex, currentVertIndex, prevVertIndex + 1);
                vh.AddTriangle(prevVertIndex + 1, currentVertIndex, currentVertIndex + 1);
            }
        }
    }
}
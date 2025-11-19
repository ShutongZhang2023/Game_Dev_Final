using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FlowchartConnection
{
    public RectTransform startImage;
    public RectTransform endImage;
    public Color lineColor = Color.white;

    [HideInInspector]
    public GameObject lineContainer;
}

public class UIFlowchartConnector : MonoBehaviour
{
    [Header("Connections")]
    public List<FlowchartConnection> connections = new List<FlowchartConnection>();

    [Header("Line Settings")]
    [Range(1f, 50f)]
    public float lineThickness = 8f;

    [Range(10, 100)]
    public int totalPoints = 40;

    [Range(0.1f, 3.0f)]
    public float tangentStrength = 1.0f;

    [Tooltip("How many pixels the line should go 'inside' the images to prevent gaps.")]
    public float overlapAmount = 5f;

    public bool autoUpdate = true;

    void Start()
    {
        UpdateAllLines();
    }

    void Update()
    {
        if (autoUpdate) UpdateAllLines();
    }

    public void UpdateAllLines()
    {
        if (connections == null) return;

        for (int i = 0; i < connections.Count; i++)
        {
            UpdateConnection(connections[i]);
        }
    }

    void UpdateConnection(FlowchartConnection connection)
    {
        if (connection.startImage == null || connection.endImage == null) return;

        // 1. Create or Get Container
        if (connection.lineContainer == null)
        {
            connection.lineContainer = new GameObject("ConnectionLine");
            // Add the LineRenderer component immediately
            connection.lineContainer.AddComponent<UILineRenderer>();
        }

        // 2. PARENTING FIX: Make the line a child of the End Image
        if (connection.lineContainer.transform.parent != connection.endImage)
        {
            connection.lineContainer.transform.SetParent(connection.endImage, false);

            // Push to back so it renders behind any text inside the box (optional)
            connection.lineContainer.transform.SetAsFirstSibling();
        }

        // Reset transform to ensure it sits at (0,0) of the End Image
        RectTransform lineRT = connection.lineContainer.GetComponent<RectTransform>();
        lineRT.anchorMin = new Vector2(0.5f, 0.5f);
        lineRT.anchorMax = new Vector2(0.5f, 0.5f);
        lineRT.pivot = new Vector2(0.5f, 0.5f);
        lineRT.anchoredPosition = Vector2.zero;
        lineRT.localRotation = Quaternion.identity;
        lineRT.localScale = Vector3.one;


        // 3. CALCULATE POINTS (In the coordinate space of the End Image)

        // A. Get Start Point (Right side of Start Image)
        // We get the World Position, then convert it to End Image's Local Space
        Vector3 startWorldPos = GetWorldPointOnRect(connection.startImage, new Vector2(1, 0.5f));
        Vector2 startLocalPos = connection.endImage.InverseTransformPoint(startWorldPos);

        // B. Get End Point (Left side of End Image)
        // Since we are inside the End Image, we just use its own rect
        Rect endRect = connection.endImage.rect;
        Vector2 endLocalPos = new Vector2(endRect.xMin, endRect.center.y);

        // 4. GAP FIX: Apply Overlap
        // Move the start point slightly to the left (local space) and end point slightly right
        // Note: Since start is likely to the left of end, we need to check direction or just force X

        // Push Start point inside the Start Image (Move Left)
        startLocalPos.x -= overlapAmount;

        // Push End point inside the End Image (Move Right)
        endLocalPos.x += overlapAmount;


        // 5. Generate Curve
        List<Vector2> points = GenerateCubicBezierPoints(startLocalPos, endLocalPos);

        // 6. Update Renderer
        UILineRenderer lineRenderer = connection.lineContainer.GetComponent<UILineRenderer>();
        lineRenderer.lineThickness = lineThickness;
        lineRenderer.color = connection.lineColor;
        lineRenderer.SetPoints(points);

        // Ensure the raycast target is off so the line doesn't block mouse clicks
        lineRenderer.raycastTarget = false;
    }

    // Helper to get a point on a RectTransform in World Space
    Vector3 GetWorldPointOnRect(RectTransform rect, Vector2 normalizedPivot)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);

        // corners[0] = bottom-left, corners[1] = top-left
        // corners[2] = top-right,   corners[3] = bottom-right

        // Simple Lerp for Right-Middle (1, 0.5) or Left-Middle (0, 0.5)
        Vector3 result = Vector3.Lerp(
            Vector3.Lerp(corners[0], corners[3], normalizedPivot.x), // Bottom X lerp
            Vector3.Lerp(corners[1], corners[2], normalizedPivot.x), // Top X lerp
            normalizedPivot.y // Y lerp
        );

        return result;
    }

    List<Vector2> GenerateCubicBezierPoints(Vector2 start, Vector2 end)
    {
        List<Vector2> points = new List<Vector2>();

        float distance = Mathf.Abs(start.x - end.x);
        float handleX = distance * 0.5f * tangentStrength;
        handleX = Mathf.Max(handleX, 20f);

        Vector2 p0 = start;
        Vector2 p1 = start + Vector2.right * handleX;
        Vector2 p2 = end + Vector2.left * handleX;
        Vector2 p3 = end;

        for (int i = 0; i <= totalPoints; i++)
        {
            float t = i / (float)totalPoints;
            points.Add(CalculateBezierPoint(t, p0, p1, p2, p3));
        }

        return points;
    }

    Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector2 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}
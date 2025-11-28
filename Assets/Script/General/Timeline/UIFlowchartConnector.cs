using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class FlowchartConnection
{
    public RectTransform startImage;
    public RectTransform endImage;
    public Color lineColor = Color.white;
    public Texture customTexture; // Optional texture for this specific line

    [HideInInspector] public GameObject lineContainer;
    [HideInInspector] public GameObject arrowObject;
}

public class UIFlowchartConnector : MonoBehaviour
{
    public static UIFlowchartConnector instance;
    [Header("Connections")]
    public List<FlowchartConnection> connections = new List<FlowchartConnection>();

    [Header("Line Visuals")]
    [Range(1f, 50f)] public float lineThickness = 8f;
    [Range(10, 100)] public int totalPoints = 40;
    [Tooltip("Requires Texture Wrap Mode = Repeat")]
    public float textureTiling = 2.0f;

    [Header("Curve Physics")]
    [Range(0.1f, 3.0f)] public float tangentStrength = 1.0f;
    public float overlapAmount = 5f;

    [Header("Arrow Settings")]
    public bool showArrows = true;
    public Sprite arrowSprite; // MUST POINT RIGHT by default
    public Vector2 arrowSize = new Vector2(20, 20);
    public Color arrowColor = Color.white;

    public bool autoUpdate = true;

    private void Awake()
    {
        instance = this;
    }

    void Start() { UpdateAllLines(); }
    void Update() { if (autoUpdate) UpdateAllLines(); }

    public void UpdateAllLines()
    {
        if (connections == null) return;
        for (int i = 0; i < connections.Count; i++) UpdateConnection(connections[i]);
    }

    void UpdateConnection(FlowchartConnection connection)
    {
        if (connection.startImage == null || connection.endImage == null) return;

        // --- 1. Setup Line Container ---
        if (connection.lineContainer == null)
        {
            connection.lineContainer = new GameObject("ConnectionLine");
            connection.lineContainer.AddComponent<UILineRenderer>();
        }

        // Parent Logic (Child of End Image)
        if (connection.lineContainer.transform.parent != connection.endImage)
        {
            connection.lineContainer.transform.SetParent(connection.endImage, false);
            connection.lineContainer.transform.SetAsFirstSibling();
        }

        // Reset Transform
        RectTransform lineRT = connection.lineContainer.GetComponent<RectTransform>();
        lineRT.anchorMin = new Vector2(0.5f, 0.5f);
        lineRT.anchorMax = new Vector2(0.5f, 0.5f);
        lineRT.pivot = new Vector2(0.5f, 0.5f);
        lineRT.anchoredPosition = Vector2.zero;
        lineRT.localRotation = Quaternion.identity;
        lineRT.localScale = Vector3.one;

        // --- 2. Calculate Points ---
        Vector3 startWorldPos = GetWorldPointOnRect(connection.startImage, new Vector2(1, 0.5f));
        Vector2 startLocalPos = connection.endImage.InverseTransformPoint(startWorldPos);
        Rect endRect = connection.endImage.rect;
        Vector2 endLocalPos = new Vector2(endRect.xMin, endRect.center.y);

        startLocalPos.x -= overlapAmount;
        endLocalPos.x += overlapAmount;

        // Generate Bezier Points
        List<Vector2> points = GenerateCubicBezierPoints(startLocalPos, endLocalPos, connection);

        // --- 3. Update Line Renderer ---
        UILineRenderer lineRenderer = connection.lineContainer.GetComponent<UILineRenderer>();
        lineRenderer.lineThickness = lineThickness;
        lineRenderer.color = connection.lineColor;
        lineRenderer.lineTexture = connection.customTexture;
        lineRenderer.tiling = textureTiling;
        lineRenderer.SetPoints(points);
        lineRenderer.raycastTarget = false;

        // --- 4. Arrow Logic ---
        UpdateArrow(connection, startLocalPos, endLocalPos);
    }

    void UpdateArrow(FlowchartConnection connection, Vector2 start, Vector2 end)
    {
        if (!showArrows || arrowSprite == null)
        {
            if (connection.arrowObject != null) Destroy(connection.arrowObject);
            return;
        }

        if (connection.arrowObject == null)
        {
            connection.arrowObject = new GameObject("DirectionArrow");
            connection.arrowObject.transform.SetParent(connection.lineContainer.transform, false);
            Image img = connection.arrowObject.AddComponent<Image>();
            img.raycastTarget = false;
        }

        // Setup Image
        Image arrowImg = connection.arrowObject.GetComponent<Image>();
        arrowImg.sprite = arrowSprite;
        arrowImg.color = arrowColor;
        arrowImg.rectTransform.sizeDelta = arrowSize;

        // Calculate Middle Position and Rotation
        // We use the control points logic here again to find the exact middle (t=0.5)
        float distance = Mathf.Abs(start.x - end.x);
        float handleX = Mathf.Max(distance * 0.5f * tangentStrength, 20f);

        Vector2 p0 = start;
        Vector2 p1 = start + Vector2.right * handleX;
        Vector2 p2 = end + Vector2.left * handleX;
        Vector2 p3 = end;

        // Position at t = 0.5
        Vector2 arrowPos = CalculateBezierPoint(0.5f, p0, p1, p2, p3);

        // Rotation: Look slightly ahead (t=0.51) to get direction
        Vector2 arrowLookPos = CalculateBezierPoint(0.51f, p0, p1, p2, p3);
        Vector2 dir = (arrowLookPos - arrowPos).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        RectTransform arrowRT = arrowImg.rectTransform;
        arrowRT.anchoredPosition = arrowPos;
        arrowRT.localRotation = Quaternion.Euler(0, 0, angle);
    }

    Vector3 GetWorldPointOnRect(RectTransform rect, Vector2 normalizedPivot)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        return Vector3.Lerp(
            Vector3.Lerp(corners[0], corners[3], normalizedPivot.x),
            Vector3.Lerp(corners[1], corners[2], normalizedPivot.x),
            normalizedPivot.y
        );
    }

    List<Vector2> GenerateCubicBezierPoints(Vector2 start, Vector2 end, FlowchartConnection conn)
    {
        List<Vector2> points = new List<Vector2>();
        float distance = Mathf.Abs(start.x - end.x);
        float handleX = Mathf.Max(distance * 0.5f * tangentStrength, 20f);

        Vector2 p0 = start;
        Vector2 p1 = start + Vector2.right * handleX;
        Vector2 p2 = end + Vector2.left * handleX;
        Vector2 p3 = end;

        for (int i = 0; i <= totalPoints; i++)
        {
            points.Add(CalculateBezierPoint(i / (float)totalPoints, p0, p1, p2, p3));
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

    public void CreateConnection(RectTransform start, RectTransform end)
    {
        FlowchartConnection newConn = new FlowchartConnection
        {
            startImage = start,
            endImage = end
        };

        connections.Add(newConn);
        UpdateConnection(newConn);
    }

    public void ClearConnections()
    {
        foreach (var conn in connections)
        {
            if (conn.lineContainer != null)
            {
                Destroy(conn.lineContainer);
            }
            if (conn.arrowObject != null)
            {
                Destroy(conn.arrowObject);
            }
        }
        connections.Clear();
    }   
}
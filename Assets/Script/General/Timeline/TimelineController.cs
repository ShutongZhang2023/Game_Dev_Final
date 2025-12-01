using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineController : MonoBehaviour
{
    public Transform nodeContainer;
    private List<TimelineNode> nodes = new List<TimelineNode>();
    public Color currentLineColor = Color.green;
    public Color defaultLineColor = Color.white;

    private void OnEnable()
    {
        LoadNodes();
        StartCoroutine(DelayedRebuild());
    }

    private IEnumerator DelayedRebuild()
    {
        yield return null;

        RebuildTimeline();
    }

    private void LoadNodes()
    {
        nodes.Clear();

        foreach (Transform child in nodeContainer)
        {
            TimelineNode node = child.GetComponent<TimelineNode>();
            if (node != null)
            {
                nodes.Add(node);
            }
        }
    }
    public void RebuildTimeline() 
    {
        foreach (var node in nodes)
        {
            if (node == null) continue;
            node.RefreshVisibility();
            node.RefreshHighlight();
        }

        UIFlowchartConnector.instance.ClearConnections();

        foreach (var parent in nodes)
        {
            if (!parent.gameObject.activeSelf) continue;

            bool isParentCompleted = parent.isCurrentNode;

            foreach (var child in parent.childNodes)
            {
                if (child == null) continue;
                if (!child.gameObject.activeSelf) continue;
                bool isChildCompleted = child.isCurrentNode;
                Color lineColor = (isParentCompleted && isChildCompleted) ? currentLineColor : defaultLineColor;

                UIFlowchartConnector.instance.CreateConnection(
                    parent.uiRect,
                    child.uiRect,
                    lineColor
                );
            }
        }
    }


}

using System.Collections.Generic;
using UnityEngine;

public class TimelineController : MonoBehaviour
{
    public Transform nodeContainer;
    private List<TimelineNode> nodes = new List<TimelineNode>();

    private void OnEnable()
    {
        LoadNodes();
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
            node.RefreshVisibility();
        }

        UIFlowchartConnector.instance.ClearConnections();

        foreach (var parent in nodes)
        {
            if (!parent.gameObject.activeSelf) continue;

            foreach (var child in parent.childNodes)
            {
                if (child == null) continue;
                if (!child.gameObject.activeSelf) continue;

                UIFlowchartConnector.instance.CreateConnection(
                    parent.uiRect,
                    child.uiRect
                );
            }
        }
    }


}

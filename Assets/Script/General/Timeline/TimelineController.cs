using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineController : MonoBehaviour
{
    public Transform nodeContainer;
    private List<TimelineNode> nodes = new List<TimelineNode>();

    private void OnEnable()
    {
        LoadNodes();
        StartCoroutine(DelayedRebuild());
    }

    private IEnumerator DelayedRebuild()
    {
        yield return null;   // 等 1 帧，确保 RectTransform 和 Canvas 布局稳定

        RebuildTimeline();   // 现在重建线不会出现任何 null / 坐标错误
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

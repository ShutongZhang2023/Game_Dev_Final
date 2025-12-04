using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineNode : MonoBehaviour
{
    [Header("Node Data")]
    public bool isSceneNode = false;          // Can player click to change scene
    public string connectedSceneName;         // Target scene if clickable
    public bool isCurrentNode = false;        // Is this the current active node

    [Header("Visibility Flag")]
    public string requiredFlag;               // Node will show only if this flag is set

    [Header("Children")]
    public List<TimelineNode> childNodes = new List<TimelineNode>();

    [Header("UI")]
    public RectTransform uiRect;
    public Color currentColor = Color.green; // 【新增】已解锁时的颜色
    public Color normalColor = Color.white;
    public Image nodeImage;

    private void Awake()
    {
        if (uiRect == null)
        {
            uiRect = GetComponent<RectTransform>();
        }
    }

    public void RefreshVisibility()
    {
        bool unlocked = StoryState.instance.HasPersistedFlag(requiredFlag);
        gameObject.SetActive(unlocked);
    }

    public void RefreshHighlight()
    {
        if (nodeImage == null) return;
        isCurrentNode = StoryState.instance.HasFlag(requiredFlag);
        if (isCurrentNode)
        {
            nodeImage.color = currentColor;
        }
        else
        {
            nodeImage.color = normalColor;
        }
    }


    public void OnClickNode()
    {
        if (!isSceneNode) return;
        Debug.Log("Loading scene: " + connectedSceneName);
        SceneManager.instance.ChangeContentScene(connectedSceneName);
        StoryState.instance.ClearFromFlag(requiredFlag);
        CanvasLoad.instance.ToggleCanvas();
    }

}

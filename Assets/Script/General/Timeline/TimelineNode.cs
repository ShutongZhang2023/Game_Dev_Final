using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TimelineNode : MonoBehaviour
{
    [Header("Node Data")]
    public bool isSceneNode = false;          // Can player click to change scene
    public string connectedSceneName;         // Target scene if clickable

    [Header("Visibility Flag")]
    public string requiredFlag;               // Node will show only if this flag is set

    [Header("Children")]
    public List<TimelineNode> childNodes = new List<TimelineNode>();

    [Header("UI")]
    public RectTransform uiRect;

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

    public void OnClickNode()
    {
        if (!isSceneNode) return;
        Debug.Log("Loading scene: " + connectedSceneName);
        // SceneManager.LoadScene(connectedSceneName);
    }

}

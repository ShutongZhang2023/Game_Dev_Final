using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;


// Enum to represent different types of dialogue nodes, can be expanded as needed
public enum DialogueNodeType
{
    Dialogue,
    Choice,
    Action
}

[Serializable]
//data for choice
public class ChoiceData
{
    public string choiceId; // must contain
    public string text;            // Text on UI, can be empty
    public string needFlag;      // can be empty, choice will show only if flag is set
    public string nextNodeId; // must contain
    public string setFlag;         // can be empty, tag will set after chosen
    public UnityEvent onChosen;    // can be empty, event gonna invoke after chosen
}


[Serializable]
public class DialogueNode
{
    public string id;              // node id, unique
    public DialogueNodeType nodeType;

    public string speaker;
    public string content;

    public ChoiceData[] choices;   // only for Choice node
    public string defaultNextNodeId;
}



[CreateAssetMenu(fileName = "DialogueAsset", menuName = "VN/Dialogue Asset")]
public class DialogueAsset : ScriptableObject
{
    public DialogueNode[] nodes;

    public DialogueNode GetNodeById(string id)
    {
        foreach (var node in nodes)
        {
            if (node.id == id)
                return node;
        }
        Debug.Log("Node not found: " + id);
        return null;
    }
}

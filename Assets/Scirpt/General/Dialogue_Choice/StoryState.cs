using System.Collections.Generic;
using UnityEngine;

public class StoryState : MonoBehaviour
{
    public HashSet<string> flags = new HashSet<string>();

    public void SetFlag(string flagName)
    {
        flags.Add(flagName);
        Debug.Log(string.Join(", ", flags));
    }

    public bool HasFlag(string flagName)
    {
        return flags.Contains(flagName);
    }

    public void ClearFlag(string flagName)
    {
        flags.Remove(flagName);
    }

}

using System.Collections.Generic;
using UnityEngine;

public class StoryState : MonoBehaviour
{
    public List<string> currentFlags = new List<string>();
    public List<string> persistedFlags = new List<string>();
    public static StoryState instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetFlag(string flagName)
    {
        if (!currentFlags.Contains(flagName))
        {
            currentFlags.Add(flagName);
        }

        if (!persistedFlags.Contains(flagName))
        {
            persistedFlags.Add(flagName);
        }
    }

    public bool HasFlag(string flagName)
    {
        return currentFlags.Contains(flagName);
    }

    public bool HasPersistedFlag(string flagName)
    {
        return persistedFlags.Contains(flagName);
    }

    public void ClearFromFlag(string flagName)
    {
        int indexToRemove = currentFlags.IndexOf(flagName);
        int countToRemove = currentFlags.Count - indexToRemove;

        if (countToRemove > 0)
        {
            currentFlags.RemoveRange(indexToRemove, countToRemove);
        }
    }

    public void ResetCurrentFlags()
    {
        currentFlags.Clear();
    }

}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoryState : MonoBehaviour
{
    [Header("Flags")]
    public List<string> currentFlags = new List<string>();
    public List<string> persistedFlags = new List<string>();

    [Header("Affection System")]
    public Dictionary<string, int> sceneAffection = new Dictionary<string, int>();
    public int totalAffection = 0;

    [Header("ExtraFlag")]
    public bool activeTimeline = false;
    public bool passRound1 = false;

    public static StoryState instance;
    public static UnityAction<string> onFlagUpdated;

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

        onFlagUpdated?.Invoke(flagName);
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

        CleanSceneAffection(flagName);
    }

    public void ResetCurrentFlags()
    {
        currentFlags.Clear();
    }

    public void SetSceneAffection(string sceneKey, int delta)
    {
        sceneAffection[sceneKey] = delta;
        RecalculateTotalAffection();
    }

    public void RecalculateTotalAffection()
    {
        int sum = 0;
        foreach (string key in currentFlags)
        {
            if (sceneAffection.TryGetValue(key, out int value))
                sum += value;
        }

        totalAffection = sum;
    }

    public void CleanSceneAffection(string thisScene)
    {
        List<string> keysToRemove = new List<string>();

        foreach (var key in sceneAffection.Keys)
        {
            if (!currentFlags.Contains(key))
            {
                keysToRemove.Add(key);
            }
        }

        if (!keysToRemove.Contains(thisScene))
        {
            keysToRemove.Add(thisScene);
        }

        foreach (var key in keysToRemove)
        {
            sceneAffection.Remove(key);
        }
        
        RecalculateTotalAffection();
    }
}

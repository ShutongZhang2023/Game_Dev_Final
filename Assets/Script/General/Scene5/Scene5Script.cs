using UnityEngine;

public class Scene5Script : MonoBehaviour
{

    void Awake()
    {
        StoryState.onFlagUpdated += ChoiceSelected;
    }

    void Update()
    {

    }

    void OnDestroy()
    {
        StoryState.onFlagUpdated -= ChoiceSelected;
    }

    public void ChoiceSelected(string flagName)
    {
        if (!flagName.Contains("Choice")) return;

        Debug.Log("Flag selected: " + flagName);

        if (flagName.Equals("Scene5Choice1"))
        {
            StoryState.instance.SetSceneAffection("Scene5", -3);
        }
        else if (flagName.Equals("Scene5Choice2"))
        {
            StoryState.instance.SetSceneAffection("Scene5", 1);
        }
        else if (flagName.Equals("Scene5Choice3"))
        {
            StoryState.instance.SetSceneAffection("Scene5", 3);
        }

        if (StoryState.instance.totalAffection > 10)
        {
            // good ending + meta space
        }
        //SceneManager.instance.ChangeContentScene("Scene5");
    }

}

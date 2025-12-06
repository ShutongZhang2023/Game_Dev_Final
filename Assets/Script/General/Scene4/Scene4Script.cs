using UnityEngine;

public class Scene4Script : MonoBehaviour
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

        if (flagName.Equals("Scene4Choice1"))
        {
            StoryState.instance.SetSceneAffection("Scene4", -2);
        }
        else if (flagName.Equals("Scene4Choice2"))
        {
            StoryState.instance.SetSceneAffection("Scene4", 0);
        }
        else if (flagName.Equals("Scene4Choice3"))
        {
            StoryState.instance.SetSceneAffection("Scene4", 2);
        }

        SceneManager.instance.ChangeContentScene("Scene5");
    }

}

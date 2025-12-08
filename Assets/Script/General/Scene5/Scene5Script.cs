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

        if (StoryState.instance.totalAffection <= 1)
        {
            StoryState.instance.SetFlag("BadEnd2");
            SceneManager.instance.ChangeContentScene("BadEnd");
        }
        else if(StoryState.instance.totalAffection <= 7)
        {
            SceneManager.instance.ChangeContentScene("NormalEnd");
        }
        else
        {
            SceneManager.instance.ChangeContentScene("GoodEnd");
        }
        //SceneManager.instance.ChangeContentScene("Scene5");
        StoryState.instance.SetFlag("Resolution");
    }

}

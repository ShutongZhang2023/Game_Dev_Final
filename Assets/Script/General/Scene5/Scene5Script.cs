using UnityEngine;

public class Scene5Script : MonoBehaviour
{

    public DialogueController dialogueController;

    void Awake()
    {
        StoryState.onFlagUpdated += ChoiceSelected;
        dialogueController.onDialogueEnd += onDialogueEnd;
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
    }

    public void onDialogueEnd()
    {
        if (StoryState.instance.totalAffection <= 1)
        {
            StoryState.instance.SetFlag("BadEnd2");
            SceneManager.instance.ChangeContentScene("BadEnd");
        }
        else if (StoryState.instance.totalAffection <= 7)
        {
            SceneManager.instance.ChangeContentScene("NormalEnd");
        }
        else
        {
            SceneManager.instance.ChangeContentScene("GoodEnd");
        }
        StoryState.instance.SetFlag("Resolution");
    }

}

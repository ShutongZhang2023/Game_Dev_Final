using UnityEngine;

public class Scene2_2Placeholder : MonoBehaviour
{
    public void SetClueB()
    {
        StoryState.instance.SetFlag("ClueB");
    }

    public void SetClueC()
    {
        StoryState.instance.SetFlag("ClueC");
    }

    public void nextScene() { 
        SceneManager.instance.ChangeContentScene("Scene3");
    }


}

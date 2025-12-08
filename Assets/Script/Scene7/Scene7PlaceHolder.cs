using UnityEngine;

public class Scene7PlaceHolder : MonoBehaviour
{
    public void TrueEnd()
    {
        StoryState.instance.SetFlag("Scene7Choice2");
        SceneManager.instance.ChangeContentScene("TrueEnd");
    }

    public void BadEnd()
    {
        StoryState.instance.SetFlag("Scene7Choice1");
        StoryState.instance.SetFlag("BadEnd3");
        SceneManager.instance.ChangeContentScene("BadEnd");
    }
}

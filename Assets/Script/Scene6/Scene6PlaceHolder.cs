using UnityEngine;

public class Scene6PlaceHolder : MonoBehaviour
{
    public string SceneName;
    public void nextScene()
    {
        SceneManager.instance.ChangeContentScene(SceneName);
    }
}

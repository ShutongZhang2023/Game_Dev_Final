using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;
    public string persistentSceneName = "Presist";
    public string firstSceneName = "Scene1";
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

    private void Start()
    {
        //add it later
    }

    public void ChangeContentScene(string sceneName)
    {
        StartCoroutine(LoadNewSceneAsync(sceneName));
    }

    private IEnumerator LoadNewSceneAsync(string newSceneName)
    {
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (scene.isLoaded && scene.name != persistentSceneName && scene.name != newSceneName)
            {
                yield return UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);
                StoryState.instance.SetFlag(newSceneName);
                break;
            }
        }
    }
}

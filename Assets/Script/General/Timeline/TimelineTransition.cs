using UnityEngine;
using UnityEngine.UI;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

public class TimelineTransition : MonoBehaviour
{
    public Image fadeImage;
    public float fadeOutDuration = 0.8f;
    public float fadeInDuration = 0.8f;
    public string sceneToLoad;

    bool isTransitioning = false;

    public void OnMemoryButtonClicked()
    {
        if (isTransitioning) return;
        if (fadeImage == null) return;
        if (string.IsNullOrEmpty(sceneToLoad)) return;

        StartCoroutine(FadeSequence());
    }

    System.Collections.IEnumerator FadeSequence()
    {
        isTransitioning = true;

        Color c = fadeImage.color;
        float t = 0f;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / fadeOutDuration);
            c.a = n;
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;

        if (SceneManager.instance != null)
        {
            SceneManager.instance.ChangeContentScene(sceneToLoad);
        }

        var targetScene = USceneManager.GetSceneByName(sceneToLoad);
        while (!targetScene.isLoaded)
        {
            targetScene = USceneManager.GetSceneByName(sceneToLoad);
            yield return null;
        }

        t = 0f;

        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / fadeInDuration);
            c.a = 1f - n;
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;

        isTransitioning = false;
    }
}

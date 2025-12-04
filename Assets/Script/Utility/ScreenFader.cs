using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader instance;

    public Image fadeImage;
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public Tween FadeIn() 
    {
        fadeImage.raycastTarget = true;
        return fadeImage.DOFade(1f, fadeDuration);
    }

    public Tween FadeOut()
    {
        return fadeImage.DOFade(0f, fadeDuration)
            .OnComplete(() => fadeImage.raycastTarget = false);
    }
}

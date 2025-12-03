using UnityEngine;
using UnityEngine.UI;

public class TestFadeClick : MonoBehaviour
{
    public Image fadeImage;

    public void OnTestClick()
    {
        Debug.Log("Button clicked");
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;
    }
}
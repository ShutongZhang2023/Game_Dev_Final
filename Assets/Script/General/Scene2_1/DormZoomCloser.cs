using UnityEngine;
using TMPro;

public class DormZoomCloser : MonoBehaviour
{
    public TextMeshProUGUI narrationText;

    public void OnClick()
    {
        if (narrationText != null) narrationText.text = "";
        gameObject.SetActive(false);
    }
}
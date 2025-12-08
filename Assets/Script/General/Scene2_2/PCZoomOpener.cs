using UnityEngine;
using UnityEngine.UI;

public class PCZoomToTarget : MonoBehaviour
{
    public RectTransform dormContainer;
    public RectTransform pcTarget;
    public GameObject pcPanel;
    public Button openPcButton;

    public float zoomDuration = 0.6f;
    public float zoomScale = 1.6f;

    Vector2 startPos;
    Vector3 startScale;
    bool running;

    public void OnClickOpenPC()
    {
        if (running) return;
        if (dormContainer == null || pcTarget == null) return;

        running = true;
        if (openPcButton != null) openPcButton.interactable = false;

        startPos = dormContainer.anchoredPosition;
        startScale = dormContainer.localScale;

        StartCoroutine(ZoomRoutine());
    }

    System.Collections.IEnumerator ZoomRoutine()
    {
        Vector2 screenCenter = Vector2.zero;
        Vector2 targetLocalPos = pcTarget.localPosition;
        Vector2 targetPos = -targetLocalPos * zoomScale;

        float t = 0f;

        while (t < zoomDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / zoomDuration);
            dormContainer.localScale = Vector3.Lerp(startScale, Vector3.one * zoomScale, n);
            dormContainer.anchoredPosition = Vector2.Lerp(startPos, targetPos, n);
            yield return null;
        }

        dormContainer.localScale = Vector3.one * zoomScale;
        dormContainer.anchoredPosition = targetPos;

        pcPanel.SetActive(true);
        if (openPcButton != null) openPcButton.gameObject.SetActive(false);
    }
}

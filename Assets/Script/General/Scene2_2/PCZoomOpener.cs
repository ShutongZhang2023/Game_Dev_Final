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

    Vector2 originalPos;
    Vector3 originalScale;
    bool running;
    bool zoomedIn;

    void Awake()
    {
        if (dormContainer != null)
        {
            originalPos = dormContainer.anchoredPosition;
            originalScale = dormContainer.localScale;
        }
    }

    public void OnClickOpenPC()
    {
        if (running) return;
        if (zoomedIn) return;
        if (dormContainer == null || pcTarget == null || pcPanel == null) return;

        running = true;
        if (openPcButton != null) openPcButton.interactable = false;

        StartCoroutine(ZoomInRoutine());
    }

    public void OnClickClosePC()
    {
        if (running) return;
        if (!zoomedIn) return;
        if (dormContainer == null) return;

        running = true;
        if (pcPanel != null) pcPanel.SetActive(false);

        StartCoroutine(ZoomOutRoutine());
    }

    System.Collections.IEnumerator ZoomInRoutine()
    {
        Vector2 targetLocalPos = pcTarget.localPosition;
        Vector2 targetPos = -targetLocalPos * zoomScale;

        Vector2 startPos = dormContainer.anchoredPosition;
        Vector3 startScale = dormContainer.localScale;
        Vector3 endScale = Vector3.one * zoomScale;

        float t = 0f;

        while (t < zoomDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / zoomDuration);
            dormContainer.localScale = Vector3.Lerp(startScale, endScale, n);
            dormContainer.anchoredPosition = Vector2.Lerp(startPos, targetPos, n);
            yield return null;
        }

        dormContainer.localScale = endScale;
        dormContainer.anchoredPosition = targetPos;

        if (pcPanel != null) pcPanel.SetActive(true);

        zoomedIn = true;
        running = false;
        if (openPcButton != null) openPcButton.gameObject.SetActive(false);
    }

    System.Collections.IEnumerator ZoomOutRoutine()
    {
        Vector2 startPos = dormContainer.anchoredPosition;
        Vector3 startScale = dormContainer.localScale;

        float t = 0f;

        while (t < zoomDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / zoomDuration);
            dormContainer.localScale = Vector3.Lerp(startScale, originalScale, n);
            dormContainer.anchoredPosition = Vector2.Lerp(startPos, originalPos, n);
            yield return null;
        }

        dormContainer.localScale = originalScale;
        dormContainer.anchoredPosition = originalPos;

        zoomedIn = false;
        running = false;

        if (openPcButton != null)
        {
            openPcButton.gameObject.SetActive(true);
            openPcButton.interactable = true;
        }
    }
}

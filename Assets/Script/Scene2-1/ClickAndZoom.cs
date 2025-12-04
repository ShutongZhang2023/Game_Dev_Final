using System.Collections;
using UnityEngine;

public class ClickAndZoom : Clickable
{
    public GameObject Dialogue;
    public float duration = 1.0f;
    public float zoomSize = 2.0f;

    private bool isZoomedIn = false;
    private bool isAnimating = false;

    protected override void OnClicked()
    {
        base.OnClicked();
        if (isAnimating) return;
        StartCoroutine(AfterClickRoutine());
    }

    private IEnumerator AfterClickRoutine()
    {
        isAnimating = true;

        if (!isZoomedIn)
        {
            CameraController.instance.ZoomIn(this.transform, zoomSize, duration);
            yield return new WaitForSeconds(duration); 
            Dialogue.SetActive(true);
        }
        else
        {
            Dialogue.SetActive(false);
            CameraController.instance.ZoomOut(duration);
            yield return new WaitForSeconds(duration);
        }

        isZoomedIn = !isZoomedIn;
        isAnimating = false;
    }
}

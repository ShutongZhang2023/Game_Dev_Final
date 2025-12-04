using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickAndZoom : Clickable
{
    public GameObject Dialogue;
    public float duration = 1.0f;
    public float zoomSize = 2.0f;
    public bool playSoundOnZoom = false;
    public Scene2_1Manager Scene2Manager;

    private bool isZoomedIn = false;
    private bool isAnimating = false;

    private bool hasSentSignal = false;


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
            if (playSoundOnZoom)
            {
                AudioManager.Instance.PlayWindSFX();
            }

            yield return new WaitForSeconds(duration); 
            Dialogue.SetActive(true);
        }
        else
        {
            Dialogue.SetActive(false);
            CameraController.instance.ZoomOut(duration);
            yield return new WaitForSeconds(duration);
            if (!hasSentSignal)
            {
                Scene2Manager.ReceiveSignal();
                hasSentSignal = true;
            }
        }

        isZoomedIn = !isZoomedIn;
        isAnimating = false;
    }
}

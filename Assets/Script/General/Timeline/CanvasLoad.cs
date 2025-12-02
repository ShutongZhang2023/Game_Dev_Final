using UnityEngine;

public class CanvasLoad : MonoBehaviour
{
    public GameObject targetCanvas;

    public void ToggleCanvas()
    {
        if (targetCanvas != null)
        {
            bool currentState = targetCanvas.activeSelf;
            targetCanvas.SetActive(!currentState);
        }
    }
}

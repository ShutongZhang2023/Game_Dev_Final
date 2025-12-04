using UnityEngine;

public class CanvasLoad : MonoBehaviour
{
    public static CanvasLoad instance;
    public GameObject targetCanvas;
    public GameObject timelineButton;

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

    public void ToggleCanvas()
    {
        if (targetCanvas != null)
        {
            bool currentState = targetCanvas.activeSelf;
            targetCanvas.SetActive(!currentState);
        }
    }
}

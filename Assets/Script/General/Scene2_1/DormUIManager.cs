using UnityEngine;

public class DormUIManager : MonoBehaviour
{
    public GameObject dialogueControllerObject;
    public GameObject[] buttons;

    bool waitingForIntroEnd = true;

    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != null) buttons[i].SetActive(false);
        }
    }

    void Update()
    {
        if (!waitingForIntroEnd) return;
        if (dialogueControllerObject == null) return;

        if (!dialogueControllerObject.activeSelf)
        {
            waitingForIntroEnd = false;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] != null) buttons[i].SetActive(true);
            }
        }
    }
}

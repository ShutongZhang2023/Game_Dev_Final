using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class BadEnd : MonoBehaviour
{
    public GameObject Dialogue;
    public TextMeshProUGUI tmpText;
    public Image backgroundImage;

    private void Start()
    {
        tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, 0f);
        Dialogue.SetActive(true);
        Dialogue.GetComponent<DialogueController>().onDialogueEnd += EndBadEnd;
    }

    private void EndBadEnd()
    {
        backgroundImage.DOFade(0f, 2f);
        tmpText.DOFade(1f, 3f);
    }
}

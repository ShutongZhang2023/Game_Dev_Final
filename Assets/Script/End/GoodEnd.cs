using UnityEngine;
using TMPro;
using DG.Tweening;

public class GoodEnd : MonoBehaviour
{
    public GameObject Dialogue;
    public TextMeshProUGUI tmpText;

    private void Start()
    {
        tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, 0f);
        Dialogue.SetActive(true);
        Dialogue.GetComponent<DialogueController>().onDialogueEnd += EndBadEnd;
    }

    private void EndBadEnd()
    {
        tmpText.DOFade(1f, 3f).OnComplete(() =>
        {
            SceneManager.instance.ChangeContentScene("Scene6");
        });
    }
}

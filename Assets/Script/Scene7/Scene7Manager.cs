using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Scene7Manager : MonoBehaviour
{
    public GameObject character;
    public GameObject notebook, dialogue1, dialogue2;
    private bool notebookClicked = false;
    private void Start()
    {
        CanvasLoad.instance.timelineButton.SetActive(false);
        character.SetActive(true);
        notebook.SetActive(false);
        dialogue1.SetActive(true);
        dialogue2.SetActive(false);

        SpriteRenderer sr = character.GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        sr.DOFade(1f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            dialogue1.SetActive(true);
            dialogue1.GetComponent<DialogueController>().onDialogueEnd += EndConversation;
        });
    }

    private void EndConversation()
    {
        // Fade out character
        SpriteRenderer cr = character.GetComponent<SpriteRenderer>();
        cr.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            character.SetActive(false);
        });
        // Show the notebook with fade-in effect
        notebook.SetActive(true);
        SpriteRenderer sr = notebook.GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        sr.DOFade(1f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            nextScene();
        });
    }

    private void nextScene()
    {
        StartCoroutine(WaitForTrueEnd());
    }

    private IEnumerator WaitForTrueEnd()
    {
        float timer = 10f;

        while (timer > 0f)
        {
            if (notebookClicked)
                yield break;

            timer -= Time.deltaTime;
            yield return null;
        }


        dialogue2.SetActive(true);
        dialogue2.GetComponent<DialogueController>().onDialogueEnd += EndConversation2;
    }

    public void OnNotebookClicked()
    {
        notebookClicked = true;
    }

    private void EndConversation2() { 
        SceneManager.instance.ChangeContentScene("TrueEnd");
    }
}

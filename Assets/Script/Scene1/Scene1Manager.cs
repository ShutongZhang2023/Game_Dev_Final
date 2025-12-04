using UnityEngine;
using DG.Tweening;

public class Scene1Manager : MonoBehaviour
{
    public GameObject d1, d2, character, notebook;

    void Start()
    {
        StoryState.instance.SetFlag("Scene1");
        StoryState.onFlagUpdated += ChoiceSelected;
        character.SetActive(true);
        notebook.SetActive(false);
        if (!StoryState.instance.passRound1)
        {
            d1.SetActive(true);
            d1.GetComponent<DialogueController>().onDialogueEnd += EndConversation;
        }
        else
        {
            d2.SetActive(true);
            d2.GetComponent<DialogueController>().onDialogueEnd += EndConversation;
        }

        startConversation();
    }

    public void ChoiceSelected(string flagName)
    {
        if (flagName.Equals("Scene1Choice1"))
        {
            StoryState.instance.SetSceneAffection("Scene1", -1);
        }
        else if (flagName.Equals("Scene1Choice3"))
        {
            StoryState.instance.SetSceneAffection("Scene1", 1);
        }
    }

    private void startConversation()
    {
        SpriteRenderer sr = character.GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        sr.DOFade(1f, 1f).SetEase(Ease.OutQuad);
    }

    private void EndConversation() {
        // Fade out character
        SpriteRenderer cr = character.GetComponent<SpriteRenderer>();
        cr.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            character.SetActive(false);
        });

        if (!StoryState.instance.passRound1)
        {
            // Show the notebook with fade-in effect
            notebook.SetActive(true);
            SpriteRenderer sr = notebook.GetComponent<SpriteRenderer>();
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
            sr.DOFade(1f, 1f).SetEase(Ease.OutQuad);
        }
        else { 
            if (StoryState.instance.HasFlag("Scene1Choice3"))
                SceneManager.instance.ChangeContentScene("Scene2-2");
            else
                SceneManager.instance.ChangeContentScene("Scene2-1");
        }
    }
}

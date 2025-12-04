using UnityEngine;
using DG.Tweening;

public class Scene2_1Manager : MonoBehaviour
{
    public GameObject d1, d2, character;
    private int receivedSignals = 0;
    private int totalSignalsNeeded = 4;
    private SpriteRenderer characterSpriteRenderer;

    private void Awake()
    {
        d1.SetActive(false);
        d2.SetActive(false);
        character.SetActive(false);
        characterSpriteRenderer = character.GetComponent<SpriteRenderer>();

    }
    private void Start()
    {
        d1.SetActive(true);
    }

    public void ReceiveSignal()
    {
        receivedSignals++;

        if (receivedSignals >= totalSignalsNeeded)
        {
            OnAllSignalsReceived();
        }
    }

    private void OnAllSignalsReceived()
    {
        character.SetActive(true);
        characterSpriteRenderer.color = new Color(characterSpriteRenderer.color.r, characterSpriteRenderer.color.g, characterSpriteRenderer.color.b, 0);
        characterSpriteRenderer.DOFade(1f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            d2.SetActive(true);
            d2.GetComponent<DialogueController>().onDialogueEnd += EndConversation;
        });
    }

    private void EndConversation()
    {
        // Fade out character
        characterSpriteRenderer.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            character.SetActive(false);
            StoryState.instance.SetFlag("BadEnd1");
            StoryState.instance.passRound1 = true;
            SceneManager.instance.ChangeContentScene("BadEnd");
        });
    }
}

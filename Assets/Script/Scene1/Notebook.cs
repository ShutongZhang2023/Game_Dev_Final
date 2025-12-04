using UnityEngine;
using DG.Tweening;

public class Notebook : Clickable
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnClicked()
    {
        SpriteRenderer book = GetComponent<SpriteRenderer>();
        book.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            StoryState.instance.activeTimeline = true;
            CanvasLoad.instance.timelineButton.SetActive(true);

            SceneManager.instance.ChangeContentScene("Scene2-1");
            gameObject.SetActive(false);
        });
    }
}

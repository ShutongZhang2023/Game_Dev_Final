using DG.Tweening;
using UnityEngine;

public class Scene7Notebook : Clickable
{
    public Scene7Manager scene7Manager;
    protected override void OnClicked()
    {
        scene7Manager.OnNotebookClicked();

        SpriteRenderer book = GetComponent<SpriteRenderer>();
        book.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            CanvasLoad.instance.timelineButton.SetActive(true);

            StoryState.instance.SetFlag("Scene7Choice1");
            StoryState.instance.SetFlag("BadEnd3");
            SceneManager.instance.ChangeContentScene("BadEnd");
            gameObject.SetActive(false);
        });
    }
}

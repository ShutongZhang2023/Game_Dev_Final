using UnityEngine;
using DG.Tweening;

public class Notebook : Clickable
{
    private void Awake()
    {
        base.Awake();
    }
    protected override void OnClicked()
    {
        SpriteRenderer book = GetComponent<SpriteRenderer>();
        book.enabled = false;
        book.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            SceneManager.instance.ChangeContentScene("Scene2-1");
            gameObject.SetActive(false);
        });
    }
}

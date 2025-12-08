using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoffeeCup : Clickable
{

    public GameObject choicePanel;
    public SpriteRenderer secondBackgroundSpriteRenderer;

    protected override void OnClicked()
    {
        base.OnClicked();
        choicePanel.SetActive(true);
        secondBackgroundSpriteRenderer.DOColor(new Color(1, 1, 1, 1), 1f);

        gameObject.SetActive(false);
    }

}

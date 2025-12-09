using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lightbulb : Clickable
{

    public GameObject choicePanel;

    protected override void OnClicked()
    {
        base.OnClicked();
        choicePanel.SetActive(true);
        gameObject.SetActive(false);
    }

}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoffeeCup : Clickable
{

    public GameObject choicePanel;

    protected override void OnClicked()
    {
        base.OnClicked();
        choicePanel.SetActive(true);
    }

}

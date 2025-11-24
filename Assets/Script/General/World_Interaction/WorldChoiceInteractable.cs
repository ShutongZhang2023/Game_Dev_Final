using UnityEngine;

public class WorldChoiceInteractable : MonoBehaviour
{
    public string choiceId;
    public DialogueController controller;
    public virtual void TrySelect()
    {
        if (controller != null)
        {
            controller.OnWorldChoiceSelected(choiceId);
        }
    }
}

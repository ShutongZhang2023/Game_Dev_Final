using UnityEngine;

public class Choice_Click_In_World : WorldChoiceInteractable
{
    private void OnMouseDown()
    {
        TrySelect();
    }
}

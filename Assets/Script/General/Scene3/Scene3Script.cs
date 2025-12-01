using System.Collections;
using TMPro;
using UnityEngine;

public class Scene3Script : MonoBehaviour
{

    public GameObject choicePanel, herChat1, myChat1, myChatObj, herChat2;
    public TextMeshProUGUI herChatText1, myChatText1, myChatText, herChatText2;

    void Start()
    {
        StoryState.onFlagUpdated += ChoiceSelected;
        StartCoroutine(ShowMessagesCo());
    }


    void Update()
    {
        
    }

    IEnumerator ShowMessagesCo()
    {
        yield return new WaitForSeconds(1f);

        herChat1.SetActive(true);
        yield return StartCoroutine(TypeTextCoroutine(herChatText1, "I'm under a lot of pressure today... Could we go for a walk together later?"));
        yield return new WaitForSeconds(0.5f);

        myChat1.SetActive(true);
        yield return StartCoroutine(TypeTextCoroutine(myChatText1, "Yeah sure."));
        yield return new WaitForSeconds(0.5f);

        choicePanel.SetActive(true);
    }

    private IEnumerator TypeTextCoroutine(TextMeshProUGUI tmp, string fullText)
    {
        tmp.text = "...";
        yield return new WaitForSeconds(0.75f);

        tmp.text = "";

        foreach (char c in fullText)
        {
            tmp.text += c;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void ChoiceSelected(string flagName)
    {
        Debug.Log("Flag selected: " + flagName);

        string text = "?";
        if (flagName.Equals("Scene3Choice1"))
        {
            text = "She's so sensitive...";
        }
        else if (flagName.Equals("Scene3Choice2"))
        {
            text = "Maybe I was just not feeling it on that day...";
        } 
        else if (flagName.Equals("Scene3Choice3"))
        {
            text = "I didn't even ask her feelings on that day.";
        }

        StartCoroutine(ShowReplyMessagesCo(text));
    }

    IEnumerator ShowReplyMessagesCo(string text)
    {
        yield return new WaitForSeconds(1f);

        myChatObj.SetActive(true);
        yield return StartCoroutine(TypeTextCoroutine(myChatText, text));

        herChat2.SetActive(true);
        yield return StartCoroutine(TypeTextCoroutine(herChatText2, "..."));
    }

}

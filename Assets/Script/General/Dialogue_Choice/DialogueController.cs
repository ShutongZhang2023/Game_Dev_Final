using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;


public class DialogueController : MonoBehaviour
{
    [Header("Data")]
    public DialogueAsset dialogueAsset;
    public string startNodeId = "start";
    public StoryState storyState;

    [Header("UI")]
    public TextMeshProUGUI contentText;
    public Transform choicesContainer;
    public GameObject choiceButtonPrefab;

    [Header("Typewriter")]
    public float typeSpeed = 0.03f;

    private DialogueNode currentNode;
    private Coroutine typingCoroutine;
    private bool textFinished;
    private bool waitingForChoice;

    private void Start()
    {
        StartDialogue(dialogueAsset, startNodeId);
    }

    // Start a dialogue from a specific node, can access from other script
    public void StartDialogue(DialogueAsset asset, string nodeId)
    {
        dialogueAsset = asset;
        ShowNode(nodeId);
        gameObject.SetActive(true);
    }

    private void ShowNode(string nodeId)
    {
        ClearChoices();
        waitingForChoice = false;

        currentNode = dialogueAsset.GetNodeById(nodeId);

        if (currentNode == null) {
            EndDialogue();
            return;
        }

        switch (currentNode.speaker) {
            case "narrator":
                contentText.color = Color.white;
                break;
            case "boy":
                contentText.color = Color.blue;
                break;
            case "girl":
                contentText.color = Color.magenta;
                break;
            default:
                contentText.color = Color.white;
                break;
        }

        //extend this if there is nothe node type
        switch (currentNode.nodeType) {
            case DialogueNodeType.Dialogue:
                ShowDialogueNode();
                break;
            case DialogueNodeType.Choice:
                ShowChoiceNode();
                break;
        }


    }

    private void EndDialogue() {
        gameObject.SetActive(false);
    }

    //use for dialogue node
    private void ShowDialogueNode()
    {
        string text = currentNode.content ?? "";
        StartTypewriter(text);
    }

    private void StartTypewriter(string fullText) {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeTextCoroutine(fullText));
    }

    private IEnumerator TypeTextCoroutine(string fullText)
    {
        textFinished = false;

        contentText.text = "";

        foreach (char c in fullText)
        {
            contentText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        textFinished = true;
    }

    //click to jump/continue, can access from UI button
    public void OnClickDialogueArea()
    {
        if (currentNode == null)
            return;

        if (currentNode.nodeType != DialogueNodeType.Dialogue)
            return;

        //if not finish typing, finish it
        if (!textFinished)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            contentText.text = currentNode.content;
            textFinished = true;
            return;
        }

        //if finish, move to next node
        if (!string.IsNullOrEmpty(currentNode.defaultNextNodeId))
        {
            ShowNode(currentNode.defaultNextNodeId);
        }
        else
        {
            EndDialogue();
        }
    }

    //use for choice node
    private void ShowChoiceNode()
    {
        textFinished = true;
        contentText.text = "";
        ShowChoicesImmediately();
    }

    private void ShowChoicesImmediately()
    {
        waitingForChoice = true;
        foreach (var choice in currentNode.choices)
        {
            //check needFlag
            if (!string.IsNullOrEmpty(choice.needFlag))
            {
                if (!storyState.HasFlag(choice.needFlag))
                    continue;
            }
            // for world choice with empty text
            if (string.IsNullOrEmpty(choice.text))
                continue;

            GameObject btnObj = Object.Instantiate(choiceButtonPrefab, choicesContainer);
            TextMeshProUGUI txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            Button btn = btnObj.GetComponent<Button>();

            txt.text = choice.text;
            ChoiceData capturedChoice = choice;
            btn.onClick.AddListener(() =>
            {
                OnChoiceSelected(capturedChoice);
            });
        }
    }

    public void OnChoiceSelected(ChoiceData choice)
    {
        waitingForChoice = false;

        if (!string.IsNullOrEmpty(choice.setFlag))
        {
            storyState.SetFlag(choice.setFlag);
        }

        if (choice.onChosen != null)
        {
            choice.onChosen.Invoke();
        }

        if (!string.IsNullOrEmpty(choice.nextNodeId))
        {
            ShowNode(choice.nextNodeId);
        }
        else
        {
            EndDialogue();
        }
    }

    private void ClearChoices()
    {
        for (int i = choicesContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(choicesContainer.GetChild(i).gameObject);
        }
    }


    // For world choice selection from world (not UI)
    public void OnWorldChoiceSelected(string choiceId)
    {
        if (currentNode == null)
            return;

        if (currentNode.nodeType != DialogueNodeType.Choice)
            return;

        foreach (var c in currentNode.choices)
        {
            if (c.choiceId == choiceId)
            {
                OnChoiceSelected(c);
                return;
            }
        }

        Debug.LogWarning("World choiceId not found in current node: " + choiceId);
    }

}

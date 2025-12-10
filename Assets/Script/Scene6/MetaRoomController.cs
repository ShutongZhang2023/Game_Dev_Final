using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MetaRoomController : MonoBehaviour
{
    public DialogueController dialogueController;
    public DialogueAsset introAsset;
    public DialogueAsset bubbleAsset;
    public DialogueAsset finalAsset;

    public string introNodeId = "start";
    public string finalNodeId = "start";

    public GameObject girlAvatar;

    int poppedCount;
    bool locked;
    bool finalStarted;

    void Start()
    {
        if (girlAvatar != null) girlAvatar.SetActive(false);
        dialogueController.onDialogueEnd += OnDialogueEnd;
        dialogueController.gameObject.SetActive(true);
        dialogueController.StartDialogue(introAsset, introNodeId);
    }

    public void OnBubblePopped(MemoryBubble bubble)
    {
        if (locked) return;
        locked = true;
        poppedCount++;

        dialogueController.gameObject.SetActive(true);
        dialogueController.StartDialogue(bubbleAsset, bubble.dialogueNodeId);
    }

    void OnDialogueEnd()
    {
        locked = false;

        if (!finalStarted && poppedCount >= FindObjectsOfType<MemoryBubble>(true).Length)
        {
            finalStarted = true;
            StartCoroutine(PlayFinal());
        }
    }

    IEnumerator PlayFinal()
    {
        locked = true;
        if (girlAvatar != null) girlAvatar.SetActive(true);
        yield return new WaitForSeconds(0.3f);

        dialogueController.gameObject.SetActive(true);
        dialogueController.StartDialogue(finalAsset, finalNodeId);

        dialogueController.onDialogueEnd = LoadNextScene;
    }

    void LoadNextScene()
    {
        SceneManager.instance.ChangeContentScene("Scene7");
    }
}

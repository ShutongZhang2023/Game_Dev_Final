using UnityEngine;
using UnityEngine.UI;

public class DormObjectClickWithZoom : MonoBehaviour
{
    public DialogueController dialogueController;
    public DialogueAsset dialogueAsset;
    public string nodeId;

    public GameObject zoomPanel;
    public Image zoomImage;
    public Sprite zoomSprite;

    public AudioClip windClip;
    public float windVolume = 0.8f;
    private AudioSource audioSource;

    void Start()
    {
        if (zoomPanel != null) zoomPanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    public void OnClick()
    {
        if (zoomPanel != null)
        {
            zoomPanel.SetActive(true);
            if (zoomImage != null)
            {
                if (zoomSprite != null)
                {
                    zoomImage.sprite = zoomSprite;
                    zoomImage.enabled = true;
                }
                else zoomImage.enabled = false;
            }
        }

        if (windClip != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(windClip, windVolume);
        }

        if (dialogueController != null)
        {
            dialogueController.gameObject.SetActive(true); 
            dialogueController.StartDialogue(dialogueAsset, nodeId);
        }

        gameObject.SetActive(false);
    }
}

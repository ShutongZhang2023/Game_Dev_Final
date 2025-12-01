using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DormSceneSimple : MonoBehaviour
{
    public TextMeshProUGUI narrationText;
    public GameObject zoomPanel;
    public Image zoomImage;
    public Sprite bedSprite;
    public Sprite deskSprite;
    public Sprite trashSprite;
    public Sprite curtainSprite;

    public Button bedButton;
    public Button deskButton;
    public Button trashButton;
    public Button curtainButton;

    public AudioClip windClip;
    public float windVolume = 0.8f;
    private AudioSource audioSource;

    void Start()
    {
        if (zoomPanel != null) zoomPanel.SetActive(false);
        if (narrationText != null) narrationText.text = "";

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    public void OnClickBed()
    {
        bedButton.gameObject.SetActive(false);
        ShowZoom(bedSprite, "There¡¯s nothing here.", false);
    }

    public void OnClickDesk()
    {
        deskButton.gameObject.SetActive(false);
        ShowZoom(deskSprite, "Has she been pushing herself too hard lately?", false);
    }

    public void OnClickTrash()
    {
        trashButton.gameObject.SetActive(false);
        ShowZoom(trashSprite, "If only he would ask me how my day was¡­ even just once.", false);
    }

    public void OnClickCurtain()
    {
        curtainButton.gameObject.SetActive(false);
        ShowZoom(curtainSprite, "She must feel really lonely¡­", true);
    }

    public void OnClickCloseZoom()
    {
        if (zoomPanel != null) zoomPanel.SetActive(false);
        if (narrationText != null) narrationText.text = "";
    }

    void ShowZoom(Sprite sprite, string text, bool playWind)
    {
        if (zoomPanel != null) zoomPanel.SetActive(true);

        if (zoomImage != null)
        {
            if (sprite != null)
            {
                zoomImage.sprite = sprite;
                zoomImage.enabled = true;
            }
            else
            {
                zoomImage.enabled = false;
            }
        }

        if (narrationText != null) narrationText.text = text;

        if (playWind && windClip != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(windClip, windVolume);
        }
    }
}

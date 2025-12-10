using UnityEngine;
using UnityEngine.EventSystems;

public class MemoryBubble : MonoBehaviour, IPointerClickHandler
{
    public string dialogueNodeId;
    public MetaRoomController metaRoom;

    public float floatAmplitude = 0.2f;
    public float floatSpeed = 1f;
    public float tiltAmplitude = 5f;

    public float growPerClick = 0.3f;
    public float maxScale = 1.8f;
    public float shrinkSpeed = 1.2f;

    Vector3 basePos;
    Vector3 baseScale;
    float growth;
    bool popped;

    void Awake()
    {
        basePos = transform.localPosition;
        baseScale = transform.localScale;
    }

    void Update()
    {
        if (popped) return;

        float t = Time.time;
        transform.localPosition = basePos + Vector3.up * Mathf.Sin(t * floatSpeed) * floatAmplitude;
        transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(t * 0.7f) * tiltAmplitude);

        growth = Mathf.Max(0f, growth - shrinkSpeed * Time.deltaTime);
        float scale = Mathf.Lerp(1f, maxScale, growth);
        transform.localScale = baseScale * scale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (popped) return;
        growth += growPerClick;

        if (growth >= 1f)
            Pop();
    }

    void Pop()
    {
        popped = true;
        transform.localScale = baseScale * maxScale;
        metaRoom.OnBubblePopped(this);
        gameObject.SetActive(false);
    }
}

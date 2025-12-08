using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Outline Settings")]
    public string thicknessProperty = "_OutlineThickness";
    public float normalThickness = 0f; 
    public float hoverThickness = 0.005f;
    public float fadeDuration = 0.5f;

    protected Material materialInstance;

    protected virtual void Awake()
    {
        var sr = GetComponent<SpriteRenderer>();
        materialInstance = sr.material;
        materialInstance.SetFloat(thicknessProperty, normalThickness);
        materialInstance.SetTexture("_MainTex", sr.sprite.texture);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        materialInstance?.DOFloat(hoverThickness, thicknessProperty, fadeDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        materialInstance?.DOFloat(normalThickness, thicknessProperty, fadeDuration);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked();
    }

    protected virtual void OnClicked()
    {
        Debug.Log($"Clicked on {gameObject.name}");
    }
}

using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    private Camera cam;

    private Vector3 originalPosition;
    private float originalSize;

    private Tweener moveTween;
    private Tweener zoomTween;

    void Awake()
    {
        instance = this;
        cam = GetComponent<Camera>();
        originalPosition = transform.position;
        originalSize = cam.orthographicSize;
    }

    public void ZoomIn(Transform target, float zoomSize, float duration)
    {
        moveTween?.Kill();
        zoomTween?.Kill();

        Vector3 targetPos = new Vector3(target.position.x, target.position.y, originalPosition.z);

        moveTween = transform.DOMove(targetPos, duration)
                             .SetEase(Ease.OutQuad);

        zoomTween = DOTween.To(() => cam.orthographicSize,
                               value => cam.orthographicSize = value,
                               zoomSize,
                               duration)
                           .SetEase(Ease.OutQuad);
    }

    public void ZoomOut(float duration)
    {
        moveTween?.Kill();
        zoomTween?.Kill();

        moveTween = transform.DOMove(originalPosition, duration)
                             .SetEase(Ease.OutQuad);

        zoomTween = DOTween.To(() => cam.orthographicSize,
                               value => cam.orthographicSize = value,
                               originalSize,
                               duration)
                           .SetEase(Ease.OutQuad);
    }
}

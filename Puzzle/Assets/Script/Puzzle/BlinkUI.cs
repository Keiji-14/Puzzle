using UnityEngine;
using DG.Tweening;

public class BlinkUI : MonoBehaviour
{
    [SerializeField] private float blinkIntervalTime = 1.5f;

    [SerializeField] private Ease easeType = Ease.Linear;

    void Start()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup
            .DOFade(0.0f, blinkIntervalTime)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Yoyo);
    }
}

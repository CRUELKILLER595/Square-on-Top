using UnityEngine;
using UnityEngine.EventSystems;

public class PulsingButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    public float pulseSpeed = 0.6f;
    public float maxScale = 1.15f;
    public float minScale = 0.95f;

    private RectTransform rt;
    private Vector3 originalScale;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        originalScale = rt.localScale;

        StartPulse();
    }

    // 🔁 Idle Pulse (Subway Surfers style)
    void StartPulse()
    {
        LeanTween.scale(rt, originalScale * maxScale, pulseSpeed)
            .setEaseInOutSine()
            .setLoopPingPong();
    }

    // 🖱️ Hover → Slight grow
    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.cancel(rt);

        LeanTween.scale(rt, originalScale * 1.2f, 0.15f)
            .setEaseOutBack();
    }

    // 🖱️ Exit → Back to pulse
    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(rt);
        StartPulse();
    }

    // 👇 Press → shrink (click feel)
    public void OnPointerDown(PointerEventData eventData)
    {
        LeanTween.cancel(rt);

        LeanTween.scale(rt, originalScale * minScale, 0.1f)
            .setEaseInOutSine();
    }

    // 👆 Release → bounce back
    public void OnPointerUp(PointerEventData eventData)
    {
        LeanTween.scale(rt, originalScale * 1.1f, 0.15f)
            .setEaseOutBack()
            .setOnComplete(() =>
            {
                StartPulse();
            });
    }
}
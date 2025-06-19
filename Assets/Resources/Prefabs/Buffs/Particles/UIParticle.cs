using UnityEngine;
using UnityEngine.UI;

public class UIParticle : MonoBehaviour
{
    public RectTransform rectTransform;
    public CanvasGroup canvasGroup;

    private Vector2 startPos;
    private Vector2 endPos;
    private float duration;
    private float time;
    private System.Action<UIParticle> onComplete;

    public void Init(Vector2 position, float riseDistance, float duration, System.Action<UIParticle> onComplete)
    {
        this.startPos = position;
        this.endPos = position + new Vector2(0, riseDistance);
        this.duration = duration;
        this.time = 0;
        this.onComplete = onComplete;

        rectTransform.anchoredPosition = startPos;
        canvasGroup.alpha = 1f;
        gameObject.SetActive(true);
    }

    void Update()
    {
        time += Time.deltaTime;
        float t = time / duration;
        rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
        canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

        if (time >= duration)
        {
            onComplete?.Invoke(this);
        }
    }

    public void ResetParticle()
    {
        gameObject.SetActive(false);
    }
}
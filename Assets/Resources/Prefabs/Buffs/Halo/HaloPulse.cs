using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HaloPulse : MonoBehaviour
{
    public Image haloImage;
    public float pulseDuration = 1.5f;
    public float maxScale = 1.3f;
    public float minAlpha = 0.2f;
    public float maxAlpha = 1f;

    private void OnEnable()
    {
        StartPulse();
    }

    public void StartPulse()
    {
        haloImage.transform.localScale = Vector3.one;
        var color = haloImage.color;
        color.a = minAlpha;
        haloImage.color = color;

        Sequence seq = DOTween.Sequence();
        seq.Append(haloImage.transform.DOScale(maxScale, pulseDuration / 2).SetEase(Ease.OutQuad));
        seq.Join(haloImage.DOFade(maxAlpha, pulseDuration / 2));
        seq.Append(haloImage.transform.DOScale(1f, pulseDuration / 2).SetEase(Ease.InQuad));
        seq.Join(haloImage.DOFade(minAlpha, pulseDuration / 2));
        seq.SetLoops(-1, LoopType.Restart);
    }
}
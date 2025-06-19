using UnityEngine;
using System.Collections.Generic;

public class UIParticlePool : MonoBehaviour
{
    public GameObject particlePrefab;
    public RectTransform parent;
    public int initialSize = 20;

    private readonly List<UIParticle> pool = new();

    void Start()
    {
        for (int i = 0; i < initialSize; i++)
            CreateNewParticle();
    }

    private UIParticle CreateNewParticle()
    {
        GameObject go = Instantiate(particlePrefab, parent);
        UIParticle particle = go.AddComponent<UIParticle>();
        particle.rectTransform = go.GetComponent<RectTransform>();
        particle.canvasGroup = go.GetComponent<CanvasGroup>();
        go.SetActive(false);
        pool.Add(particle);
        return particle;
    }

    public UIParticle GetParticle()
    {
        foreach (var p in pool)
        {
            if (!p.gameObject.activeSelf)
                return p;
        }
        return CreateNewParticle(); // Extend pool if needed
    }

    public void ReturnToPool(UIParticle p)
    {
        p.ResetParticle();
    }
}

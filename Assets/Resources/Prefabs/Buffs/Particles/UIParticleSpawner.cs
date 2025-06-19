using UnityEngine;

public class UIParticleSpawner : MonoBehaviour
{
    public UIParticlePool particlePool;
    public Vector2 spawnCenter = new Vector2(0, 0);
    public float spawnRadius = 30f;
    public float riseDistance = 50f;
    public float duration = 1f;
    public float spawnInterval = 0.2f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnParticle();
            timer = 0f;
        }
    }

    private void SpawnParticle()
    {
        UIParticle p = particlePool.GetParticle();
        Vector2 offset = Random.insideUnitCircle * spawnRadius;
        p.Init(spawnCenter + offset, riseDistance, duration, particlePool.ReturnToPool);
    }
}
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class Projectile : MonoBehaviour
{
    public Transform origin;
    public Transform target;
    public float lifeTime = 0f;
    public float targetDuration = 1f;
    public List<Effect> effects;
    public Context context;
    public Image image;

    public void Init(Context context, List<Effect> effects, Sprite sprite, float targetDuration)
    {
        this.context = context;
        this.effects = effects;
        origin = context.source.transform;
        target = context.target.transform;
        this.image.sprite = sprite;
        this.targetDuration = targetDuration;
        
        transform.position = origin.position;
        transform.up = target.position - transform.position;
    }
    public void Update()
    {
        lifeTime += Time.deltaTime;
        transform.position = Vector3.Lerp(origin.position,target.position,lifeTime/targetDuration);
        if (lifeTime >= targetDuration)
        {
            foreach (var effect in effects)
            {
                effect.Apply(context);
            }
            Destroy(gameObject);
        }
        
    }
}
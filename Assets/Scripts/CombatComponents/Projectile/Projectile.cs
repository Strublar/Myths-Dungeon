using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Projectile : MonoBehaviour
{
    public Transform origin;
    public Vector3 targetPosition;
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
        var targetTransform = context.target.transform;
        targetPosition = targetTransform.position+new Vector3(Random.Range(-200,200),Random.Range(-50,50));
        this.image.sprite = sprite;
        this.targetDuration = targetDuration;

        var thisTransform = transform;
        thisTransform.position = origin.position;
        thisTransform.up = targetPosition - thisTransform.position;
    }
    public void Update()
    {
        lifeTime += Time.deltaTime;
        transform.position = Vector3.Lerp(origin.position,targetPosition,lifeTime/targetDuration);
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
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
    public Vector3 originPosition;
    public Vector3 targetPosition;
    public float lifeTime = 0f;
    public float targetDuration = 1f;
    public List<Effect> effects;
    public Context context;
    public GameObject model;

    public void Init(Context context, List<Effect> effects, GameObject model, float targetDuration)
    {
        lifeTime = 0;
        this.context = context;
        this.effects = effects;
        this.model = Instantiate(model, transform);
        
        var uiCamera = GameManager.instance.mainCamera;
        Vector3 screenOrigin = RectTransformUtility.WorldToScreenPoint(uiCamera, context.source.transform.position);
        Vector3 screenTarget = RectTransformUtility.WorldToScreenPoint(uiCamera, context.target.transform.position);

        Vector3 worldOrigin = uiCamera.ScreenToWorldPoint(new Vector3(screenOrigin.x, screenOrigin.y, 1f));
        Vector3 worldTarget = uiCamera.ScreenToWorldPoint(new Vector3(screenTarget.x, screenTarget.y, 1f));

        if(context.target is Enemy) worldTarget += new Vector3(Random.Range(-1f, 1f), Random.Range(-.7f, 0.7f), 0f);

        this.context = context;
        this.effects = effects;
        this.targetDuration = targetDuration;

        transform.position = worldOrigin;
        originPosition = worldOrigin;
        targetPosition = worldTarget;
        transform.up = (targetPosition - transform.position).normalized;

    }
    public void Update()
    {
        lifeTime += Time.deltaTime;
        transform.position = Vector3.Lerp(originPosition,targetPosition,lifeTime/targetDuration);
        if (lifeTime >= targetDuration)
        {
            foreach (var effect in effects)
            {
                effect.Apply(context);
            }

            Destroy(model);
            ProjectileManager.instance.ReturnObject(this);
        }
        
    }
}
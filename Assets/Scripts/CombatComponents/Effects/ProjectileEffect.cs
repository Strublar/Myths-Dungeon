using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileEffect", menuName = "Effects/Projectile")]
public class ProjectileEffect : Effect
{
    public Sprite sprite;
    public float travelTime = 0.3f;
    public List<Effect> effects;

    public override void Apply(Context context)
    {
        if (context.source == context.target)
        {
            foreach (var effect in effects)
            {
                effect.Apply(context);
            }
        }
        else
        {
            Projectile projectile = Instantiate(ProjectileManager.instance.projectilePrefab,
                ProjectileManager.instance.container);
            projectile.Init(context, effects, sprite, travelTime);
        }
    }
}
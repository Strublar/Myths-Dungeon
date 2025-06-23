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
        Projectile projectile = Instantiate(ProjectileManager.instance.projectilePrefab,
            ProjectileManager.instance.container);
        projectile.Init(context, effects, sprite, travelTime);
    }
}
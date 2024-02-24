using UnityEngine;

public class ProjectilePool : ObjectPool<Projectile>
{
    //you need to activate the projectile by yourself after initializing it
    public override Projectile GetObjectFromPool(Vector2 InitialPosition)
    {
        Projectile Projectile = GetAvailableObject();
        Projectile.transform.position = InitialPosition;
       
        return GetAvailableObject();
    }
}

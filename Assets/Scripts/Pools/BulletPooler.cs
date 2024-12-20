using UnityEngine;

public class BulletPooler : ObjectPooler<BulletProjectileController>
{
    public static BulletPooler Instance { get; private set; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

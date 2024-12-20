using UnityEngine;

public class EnemyPooler : ObjectPooler<EnemyAI>
{
    public static EnemyPooler Instance { get; private set; }

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

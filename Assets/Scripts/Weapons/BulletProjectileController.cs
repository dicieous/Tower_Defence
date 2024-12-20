using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletProjectileController : MonoBehaviour
{
    public float Damage { get; set; }
    public float speed = 10f;
    public float lifetime = 5f;

    [SerializeField] private Transform vfxHitRed;
    
    private Timer _bulletLifetimeTimer;

    private void Start()
    {
        _bulletLifetimeTimer = new Timer(lifetime);
    }
    
    private void Update()
    {
        if (_bulletLifetimeTimer.UpdateAndCheck(Time.deltaTime))
        {
            RemoveBullet();
        }
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider _collider)
    {
        // Handle damage to enemy
        if (_collider.transform.TryGetComponent(out EnemyAI enemyAI))
        {
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(Damage);
                Instantiate(vfxHitRed, transform.position, Quaternion.identity);
                Debug.Log("AI Taking Damage");
            }
        }
        
        RemoveBullet();
    }
    
    private void RemoveBullet()
    {
        BulletPooler.Instance.ReturnObject(this);
    }
}

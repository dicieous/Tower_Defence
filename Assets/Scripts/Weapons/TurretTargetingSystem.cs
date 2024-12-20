using Unity.Mathematics;
using UnityEngine;

public class TurretTargetingSystem : MonoBehaviour
{
   [SerializeField] protected TurretsConfigSO turretConfigs;
   
   [Space(10)]
   [SerializeField] protected Transform turretBase;
   [SerializeField] protected Transform turretBarrel;
   [SerializeField] protected Transform firePoint;

   private float _lastFireTime = 0.0f;
   protected float _currentBarrelAngle = 0.0f;

   private Transform _targetEnemy;
   protected virtual void Start()
   {
      if (turretConfigs == null)
      {
         Debug.LogWarning("Turret Configuration is missing!");
         enabled = false;
      }
      
      _currentBarrelAngle = turretBarrel.localEulerAngles.x;
      if (_currentBarrelAngle > 180) _currentBarrelAngle -= 360;
   }
   
   protected virtual void Update()
   {
      FindTarget();
      if (_targetEnemy != null)
      {
         bool isBaseAligned = RotateBase();
         bool isBarrelAligned = AdjustBarrelElevation();
         
         if (isBaseAligned && isBarrelAligned)
         {
            TryShoot();
         }
      }
   }

   protected virtual void FindTarget()
   {
      Collider[] hitColliders = Physics.OverlapSphere(transform.position, turretConfigs.detectionRadius);
      float closestEnemyDist = Mathf.Infinity;
      Transform closestTarget = null;
      
      foreach (var hitCollider in hitColliders)
      {
         if (hitCollider.TryGetComponent(out EnemyAI enemyAI))
         {
            var distance = Vector3.Distance(transform.position, enemyAI.transform.position);
            if (distance < closestEnemyDist)
            {
               closestTarget= enemyAI.transform;
               closestEnemyDist = distance;
            }
         }
      }

      _targetEnemy = closestTarget;
   }

    protected virtual bool RotateBase()
    {
        Vector3 targetPositionFlat = new Vector3(_targetEnemy.position.x, turretBase.position.y, _targetEnemy.position.z);
        Vector3 directionToTarget = targetPositionFlat - turretBase.position;
        
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        
      
        turretBase.rotation = Quaternion.RotateTowards(
            turretBase.rotation,
            targetRotation,
            turretConfigs.baseRotationSpeed * Time.deltaTime
        );
        
        return Quaternion.Angle(turretBase.rotation, targetRotation) < 5f;
    }

    protected virtual bool AdjustBarrelElevation()
    {
        Vector3 targetLocalPos = turretBase.InverseTransformPoint(_targetEnemy.position);
        
       
        float targetElevation = -Mathf.Atan2(targetLocalPos.y, new Vector2(targetLocalPos.z, targetLocalPos.x).magnitude) * Mathf.Rad2Deg;
        
     
        targetElevation = Mathf.Clamp(targetElevation, turretConfigs.minElevationAngle, turretConfigs.maxElevationAngle);
        
       
        Vector3 currentRotation = turretBarrel.localEulerAngles;
        float currentAngle = currentRotation.x;
        if (currentAngle > 180) currentAngle -= 360; // Normalize angle
        
      
        float newAngle = Mathf.MoveTowards(currentAngle, targetElevation, turretConfigs.barrelRotationSpeed * Time.deltaTime);
        
       
        turretBarrel.localRotation = Quaternion.Euler(newAngle, 0, 0);
        
      
        _currentBarrelAngle = newAngle;
        
    
        return Mathf.Abs(newAngle - targetElevation) < 2f;
    }

    protected virtual void TryShoot()
    {
        if (Time.time - _lastFireTime > 1f / turretConfigs.fireRate)
        {
            Shoot();
            Debug.Log("TryShoot");
            _lastFireTime = Time.time;
        }
    }

    protected virtual void Shoot()
    {
        if (BulletPooler.Instance.PooledCount != 0)
        {
            Debug.Log("3");
            Vector3 spawnPosition = firePoint.position; 
            Quaternion spawnRotation = firePoint.rotation; 
            
            Debug.DrawRay(spawnPosition, firePoint.forward * 2f, Color.magenta, 2f);
            BulletProjectileController controller =  BulletPooler.Instance.GetObject(
                spawnPosition, 
               spawnRotation
            );
            
            if (controller != null)
            {
                controller.Damage = turretConfigs.damagePerShot;
            }
        }
    }

    /*protected virtual void OnDrawGizmos()
    {
        if (turretConfigs != null)
        {
            // Draw detection radius
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, turretConfigs.detectionRadius);
            
            if (_targetEnemy != null && turretBarrel != null)
            {
                // Draw line showing actual barrel direction
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(turretBarrel.position, turretBarrel.forward * 5f);
                
                // Draw line to target
                Gizmos.color = Color.red;
                Gizmos.DrawLine(turretBarrel.position, _targetEnemy.position);
                
                // Draw the allowed elevation range
                if (Application.isEditor && !Application.isPlaying)
                {
                    Gizmos.color = Color.green;
                    Vector3 minAngle = Quaternion.Euler(turretConfigs.minElevationAngle, 0, 0) * Vector3.forward * 3f;
                    Vector3 maxAngle = Quaternion.Euler(turretConfigs.maxElevationAngle, 0, 0) * Vector3.forward * 3f;
                    Gizmos.DrawRay(turretBarrel.position, minAngle);
                    Gizmos.DrawRay(turretBarrel.position, maxAngle);
                }
            }
        }
    }*/
}

using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Turret", fileName = "Turret")]
public class TurretsConfigSO : ScriptableObject
{
    public string turretName;
    public Transform turretPrefab;

    public float fireRate;
    public float detectionRadius;
    public float damagePerShot = 10f;
    public float baseRotationSpeed = 10f;
    public float barrelRotationSpeed = 10f;
    public float minElevationAngle = -45f;
    public float maxElevationAngle = 45f;
}

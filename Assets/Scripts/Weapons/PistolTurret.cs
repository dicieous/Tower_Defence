using UnityEngine;

public class PistolTurret : TurretTargetingSystem
{
    public WeaponList weapon;
    protected override void Start()
    {
        weapon = WeaponList.PistolTurret;
        base.Start();
    }
}

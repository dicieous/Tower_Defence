using UnityEngine;

public class MachineGunTurret : TurretTargetingSystem
{
    public WeaponList weapon;
    protected override void Start()
    {
        weapon = WeaponList.MachineGunTurret;
        base.Start();
    }
}

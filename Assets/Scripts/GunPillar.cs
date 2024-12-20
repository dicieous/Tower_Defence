using System;
using Unity.Mathematics;
using UnityEngine;

public class GunPillar : MonoBehaviour
{
    [SerializeField] private Outline outline;
    [SerializeField] private Transform gunSpawnPos;
    public bool isAGunAlreadyPlaced = false;
    public GameObject placedWeapon;


    private void Start()
    {
        outline.enabled = false;
    }

    private void OnMouseDown()
    {
        if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider == transform.GetComponent<Collider>()) // Check if this object is hit
                {
                    GameManager.Instance.GetClickedPillar(gameObject);
                    UIManager.Instance.ShowScreen(Screen.StorePopUp);
                }
            }
        }
    }
    
    private void OnMouseEnter()
    {
        if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform) // Check if this object is hit
                {
                    outline.enabled = true;
                }
            }
        }
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
    }

    public void PlaceGunOnPillar(WeaponList _weapon)
    {
        var weaponsList = GameManager.Instance.weaponDataList;

        foreach (var weapon in weaponsList)
        {
            Debug.Log("Inside for loop");
            if (weapon.weapon == _weapon)
            {
                if (isAGunAlreadyPlaced && placedWeapon != null)
                {
                    var tempObj = placedWeapon.gameObject;
                    placedWeapon = null;
                    Destroy(tempObj);
                    Debug.Log("destroyed Temp obj");
                }
                placedWeapon = Instantiate(weapon.weaponObj, gunSpawnPos.position,gunSpawnPos.localRotation);
                placedWeapon.transform.SetParent(gunSpawnPos);
                isAGunAlreadyPlaced = true;
                Debug.Log("Weapon Instantiated");
            }
        }
    }
}
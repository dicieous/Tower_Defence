using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum WeaponList
{
    None,
    PistolTurret,
    MachineGunTurret,
}

public class GameManager : MonoBehaviour
{
    
    [System.Serializable]
    public struct WeaponData
    {
        public WeaponList weapon;
        public int price;
        public bool isBought;
        public GameObject weaponObj;

    }
    
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("UI Manager");
                _instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }
    
    [HideInInspector] public GameObject clickedPillarCache;
    
    public List<WeaponData> weaponDataList;
    public int GetCoins { get; private set; } = 0;
    public int HouseMaxHealth { get; private set; } = 100;

    [SerializeField] private int killCountToWin;
    private int enemiesKilledCount = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UIManager.Instance.ShowScreen(Screen.StartScreen);
    }

    public void StartGame()
    {
        GetCoins = 100;
        UIManager.Instance.ShowScreen(Screen.GameScreen);
        GameEvents.GameStarted();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameEnded()
    {
        GameEvents.GameEnded();
        UIManager.Instance.ShowScreen(Screen.EndScreen);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        UIManager.Instance.ShowScreen(Screen.GameScreen);
        Time.timeScale = 1f;
        
    }

    public void IncreaseKillCount()
    {
        enemiesKilledCount++;
        if (enemiesKilledCount >= killCountToWin)
        {
            GameEnded();
        }
    }
    
    public void GetClickedPillar(GameObject _gameObject){
        clickedPillarCache = _gameObject;
    }

    public void PlacePillarWeapon(WeaponList weapon)
    {
        if (clickedPillarCache.TryGetComponent(out GunPillar gunPillar))
        {
            gunPillar.PlaceGunOnPillar(weapon);
            Debug.Log("PlaceGunOnPillar Called from GameManager");
        }
        
    }
    
    public void SpendCoins(int coins)
    {
        GetCoins -= coins;
        GameEvents.CoinsChanged(GetCoins);
    }

    public void AddCoins(int coins)
    {
        GetCoins += coins;
        GameEvents.CoinsChanged(GetCoins);
    }
}

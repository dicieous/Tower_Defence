using System;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenType
{
    Regular,
    Popup,
}

public enum Screen
{
    None,
    StorePopUp,
    EndScreen,
    GameScreen,
    StartScreen,
}

[Serializable]
public class ScreenData
{
    public Screen screenType;
    public UIBase screen;
    public ScreenType type;
    public bool isScreenOn = false;
}

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("UI Manager");
                _instance = go.AddComponent<UIManager>();
                DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }

    [SerializeField] private List<ScreenData> screenDataList = new List<ScreenData>();

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

    public void ShowScreen(Screen screenType)
    {
        ScreenData screenData = screenDataList.Find(x => x.screenType == screenType);

        if (screenData.type == ScreenType.Regular)
        {
            foreach (var screen in screenDataList)
            {
                screen.screen.Hide();
            }
        }

        screenData.screen.Show();
        screenData.isScreenOn = true;
    }


    public void HideScreen(Screen screenType)
    {
        ScreenData screenData = screenDataList.Find(x => x.screenType == screenType);

        screenData.screen.Hide();
        screenData.isScreenOn = false;
    }

    
    
    public bool IsScreenVisible(Screen screenType)
    {
        var screen = screenDataList.Find(x => x.screenType == screenType).screen;
        return screen.gameObject.activeSelf && screen.GetComponent<CanvasGroup>().alpha > 0;
    }
}
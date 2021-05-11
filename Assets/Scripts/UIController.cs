using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EUIScreen
{
    Title,
    Lobby,
    Game,
    EndGame
}

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [Serializable]
    public struct ScreenData
    {
        public EUIScreen Screen;
        public GameObject RootObject;
    }

    public List<ScreenData> ScreenDatas;

    private Dictionary<EUIScreen, GameObject> _screens = new Dictionary<EUIScreen, GameObject>();
    private GameObject _activeScreen;

    private void Awake()
    {
        Instance = this;
        foreach (var screenData in ScreenDatas)
        {
            _screens.Add(screenData.Screen, screenData.RootObject);
            if (screenData.RootObject != null)
            {
                screenData.RootObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        GameModeController.Instance.OnHostStarted += OnHostStarted;
        GameModeController.Instance.OnClientStarted += OnClientStarted;
        GameModeController.Instance.OnClientConnected += OnClientConnected;
        GameModeController.Instance.OnGameEnd += OnGameEnd;
        
        GoToScreen(EUIScreen.Title);
    }

    private void OnGameEnd(EBoardSymbol victor)
    {
        GoToScreen(EUIScreen.EndGame);
    }

    private void OnHostStarted()
    {
        GoToScreen(EUIScreen.Lobby);
    }

    private void OnClientStarted()
    {
        GoToScreen(EUIScreen.Game);
    }

    private void OnClientConnected()
    {
        GoToScreen(EUIScreen.Game);
    }

    public void GoToScreen(EUIScreen screen)
    {
        if (_screens.TryGetValue(screen, out var rootObject))
        {
            if (_activeScreen != null)
            {
                _activeScreen.SetActive(false);
            }

            _activeScreen = rootObject;
            if (rootObject != null)
            {
                rootObject.SetActive(true);
            }
        }
    }
}
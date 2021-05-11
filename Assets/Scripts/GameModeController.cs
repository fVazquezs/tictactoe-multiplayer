using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine;
using UnityEngine.UI;

public class GameModeController : MonoBehaviour
{
    public static GameModeController Instance { get; private set; }

    public event Action OnHostStarted;
    public event Action OnClientStarted;
    public event Action OnClientConnected;
    public event Action<EBoardSymbol> OnGameEnd; 
    
    public bool IsGameOver { get; private set; }
    public EBoardSymbol GameVictor { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    
    public void StartHost()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.StartHost();
        OnHostStarted?.Invoke();
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            OnClientConnected?.Invoke();
        } 
    }

    public void StartClient(string serverAddress)
    {
        var netManager = NetworkManager.Singleton;
        netManager.GetComponent<UNetTransport>().ConnectAddress = serverAddress;
        netManager.StartClient();
      
        OnClientStarted?.Invoke();
    }

    public void DispatchGameEnd(EBoardSymbol victor)
    {
        IsGameOver = true;
        GameVictor = victor;
        OnGameEnd?.Invoke(victor);
    }
}
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
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        OnHostStarted?.Invoke();
    }

    public void StartClient(string serverAddress)
    {
        var netManager = NetworkManager.Singleton;
        netManager.GetComponent<UNetTransport>().ConnectAddress = serverAddress;
        netManager.StartClient();
      
        OnClientStarted?.Invoke();
    }
    
}
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public InputField ServerAddressField;

    public void StartHost()
    {
        GameModeController.Instance.StartHost();
    }

    public void StartClient()
    {
        GameModeController.Instance.StartClient(ServerAddressField.text);
    }
}

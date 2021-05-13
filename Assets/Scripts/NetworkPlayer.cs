using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;


public enum EBoardSymbol
{
    None,
    Cross,
    Circle
}

public class NetworkPlayer : NetworkBehaviour
{
    public LayerMask PickingMask;

    private void Start()
    {
        if (IsServer)
        {
            BoardController.Instance.AddPlayer(OwnerClientId);
            BoardController.Instance.OnUpdateBoard += UpdateBoard;
            BoardController.Instance.OnGameEnd += OnGameEnd;
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var pickRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(pickRay, out var hitInfo, Mathf.Infinity, PickingMask))
                {
                    var slot = hitInfo.transform.GetComponentInParent<Slot>();
                    if (slot != null)
                    {
                        //make play and make sure its available
                        MakePlayServerRpc(slot.Line, slot.Column);
                    }
                }
            }
        }
    }

    private void OnGameEnd(EBoardSymbol victor)
    {
        OnGameEndClientRpc(victor);
        StartCoroutine(ShutdownServer());
    }

    private void UpdateBoard(int line, int column, EBoardSymbol symbol)
    {
        try
        {
            UpdateBoardClientRpc(line, column, symbol);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [ServerRpc]
    private void MakePlayServerRpc(int line, int column)
    {
        BoardController.Instance.MakePlay(OwnerClientId, line, column);
    }

    [ClientRpc]
    public void UpdateBoardClientRpc(int line, int column, EBoardSymbol symbol)
    {
        BoardController.Instance.UpdateBoardVisuals(line, column, symbol);
    }

    [ClientRpc]
    public void OnGameEndClientRpc(EBoardSymbol victor)
    {
        GameModeController.Instance.DispatchGameEnd(victor);
    }

    IEnumerator ShutdownServer()
    {
        yield return new WaitForSeconds(1);


        NetworkManager.Singleton.Shutdown();
    }
}
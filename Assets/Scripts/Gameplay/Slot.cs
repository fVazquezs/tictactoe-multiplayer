using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int Line;
    public int Column;

    public GameObject CircleRoot;
    public GameObject CrossRoot;

    private void Start()
    {
        BoardController.Instance.RegisterSlot(this);
        GameModeController.Instance.OnGameEnd += OnGameEnd;
    }

    private void OnGameEnd(EBoardSymbol obj)
    {
        CrossRoot.SetActive(false);
        CircleRoot.SetActive(false);
    }


    public void SetSymbol(EBoardSymbol symbol)
    {
        switch (symbol)
        {
            case EBoardSymbol.Circle:
                CircleRoot.SetActive(true);
                break;
            case EBoardSymbol.Cross:
                CrossRoot.SetActive(true);
                break;
        }
    }
}

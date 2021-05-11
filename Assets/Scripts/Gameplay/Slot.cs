using System;
using System.Collections;
using System.Collections.Generic;
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

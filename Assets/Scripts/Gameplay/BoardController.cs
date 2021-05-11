using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public static BoardController Instance { get; private set; }

    public Action<int, int, EBoardSymbol> OnUpdateBoard;
    public Action<EBoardSymbol> OnGameEnd;

    private bool _isGameOver;
    private EBoardSymbol _gameVictor;

    private int _currentPlayerIndex;
    private EBoardSymbol _currentVictor;
    private ulong _currentPlayerId => _playerIds[_currentPlayerIndex];

    private readonly List<ulong> _playerIds = new List<ulong>();

    private Slot[,] _slots = new Slot[3, 3];
    private EBoardSymbol[,] _board = new EBoardSymbol[3, 3];

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterSlot(Slot slot)
    {
        _slots[slot.Line, slot.Column] = slot;
    }

    public void AddPlayer(ulong clientId)
    {
        _playerIds.Add(clientId);
        Debug.LogFormat("Player {0} added to the game", clientId);
    }

    public void MakePlay(ulong clientId, int line, int column)
    {
        if (_isGameOver) return;
        if (clientId != _currentPlayerId) return;
        if (_board[line, column] != EBoardSymbol.None) return;

        EBoardSymbol symbolToSet = _currentPlayerIndex == 0 ? EBoardSymbol.Cross : EBoardSymbol.Circle;
        OnUpdateBoard?.Invoke(line, column, symbolToSet);

        _board[line, column] = symbolToSet;

        if (CheckIsGameOver(out EBoardSymbol currentVictor))
        {
            _isGameOver = true;
            _gameVictor = currentVictor;
            
            OnGameEnd?.Invoke(_gameVictor);
        }

        _currentPlayerIndex = 1 - _currentPlayerIndex;
    }

    private bool CheckIsGameOver(out EBoardSymbol victor)
    {
        if (_board[0, 0] == _board[0, 1] && _board[0, 0] == _board[0, 2] && _board[0, 0] != EBoardSymbol.None)
        {
            victor = _board[0, 0];
            return true;
        }

        victor = EBoardSymbol.None;
        return false;
    }

    public void UpdateBoardVisuals(int line, int column, EBoardSymbol symbol)
    {
        _slots[line, column].SetSymbol(symbol);
    }
}
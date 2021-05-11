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
        bool hasAvailableSlot = false;
        //horizontal
        for (int line = 0; line < 3; line++)
        {
            if (_board[line, 0] == EBoardSymbol.None)
            {
                hasAvailableSlot = true;
                continue;
            }

            int symbolCount = 0;
            for (int c = 0; c < 3; c++)
            {
                if (_board[line, c] == _board[line, 0])
                {
                    symbolCount++;
                }

                if (_board[line, c] == EBoardSymbol.None)
                {
                    hasAvailableSlot = true;
                }
            }

            if (symbolCount == 3)
            {
                victor = _board[line, 0];
                return true;
            }
        }

        //vertical
        for (int column = 0; column < 3; column++)
        {
            if (_board[0, column] == EBoardSymbol.None)
            {
                continue;
            }

            int symbolCount = 0;
            for (int l = 0; l < 3; l++)
            {
                if (_board[l, column] == _board[0, column])
                {
                    symbolCount++;
                }
            }

            if (symbolCount == 3)
            {
                victor = _board[0, column];
                return true;
            }
        }

        //diagonal
        int diagonalCount1 = 0;
        int diagonalCount2 = 0;
        for (int index = 0; index < 3; index++)
        {
            if (_board[index, index] == _board[0, 0] && _board[0, 0] != EBoardSymbol.None)
            {
                diagonalCount1++;
            }

            if (_board[index, 3 - index - 1] == _board[0, 3 - 1] &&
                _board[0, 3 - 1] != EBoardSymbol.None)
            {
                diagonalCount2++;
            }
        }

        if (diagonalCount1 == 3)
        {
            victor = _board[0, 0];
            return true;
        }
        if (diagonalCount2 == 3)
        {
            victor = _board[0, 3 - 1];
            return true;
        }

        victor = EBoardSymbol.None;
        return !hasAvailableSlot;
    }

    public void UpdateBoardVisuals(int line, int column, EBoardSymbol symbol)
    {
        _slots[line, column].SetSymbol(symbol);
    }
}
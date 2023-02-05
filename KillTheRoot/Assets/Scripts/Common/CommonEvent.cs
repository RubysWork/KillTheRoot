using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameChange
{
    public GameStatus gameState;
    public GameChange(GameStatus gameStatus)
    {
        this.gameState = gameStatus;
    }
}

public class ShowBossEmailEvent
{
    public ShowBossEmailEvent() { }
}


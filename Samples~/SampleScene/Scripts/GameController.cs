using GameEvents.Channels;
using GameEventsSystem.Runtime;
using UnityEngine;

public class GameController : BaseSubscriber
{
    public static bool IsGameOver { get; private set; }
    
    [GameplayChannel.OnGameWin]
    [GameplayChannel.OnGameOver]
    private void FinishTheGame(int score)
    {
        if (IsGameOver)
            return;
        
        Debug.Log($"Final score {score}");
        IsGameOver = true;
        Time.timeScale = 0;
    }
}
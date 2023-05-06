using GameEvents.Channels;
using GameEventsSystem.Runtime;
using UnityEngine;

public class SomeController : BaseSubscriber
{
    // pause/unpause
    // objects collecting
    // logging gameover and win
    public PlayerSide side;
    private int _currentScore;

    // [MenuChannel.OnPause]
    public void Pause()
    {
        Debug.Log("Pausing");
    }

    // [MenuChannel.OnPause]
    public void Unpause()
    {
        Debug.Log("Unpause");
    }

    // [MatchChannel.OnGoal]
    private void ScoreGoal(PlayerSide side, int score)
    {
        _currentScore += score;
        Debug.Log($"{side.Side}: {_currentScore}");
    }
}

using GameEvents.Channels;
using GameEventsSystem.Runtime;
using UnityEngine;

public class MenuController : BaseSubscriber
{
    [MenuChannel.OnPauseToggled]
    private void TogglePause()
    {
        if (GameController.IsGameOver)
            return;
        
        Debug.Log($"Pause {Time.timeScale}");
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }
}
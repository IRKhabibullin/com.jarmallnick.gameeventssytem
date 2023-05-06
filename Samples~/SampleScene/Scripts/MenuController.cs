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

        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        Debug.Log(Time.timeScale == 0 ? "Paused" : "Unpaused");
    }
}
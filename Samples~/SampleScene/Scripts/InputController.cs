using GameEvents.Channels;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            MenuChannel.OnPauseToggled?.Invoke();
        }
    }
}
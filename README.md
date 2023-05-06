Game Events System
=====================

A system that will help create game events through a user-friendly interface and make it easy to subscribe to these events

### Generate events

Open Game Events Manager ```Window -> Game Events Manager``` or press ```Ctrl+Shift+E```

Add required events, select types of arguments, group events by logical channels and click "Generate"

![Game events generating](https://i.imgur.com/UtRHifO.gif)

It will generate channel classes with static Action events

If you want to set an argument of type, declared in some package, enable "Include packages" toggle

### Invoke events

Invoke created event as usually

```csharp
using UnityEngine;

public class InputController : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            MenuChannel.OnPause?.Invoke();
        }
    }
}
```

### Subscribe to events
- Inherit subscriber class from BaseSubscriber
- Add an attribute with a name of event to callback method

```csharp
using GameEventsSystem.Runtime;
using UnityEngine;

public class MenuController : BaseSubscriber
{
    [MenuChannel.OnPause]
    public void Pause()
    {
        // Event handling logic here
        Time.timeScale = 0;
        
        // ...
    }
}
```

That's it! All subscribe/unsubscribe logic is done for you
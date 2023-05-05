using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventsSystem.Editor
{
    public class GameEventsDataSo : ScriptableObject
    {
        public List<GameEventsChannel> channels = new();
    }

    [Serializable]
    public class GameEventsChannel
    {
        public string channelName;
        public List<GameEvent> events = new();
    }

    [Serializable]
    public class GameEvent
    {
        public string name;
        public List<string> args;

        public GameEvent()
        {
            args = new List<string>();
        }
    }
}
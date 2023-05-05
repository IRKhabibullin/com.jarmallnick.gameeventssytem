using System;

namespace GameEventsSystem.Runtime
{
    /// <summary>
    /// Attribute used to subscribe/unsubscribe listeners to some event.
    /// Which specific event is subscribed/unsubscribed should be defined in implementation.<br/><br/>
    /// <b>WARNING!</b> Do not use manually unless you know what you are doing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class BaseGameEventAttribute : Attribute
    {
        public abstract void Subscribe(Delegate callback);

        public abstract void Unsubscribe(Delegate callback);
    }
}
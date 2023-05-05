using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace GameEventsSystem.Runtime
{
    /// <summary>
    /// All event subscribers must inherit from this class.<br/><br/>
    /// If subscriber have some logic in OnEnable or OnDisable methods,
    /// they should override and call base version of the methods.
    /// </summary>
    public class BaseSubscriber : MonoBehaviour
    {
        private Dictionary<BaseGameEventAttribute, List<Delegate>> _callbacks;

        /// <summary>
        /// Iterates through all methods that have attribute derived from BaseChannelEventAttribute and
        /// subscribes these callbacks to corresponding events.
        /// </summary>
        private void SubscribeCallbacks()
        {
            _callbacks = new Dictionary<BaseGameEventAttribute, List<Delegate>>();
            foreach (var methodInfo in GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttributes()
                .Any(attr => attr.GetType().BaseType == typeof(BaseGameEventAttribute))))
            {
                var attribute = methodInfo.GetCustomAttribute<BaseGameEventAttribute>();
                if (!_callbacks.ContainsKey(attribute))
                    _callbacks.Add(attribute, new List<Delegate>());

                var actionType = Expression.GetActionType(methodInfo.GetParameters().Select(p => p.ParameterType).ToArray());
                var d = Delegate.CreateDelegate(actionType, null, methodInfo);
                _callbacks[attribute].Add(d);

                attribute.Subscribe(d);
            }
        }

        /// <summary>
        /// Unsubscribes methods from events that were subscribed in SetCallbacks.
        /// </summary>
        private void UnsubscribeCallbacks()
        {
            foreach (var eventAttribute in _callbacks.Keys)
            {
                foreach (var callback in _callbacks[eventAttribute])
                {
                    eventAttribute.Unsubscribe(callback);
                }
            }
        }

        protected virtual void OnEnable()
        {
            // todo try to move this logic from here so users wont need to use override in OnEnable method
            SubscribeCallbacks();
        }

        protected virtual void OnDisable()
        {
            UnsubscribeCallbacks();
        }
    }
}
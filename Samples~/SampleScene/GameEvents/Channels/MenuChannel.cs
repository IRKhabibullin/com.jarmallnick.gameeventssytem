// ----- AUTO GENERATED CODE. DO NOT MODIFY ----- //
using System;
using GameEventsSystem.Runtime;

namespace GameEvents.Channels
{
	public static class MenuChannel
	{
		public static Action OnPauseToggled;

		/// <summary>
		/// Accepts Action callback
		/// </summary>
		[AttributeUsage(AttributeTargets.Method)]
		public class OnPauseToggledAttribute : BaseGameEventAttribute
		{
			public override void Subscribe(Delegate callback)
			{
				OnPauseToggled += (Action)callback;
			}

			public override void Unsubscribe(Delegate callback)
			{
				OnPauseToggled -= (Action)callback;
			}
		}
	}
}

// ----- AUTO GENERATED CODE. DO NOT MODIFY ----- //
using System;
using GameEventsSystem.Runtime;

namespace GameEvents.Channels
{
	public static class GameplayChannel
	{
		public static Action<ItemType, int> OnItemCollected;
		public static Action<int> OnGameOver;
		public static Action<int> OnGameWin;

		/// <summary>
		/// Accepts Action&lt;ItemType, int> callback
		/// </summary>
		[AttributeUsage(AttributeTargets.Method)]
		public class OnItemCollectedAttribute : BaseGameEventAttribute
		{
			public override void Subscribe(Delegate callback)
			{
				OnItemCollected += (Action<ItemType, int>)callback;
			}

			public override void Unsubscribe(Delegate callback)
			{
				OnItemCollected -= (Action<ItemType, int>)callback;
			}
		}

		/// <summary>
		/// Accepts Action&lt;int> callback
		/// </summary>
		[AttributeUsage(AttributeTargets.Method)]
		public class OnGameOverAttribute : BaseGameEventAttribute
		{
			public override void Subscribe(Delegate callback)
			{
				OnGameOver += (Action<int>)callback;
			}

			public override void Unsubscribe(Delegate callback)
			{
				OnGameOver -= (Action<int>)callback;
			}
		}

		/// <summary>
		/// Accepts Action&lt;int> callback
		/// </summary>
		[AttributeUsage(AttributeTargets.Method)]
		public class OnGameWinAttribute : BaseGameEventAttribute
		{
			public override void Subscribe(Delegate callback)
			{
				OnGameWin += (Action<int>)callback;
			}

			public override void Unsubscribe(Delegate callback)
			{
				OnGameWin -= (Action<int>)callback;
			}
		}
	}
}

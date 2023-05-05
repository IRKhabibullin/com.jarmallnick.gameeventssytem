using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameEventsSystem.Editor.Logics.Drawers
{
    /// <summary>
    /// Defines how GameEvent should be drawn in manager window
    /// </summary>
    public class GameEventDrawer
    {
        private const string DefaultEventArgType = "int";

        #region Layout size

        private static readonly Vector2 AddRemoveButtonWidth = Vector2.right * GUI.skin.button.CalcSize(new GUIContent("-")).x;
        private static readonly Vector2 AddRemoveButtonsOffset = Vector2.right * 5;
        private static readonly Vector2 CommaWidth = Vector2.right * GUI.skin.label.CalcSize(new GUIContent(",")).x;
        private static readonly Vector2 ActionOpenLabelWidth = Vector2.right * GUI.skin.label.CalcSize(new GUIContent("Action<")).x;
        private static readonly Vector2 ActionCloseLabelWidth = Vector2.right * (GUI.skin.label.CalcSize(new GUIContent(">")).x + 5);

        #endregion

        public static void Draw(Rect rect, GameEvent @event)
        {
            var contentOffset = new Vector2(rect.x, rect.y);
            
            GUILayout.BeginHorizontal();
            
            GUI.Label(new Rect(contentOffset.x, contentOffset.y, ActionOpenLabelWidth.x, EditorGUIUtility.singleLineHeight), "Action<");
            contentOffset += ActionOpenLabelWidth;
            
            DrawEventArgs(rect, ref contentOffset, @event);

            DrawAddRemoveButtons(ref contentOffset, @event.args);
            
            GUI.Label(new Rect(contentOffset.x, contentOffset.y, ActionCloseLabelWidth.x, EditorGUIUtility.singleLineHeight), ">");
            contentOffset += ActionCloseLabelWidth;
            
            var contentWidth = (int)GUI.skin.textField.CalcSize(new GUIContent(@event.name)).x + 5;
            @event.name = GUI.TextField(new Rect(contentOffset.x, contentOffset.y, contentWidth, EditorGUIUtility.singleLineHeight), @event.name);
            
            GUILayout.EndHorizontal();
        }

        private static void DrawEventArgs(Rect rect, ref Vector2 contentOffset, GameEvent @event)
        {
            var eventArgs = @event.args;
            for (var i = 0; i < eventArgs.Count; i++)
            {
                var eventArg = eventArgs[i];
                if (i > 0)
                {
                    GUI.Label(new Rect(contentOffset.x, contentOffset.y, CommaWidth.x, EditorGUIUtility.singleLineHeight), ",");
                    contentOffset += Vector2.right * CommaWidth;
                }
                
                var contentWidth = (int)GUI.skin.button.CalcSize(new GUIContent(eventArg)).x;
                if (GUI.Button(new Rect(contentOffset.x, contentOffset.y, contentWidth, EditorGUIUtility.singleLineHeight),
                        eventArg))
                {
                    GameEventsManagerWindow.DrawTypesDropdown(@event, i, rect);
                }
                contentOffset += Vector2.right * contentWidth;
            }

            if (eventArgs.Count > 0)
            {
                contentOffset += AddRemoveButtonsOffset;
            }
        }

        private static void DrawAddRemoveButtons(ref Vector2 contentRect, List<string> eventArgs)
        {
            GUI.color = Color.green;
            if (GUI.Button(
                new Rect(contentRect.x, contentRect.y, AddRemoveButtonWidth.x, EditorGUIUtility.singleLineHeight),
                "+",
                EditorStyles.miniButtonLeft))
            {
                eventArgs.Add(DefaultEventArgType);
            }
            contentRect += AddRemoveButtonWidth;
            GUI.color = Color.red;
            if (GUI.Button(
                new Rect(contentRect.x, contentRect.y, AddRemoveButtonWidth.x, EditorGUIUtility.singleLineHeight),
                "-",
                EditorStyles.miniButtonRight))
            {
                if (eventArgs.Count > 0)
                    eventArgs.RemoveAt(eventArgs.Count - 1);
            }
            contentRect += AddRemoveButtonWidth;
            GUI.color = Color.white;
        }
    }
}
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace GameEventsSystem.Editor.Logics.Drawers
{
    /// <summary>
    /// Defines how GameEventsChannel should be drawn in manager window
    /// </summary>
    public class EventsChannelDrawer
    {
        public GameEventsChannel Channel { get; }
        private readonly ReorderableList _eventsList;

        private string _currentChannelName;
        private const string ChannelNameLabelText = "Channel name";

        public EventsChannelDrawer(GameEventsChannel channel)
        {
            Channel = channel;
            _currentChannelName = Channel.channelName;

            _eventsList = new ReorderableList(
                Channel.events,
                typeof(GameEvent),
                true,
                true,
                true,
                true
            );
            _eventsList.drawHeaderCallback += DrawHeader;
            _eventsList.drawElementCallback += DrawEvent;
            _eventsList.elementHeightCallback += GetEventHeight;
        }

        public void Draw(Rect rect)
        {
            rect.y += 2;
            GUILayout.BeginHorizontal();
            var contentWidth = (int)GUI.skin.label.CalcSize(new GUIContent(ChannelNameLabelText)).x + 5;
            GUI.Label(new Rect(rect.x, rect.y, contentWidth, EditorGUIUtility.singleLineHeight), ChannelNameLabelText);

            var textFieldWidth = (int)GUI.skin.textField.CalcSize(new GUIContent(Channel.channelName)).x + 5;
            _currentChannelName = GUI.TextField(
                new Rect(rect.x + contentWidth, rect.y, textFieldWidth, EditorGUIUtility.singleLineHeight),
                Channel.channelName
            );
            
            if (!_currentChannelName.Contains(" "))
            {
                Channel.channelName = _currentChannelName;
            }
            GUILayout.EndHorizontal();
            
            rect.y += EditorGUIUtility.singleLineHeight * 1.5f;
            _eventsList.DoList(rect);
        }

        private static float GetEventHeight(int index)
        {
            return EditorGUIUtility.singleLineHeight * 1.25f;
        }

        private static void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Events");
        }

        private void DrawEvent(Rect rect, int index, bool isActive, bool isFocused)
        {
            GameEventDrawer.Draw(rect, Channel.events[index]);
        }
    }
}
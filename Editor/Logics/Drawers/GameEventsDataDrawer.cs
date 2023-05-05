using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace GameEventsSystem.Editor.Logics.Drawers
{
    /// <summary>
    /// Defines how GameEventsDataSO should be drawn in manager window
    /// </summary>
    public class GameEventsDataDrawer
    {
        private GameEventsDataSo _data;
        private ReorderableList _channelsList;
        private List<EventsChannelDrawer> _channels;
        
        private const string ChannelPattern = @"Channel(\d)";

        public GameEventsDataDrawer(GameEventsDataSo data)
        {
            _data = data;
            InitChannels();

            _channelsList = new ReorderableList(
                _data.channels,
                typeof(GameEventsChannel),
                true,
                true,
                true,
                true
            );

            _channelsList.drawElementCallback += DrawChannel;
            _channelsList.drawHeaderCallback += DrawHeader;
            _channelsList.elementHeightCallback += GetChannelHeight;
            _channelsList.onReorderCallback += ReorderChannels;
            _channelsList.onAddCallback += AddChannel;
            _channelsList.onRemoveCallback += RemoveChannel;
        }

        private void InitChannels()
        {
            _channels = new List<EventsChannelDrawer>();
            foreach (var channel in _data.channels)
            {
                _channels.Add(new EventsChannelDrawer(
                    channel
                ));
            }
        }

        public void Draw()
        {
            _channelsList.DoLayoutList();
        }

        private void DrawChannel(Rect rect, int index, bool isActive, bool isFocused)
        {
            _channels[index].Draw(rect);
        }

        private float GetChannelHeight(int index)
        {
            if (_data.channels[index].events.Count == 0)
                return 5.5f * EditorGUIUtility.singleLineHeight;
            return (_data.channels[index].events.Count + 5) * EditorGUIUtility.singleLineHeight;
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Channels");
        }

        private void ReorderChannels(ReorderableList list)
        {
            _channels = _channels
                .Select(ch=>new{orderOf=_data.channels.IndexOf(ch.Channel),val=ch})
                .OrderBy(a=>a.orderOf)
                .Select(a=>a.val)
                .ToList();
        }

        private void AddChannel(ReorderableList list)
        {
            var greatestIndex = 0;
            if (_data.channels.Count > 0)
                greatestIndex = _data.channels
                    .Select(ch => Regex.Match(ch.channelName, ChannelPattern))
                    .Where(m => m.Groups.Count > 1)
                    .Select(m => int.Parse(m.Groups[1].Value)).Max();
            _data.channels.Add(new GameEventsChannel
            {
                channelName = $"Channel{greatestIndex + 1}"
            });
            _channels.Add(new EventsChannelDrawer(
                _data.channels[^1]
            ));
        }

        private void RemoveChannel(ReorderableList list)
        {
            if (_channels.Count == 0)
                return;
            _data.channels.RemoveAt(list.index);
            _channels.RemoveAt(list.index);
        }
    }
}
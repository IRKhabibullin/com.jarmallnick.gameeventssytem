using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GameEventsSystem.Editor.Logics.Types;
using UnityEditor;
using UnityEngine;

namespace GameEventsSystem.Editor
{
    public static class GameEventsGenerator
    {
        public static void Generate(GameEventsDataSo data)
        {
            if (!AssetDatabase.IsValidFolder("Assets/GameEvents"))
            {
                AssetDatabase.CreateFolder("Assets", "GameEvents");
            }
            if (!AssetDatabase.IsValidFolder("Assets/GameEvents/Channels"))
            {
                AssetDatabase.CreateFolder("Assets/GameEvents", "Channels");
            }
            
            foreach (var channel in data.channels)
            {
                var path = $"Assets/GameEvents/Channels/{channel.channelName}Channel.cs";
                
                try
                {
                    if (File.Exists(path))
                        File.Delete(path);
                    using ( var stream = File.Open( path, FileMode.Create, FileAccess.Write ) )
                    {
                        using( var writer = new StreamWriter(stream) )
                        {
                            var builder = new StringBuilder();
                            
                            GenerateChannel(builder, channel);
                            writer.Write( builder.ToString() );
                        }
                    }
                }
                catch(Exception e)
                {
                    Debug.LogException( e );
     
                    if(File.Exists( path ))
                        File.Delete( path );
                }
            }
 
            AssetDatabase.Refresh();
        }

        #region Code generation
        
        private static void GenerateChannel(StringBuilder builder, GameEventsChannel channel)
        {
            builder.AppendLine("// ----- AUTO GENERATED CODE. DO NOT MODIFY ----- //");
            AddUsingNamespaces(builder, channel);
            builder.AppendLine();
            builder.AppendLine("namespace GameEvents.Channels");
            builder.AppendLine("{");
            builder.AppendLine($"\tpublic static class {channel.channelName}Channel");
            builder.AppendLine("\t{");

            AddEventsCode(builder, channel);
            AddEventAttribute(builder, channel);

            builder.AppendLine("\t}");
            builder.AppendLine("}");
        }

        private static void AddUsingNamespaces(StringBuilder builder, GameEventsChannel channel)
        {
            var namespaces = new HashSet<string>
            {
                "System",
                "GameEventsSystem.Runtime"
            };

            var excludedNamespaces = new List<string>
            {
                "Default namespace"
            };
            
            foreach (var channelEvent in channel.events)
            {
                foreach (var eventArg in channelEvent.args)
                {
                    var typeReference = TypesCollector.GetTypeReference(eventArg);
                    if (typeReference == null)
                    {
                        Debug.LogWarning($"Can not find type {eventArg}");
                        continue;
                    }

                    namespaces.Add(typeReference.NamespaceName);
                }
            }

            foreach (var ns in namespaces.Except(excludedNamespaces))
            {
                builder.AppendLine($"using {ns};");
            }
        }

        private static void AddEventsCode(StringBuilder builder, GameEventsChannel channel)
        {
            foreach (var channelEvent in channel.events)
            {
                var eventType = channelEvent.args.Count > 0 ? $"<{string.Join(", ", channelEvent.args)}>" : "";
                builder.AppendLine($"\t\tpublic static Action{eventType} {channelEvent.name};");
            }
        }

        private static void AddEventAttribute(StringBuilder builder, GameEventsChannel channel)
        {
            foreach (var channelEvent in channel.events)
            {
                var eventType = channelEvent.args.Count > 0 ? $"<{string.Join(", ", channelEvent.args)}>" : "";
                var eventTypeComment = channelEvent.args.Count > 0 ? $"&lt;{string.Join(", ", channelEvent.args)}>" : "";
                
                builder.AppendLine();
                builder.AppendLine("\t\t/// <summary>");
                builder.AppendLine($"\t\t/// Accepts Action{eventTypeComment} callback");
                builder.AppendLine("\t\t/// </summary>");
                builder.AppendLine("\t\t[AttributeUsage(AttributeTargets.Method)]");
                builder.AppendLine($"\t\tpublic class {channelEvent.name}Attribute : BaseGameEventAttribute");
                builder.AppendLine("\t\t{");
                builder.AppendLine("\t\t\tpublic override void Subscribe(Delegate callback)");
                builder.AppendLine("\t\t\t{");
                builder.AppendLine($"\t\t\t\t{channelEvent.name} += (Action{eventType})callback;");
                builder.AppendLine("\t\t\t}");
                builder.AppendLine();
                builder.AppendLine("\t\t\tpublic override void Unsubscribe(Delegate callback)");
                builder.AppendLine("\t\t\t{");
                builder.AppendLine($"\t\t\t\t{channelEvent.name} -= (Action{eventType})callback;");
                builder.AppendLine("\t\t\t}");
                builder.AppendLine("\t\t}");
            }
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.IO;

namespace GameEventsSystem.Editor.Logics.Types
{
    public class TypeReference
    {
        private readonly string _assemblyName;
        private readonly Type _type;

        private static Dictionary<Type, string> typeNames = new()
        {
            { typeof(int), "int" },
            { typeof(float), "float" },
            { typeof(char), "char" },
            { typeof(string), "string" },
            { typeof(bool), "bool" },
        };

        public string AssemblyName => _assemblyName ?? "";
        public string NamespaceName => _type.Namespace ?? "Default namespace";
        public string TypeName => GetTypeName(_type);

        public TypeReference(Type type, string assemblyName = null)
        {
            _assemblyName = assemblyName ?? Path.GetFileNameWithoutExtension(type.Assembly.Location);
            _type = type;
        }

        private static string GetTypeName(Type type)
        {
            return typeNames.TryGetValue(type, out var typeName) ? typeName : type.Name;
        }
    }
}
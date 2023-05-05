using System;
using System.IO;

namespace GameEventsSystem.Editor.Logics.Types
{
    public class TypeReference
    {
        private readonly string _assemblyName;
        private readonly Type _type;
        
        public string AssemblyName => _assemblyName ?? "";
        public string NamespaceName => _type.Namespace ?? "Default namespace";
        public string TypeName => _type.Name;

        public TypeReference(Type type, string assemblyName = null)
        {
            _assemblyName = assemblyName ?? Path.GetFileNameWithoutExtension(type.Assembly.Location);
            _type = type;
        }
    }
}
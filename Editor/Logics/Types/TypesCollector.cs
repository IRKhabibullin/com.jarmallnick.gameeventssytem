using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace GameEventsSystem.Editor.Logics.Types
{
    /// <summary>
    /// Collects all types available in current project
    /// </summary>
    public static class TypesCollector
    {
        public static bool IncludePackages;  // if types from packages in project should be included in collection
        public static List<TypeReference> Types = new();

        public static TypeReference GetTypeReference(string name)
        {
            return Types.FirstOrDefault(t => t.TypeName == name);
        }
        
        public static void UpdateAvailableTypes()
        {
            Types.Clear();
            
            AddPrimitiveTypes();
            AddCurrentAssemblyTypes();
            AddAsmdefTypes();
        }

        /// <summary>
        /// Primitives from System namespace
        /// </summary>
        private static void AddPrimitiveTypes()
        {
            Types.AddRange(new List<TypeReference>
            {
                new(typeof(int), "Primitives"),
                new(typeof(float), "Primitives"),
                new(typeof(char), "Primitives"),
                new(typeof(string), "Primitives"),
                new(typeof(bool), "Primitives")
            });
        }

        /// <summary>
        /// Types from project's Assembly-CSharp assembly
        /// </summary>
        private static void AddCurrentAssemblyTypes()
        {
            var currentAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(ass => ass.FullName.StartsWith("Assembly-CSharp"));

            if (currentAssembly == null)
                return;
            Types.AddRange(currentAssembly.GetTypes()
                .Where(t => !t.Name.Contains("<>c") && !t.Name.Contains("AnonymousType"))
                .Select(t => new TypeReference(t, "Current assembly"))
            );
        }

        /// <summary>
        /// Types from other assemblies in project.
        /// If IncludePackages is true then types from packages also added to types collection
        /// </summary>
        private static void AddAsmdefTypes()
        {
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToArray();
            var asmdefPaths = AssetDatabase.FindAssets("t:AssemblyDefinitionAsset")
                .Select(AssetDatabase.GUIDToAssetPath);

            if (!IncludePackages)
            {
                asmdefPaths = asmdefPaths.Where(assPath => !assPath.StartsWith("Packages"));
            }

            foreach (var asmdefPath in asmdefPaths)
            {
                var asmdefName = Path.GetFileNameWithoutExtension(asmdefPath);
                var assembly =
                    allAssemblies.FirstOrDefault(t => Path.GetFileNameWithoutExtension(t.Location) == asmdefName);
                if (assembly == null)
                    continue;
                
                Types.AddRange(assembly.GetTypes()
                    .Where(t => !t.Name.Contains("<>c") && !t.Name.Contains("AnonymousType"))
                    .Select(t => new TypeReference(t))
                );
            }
        }
    }
}
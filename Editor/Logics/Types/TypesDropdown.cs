using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace GameEventsSystem.Editor.Logics.Types
{
    /// <summary>
    /// Dropdown with types available in project.
    /// </summary>
    internal class TypesDropdown : AdvancedDropdown
    {
        private List<TypeReference> _types;
        
        public TypesDropdown(AdvancedDropdownState state) : base(state)
        {
        }

        public void SetData(List<TypeReference> types)
        {
            _types = types;
        }

        /// <summary>
        /// Builds hierarchy of available types.
        /// First level: assembly, second: namespace, third: types
        /// </summary>
        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem("Available types");

            var assembliesDictionary = new Dictionary<string, AdvancedDropdownItem>();
            
            var namespacesDictionary = new Dictionary<string, AdvancedDropdownItem>();

            foreach (var typeReference in _types)
            {
                if (!assembliesDictionary.ContainsKey(typeReference.AssemblyName))
                {
                    assembliesDictionary.Add(typeReference.AssemblyName, new AdvancedDropdownItem(typeReference.AssemblyName));
                    root.AddChild(assembliesDictionary[typeReference.AssemblyName]);
                }
                
                if (!namespacesDictionary.ContainsKey(typeReference.NamespaceName))
                {
                    namespacesDictionary.Add(typeReference.NamespaceName, new AdvancedDropdownItem(typeReference.NamespaceName));
                    assembliesDictionary[typeReference.AssemblyName].AddChild(namespacesDictionary[typeReference.NamespaceName]);
                }
                
                namespacesDictionary[typeReference.NamespaceName].AddChild(new AdvancedDropdownItem(typeReference.TypeName));
            }

            return root;
        }

        /// <summary>
        /// Callback for type selected in dropdown
        /// </summary>
        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            base.ItemSelected(item);
            GameEventsManagerWindow.OnTypeSelected?.Invoke(item.name);
        }
    }
}
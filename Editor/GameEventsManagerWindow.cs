using System;
using GameEventsSystem.Editor.Logics.Drawers;
using GameEventsSystem.Editor.Logics.Types;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace GameEventsSystem.Editor
{
    /// <summary>
    /// Manager window. Draws GameEventsDataSo in convenient format
    /// </summary>
    public class GameEventsManagerWindow : EditorWindow
    {
        public static Action<string> OnTypeSelected;
        
        #region Fields
        private static TypesDropdown _typesDropdown;
        private static GameEventsDataSo _data;

        private static GameEventsDataDrawer _gameEventsDataDrawer;
        private const string DataPath = "Assets/GameEvents/DataSO.asset";
        
        #endregion

        private void OnGUI()
        {
            DrawIncludePackagesToggle();
            
            _gameEventsDataDrawer.Draw();
            
            DrawGenerateButton();
        }

        [MenuItem("Window/Game Events Manager %#e")]
        private static void Init()
        {
            GetWindow<GameEventsManagerWindow>("Game Events Manager");
        }

        private void OnEnable()
        {
            TypesCollector.UpdateAvailableTypes();
            LoadData();
            
            _typesDropdown = new TypesDropdown(new AdvancedDropdownState());
            _typesDropdown.SetData(TypesCollector.Types);

            _gameEventsDataDrawer = new GameEventsDataDrawer(_data);
        }

        private static void LoadData()
        {
            var dataAsset = AssetDatabase.LoadAssetAtPath<GameEventsDataSo>(DataPath);
            if (dataAsset != null)
            {
                _data = dataAsset;
            }
            else
            {
                _data = CreateInstance<GameEventsDataSo>();
                AssetDatabase.CreateFolder("Assets", "GameEvents");
                AssetDatabase.CreateAsset(_data, DataPath);
            }
        }

        public static void DrawTypesDropdown(GameEvent @event, int argIndex, Rect rect)
        {
            _typesDropdown.Show(rect);
            OnTypeSelected += GetTypeSetter(@event, argIndex);
        }

        private static void DrawGenerateButton()
        {
            if (GUILayout.Button("Generate"))
            {
                GameEventsGenerator.Generate(_data);
            }
        }

        private static Action<string> GetTypeSetter(GameEvent @event, int argIndex)
        {
            void Setter(string typeName)
            {
                @event.args[argIndex] = typeName;
                OnTypeSelected = null;
            }

            return Setter;
        }
        
        private static void DrawIncludePackagesToggle()
        {
            var includePackagesToggle = EditorGUILayout.Toggle("Include packages", TypesCollector.IncludePackages);
            if (TypesCollector.IncludePackages == includePackagesToggle)
                return;
            TypesCollector.IncludePackages = includePackagesToggle;
            TypesCollector.UpdateAvailableTypes();
            _typesDropdown.SetData(TypesCollector.Types);
        }
    }
}
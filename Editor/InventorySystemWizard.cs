using UnityEditor;
using UnityEngine;

namespace Studio23.SS2.InventorySystem.Editor
{

    public class InventorySystemWizard : EditorWindow
    {
        private int _currentTab;

        private BackpackEditorWindow _backPackWizard;


        private Texture _header;

        [MenuItem("Studio-23/Inventory System/Wizard")]
        public static void ShowWindow()
        {
            GetWindow<InventorySystemWizard>("Inventory System Wizard");
        }

        private void OnEnable()
        {
            _backPackWizard = new BackpackEditorWindow();
            _header = Resources.Load<Texture>("InventorySystemheader");
        }

        private void OnGUI()
        {

            GUILayout.Box(_header, GUILayout.Height(200), GUILayout.ExpandWidth(true));

            _currentTab = GUILayout.Toolbar(_currentTab, new string[] { "Backpack Wizard", "Hint Wizard" });

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            if (_currentTab == 0)
            {
                _backPackWizard.ShowWindow();
            }
            else if (_currentTab == 1)
            {
                //_dialogueGraphEditorUI.ShowWindow();
            }

            EditorGUILayout.EndVertical();
        }



    }
}
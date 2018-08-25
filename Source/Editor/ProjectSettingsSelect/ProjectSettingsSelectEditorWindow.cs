using UnityEditor;
using UnityEngine;

namespace PumpEditor
{
    public class ProjectSettingsSelectEditorWindow : EditorWindow
    {
        private Vector2 scrollPosition;

        [MenuItem("Window/Pump Editor/Project Settings Select")]
        private static void Init()
        {
            var window = EditorWindow.GetWindow<ProjectSettingsSelectEditorWindow>();
            var icon = EditorGUIUtility.Load("SettingsIcon") as Texture2D;
            window.titleContent = new GUIContent("Project", icon);
            window.Show();
        }

        private static void DrawProjectSettingsButton(string menuItemName)
        {
            if (GUILayout.Button(menuItemName))
            {
                var menuItemPath = "Edit/Project Settings/" + menuItemName;
                EditorApplication.ExecuteMenuItem(menuItemPath);
            }
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            DrawProjectSettingsButton("Input");
            DrawProjectSettingsButton("Tags and Layers");
            DrawProjectSettingsButton("Audio");
            DrawProjectSettingsButton("Time");
            DrawProjectSettingsButton("Player");
            DrawProjectSettingsButton("Physics");
            DrawProjectSettingsButton("Physics 2D");
            DrawProjectSettingsButton("Quality");
            DrawProjectSettingsButton("Graphics");
            DrawProjectSettingsButton("Network");
            DrawProjectSettingsButton("Editor");
            DrawProjectSettingsButton("Script Execution Order");
            DrawProjectSettingsButton("Preset Manager");

            EditorGUILayout.EndScrollView();
        }
    }
}

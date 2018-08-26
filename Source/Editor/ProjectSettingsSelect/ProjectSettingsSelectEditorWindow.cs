using UnityEditor;
using UnityEngine;

namespace PumpEditor
{
    public class ProjectSettingsSelectEditorWindow : EditorWindow
    {
        private bool showSettingsInspector;
        private Vector2 windowScrollPosition;
        private Vector2 buttonsScrollPosition;
        private Vector2 inspectorScrollPosition;

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
            if (GUILayout.Button(menuItemName, GUILayout.MinWidth(150)))
            {
                var menuItemPath = "Edit/Project Settings/" + menuItemName;
                EditorApplication.ExecuteMenuItem(menuItemPath);
            }
        }

        private void OnGUI()
        {
            windowScrollPosition = EditorGUILayout.BeginScrollView(windowScrollPosition);
            EditorGUILayout.BeginHorizontal();

            ProjectSettingsButtonsGUI();
            ProjectSettingsInspectorGUI();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }

        private void OnSelectionChange()
        {
            Repaint();
        }

        private void ProjectSettingsButtonsGUI()
        {
            EditorGUILayout.BeginVertical();
            showSettingsInspector = EditorGUILayout.Toggle("Show Settings Inspector", showSettingsInspector);
            buttonsScrollPosition = EditorGUILayout.BeginScrollView(buttonsScrollPosition);
            EditorGUILayout.BeginVertical();

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

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void ProjectSettingsInspectorGUI()
        {
            if (!showSettingsInspector)
            {
                return;
            }

            var selectedObject = Selection.activeObject;
            if (selectedObject == null
                || !ProjectSettingsTypeHelper.IsProjectSettingsType(selectedObject))
            {
                EditorGUILayout.HelpBox("No project settings to inspect.", MessageType.Info);
                return;
            }

            inspectorScrollPosition = EditorGUILayout.BeginScrollView(inspectorScrollPosition);
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();

            Editor editor = Editor.CreateEditor(selectedObject);
            editor.OnInspectorGUI();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
}

using UnityEditor;
using UnityEngine;

namespace PumpEditor
{
    public class LogFilesEditorWindow : EditorWindow
    {
        [MenuItem("Window/Pump Editor/Log Files")]
        private static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<LogFilesEditorWindow>();
            window.titleContent = new GUIContent("Log Files");
            window.Show();
        }

        private static void DrawLogFileItem(string name, string logFilePath)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.TextField(name, logFilePath);

                if (GUILayout.Button("Log path to console"))
                {
                    Debug.Log(logFilePath);
                }
            }
        }

        private static void EditorLogsGUI()
        {
            EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);
            DrawLogFileItem("Editor log", LogFilePathsAPI.GetEditorLogPath());
        }

        private void OnGUI()
        {
            EditorLogsGUI();
        }
    }
}

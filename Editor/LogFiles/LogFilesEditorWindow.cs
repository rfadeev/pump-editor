using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
            EditorGUILayout.TextField(name, logFilePath);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Log path to console"))
                {
                    Debug.Log(logFilePath);
                }

                if (GUILayout.Button("Open directory"))
                {
                    var dirPath = Path.GetDirectoryName(logFilePath);
                    Process.Start(dirPath);
                }

                if (GUILayout.Button("Open file"))
                {
                   Process.Start(logFilePath);
                }
            }
        }

        private static void EditorLogsGUI()
        {
            EditorGUILayout.LabelField("Package Manager", EditorStyles.boldLabel);
            DrawLogFileItem("Package manager log", LogFilePathsAPI.GetPackageManagerLogPath());

            EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);
            DrawLogFileItem("Editor log", LogFilePathsAPI.GetEditorLogPath());
            DrawLogFileItem("Editor log prev", LogFilePathsAPI.GetEditorLogPrevPath());

            EditorGUILayout.LabelField("Player", EditorStyles.boldLabel);
            DrawLogFileItem("Player log", LogFilePathsAPI.GetPlayerLogPath());
        }

        private void OnGUI()
        {
            EditorLogsGUI();
        }
    }
}

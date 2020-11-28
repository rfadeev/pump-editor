using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
#if UNITY_2019_2_OR_NEWER
using Unity.CodeEditor;
#endif

namespace PumpEditor
{
    public class LogFilesEditorWindow : EditorWindow
    {
        [MenuItem("Window/Pump Editor/Log Files")]
        private static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<LogFilesEditorWindow>();
            var icon = EditorGUIUtility.Load("UnityEditor.ConsoleWindow") as Texture2D;
            window.titleContent = new GUIContent("Log Files", icon);
            window.Show();
        }

        private static void OpenLogFile(string logFilePath)
        {
#if UNITY_2019_2_OR_NEWER
            CodeEditor.CurrentEditor.OpenProject(logFilePath);
#else
            Process.Start(logFilePath);
#endif
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
                   OpenLogFile(logFilePath);
                }
            }
        }

        private static void EditorLogsGUI()
        {
            using (new EditorGUILayout.VerticalScope((GUI.skin.box)))
            {
                EditorGUILayout.LabelField("Package Manager", EditorStyles.boldLabel);
                DrawLogFileItem("Package manager log", LogFilePathsAPI.GetPackageManagerLogPath());
            }

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope((GUI.skin.box)))
            {
                EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);
                DrawLogFileItem("Editor log", LogFilePathsAPI.GetEditorLogPath());
                EditorGUILayout.Space();
                DrawLogFileItem("Editor log prev", LogFilePathsAPI.GetEditorLogPrevPath());
            }

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope((GUI.skin.box)))
            {
                EditorGUILayout.LabelField("Player", EditorStyles.boldLabel);
                DrawLogFileItem("Player log", LogFilePathsAPI.GetPlayerLogPath());
                EditorGUILayout.Space();
                DrawLogFileItem("Player log prev", LogFilePathsAPI.GetPlayerLogPrevPath());
            }
        }

        private void OnGUI()
        {
            EditorLogsGUI();
        }
    }
}

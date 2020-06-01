using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace PumpEditor
{
    public class CompilationWindow : EditorWindow
    {
        [MenuItem("Window/Pump Editor/Compilation")]
        private static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<CompilationWindow>();
            window.titleContent = new GUIContent("Compilation");
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Request Script Compilation"))
            {
                CompilationPipeline.RequestScriptCompilation();
            }
        }
    }
}

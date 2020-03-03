using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PumpEditor
{
    // Inspired by https://forum.unity.com/threads/shortcut-key-for-lock-inspector.95815/#post-5013983
    public static class LockToggle
    {
        [MenuItem("Window/Pump Editor/Tools/Toggle Lock Focused Window %w")]
        private static void ToggleLockFocusedWindow()
        {
            ToggleLockEditorWindow(EditorWindow.focusedWindow);
        }

        [MenuItem("Window/Pump Editor/Tools/Toggle Lock Mouse Over Window %e")]
        private static void ToggleLockMouseOverWindow()
        {
            ToggleLockEditorWindow(EditorWindow.mouseOverWindow);
        }

        [MenuItem("Window/Pump Editor/Tools/Toggle Lock All Windows %#w")]
        private static void ToggleLockAllWindows()
        {
            var allWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
            foreach (var editorWindow in allWindows)
            { 
                ToggleLockEditorWindow(editorWindow);
            }
        }

        private static void ToggleLockEditorWindow(EditorWindow editorWindow)
        {
            Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));
            Type projectBrowserType = editorAssembly.GetType("UnityEditor.ProjectBrowser");
            Type inspectorWindowType = editorAssembly.GetType("UnityEditor.InspectorWindow");
            Type sceneHierarchyWindowType = editorAssembly.GetType("UnityEditor.SceneHierarchyWindow");

            Type editorWindowType = editorWindow.GetType();
            if (editorWindowType == projectBrowserType)
            {
                // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/c6ec7823//Editor/Mono/ProjectBrowser.cs#L113
                PropertyInfo propertyInfo = projectBrowserType.GetProperty("isLocked", BindingFlags.Instance | BindingFlags.NonPublic);

                bool value = (bool)propertyInfo.GetValue(editorWindow);
                propertyInfo.SetValue(editorWindow, !value);
            }
            else if (editorWindowType == inspectorWindowType)
            {
                // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/c6ec7823//Editor/Mono/Inspector/InspectorWindow.cs##L492
                PropertyInfo propertyInfo = inspectorWindowType.GetProperty("isLocked");

                bool value = (bool)propertyInfo.GetValue(editorWindow);
                propertyInfo.SetValue(editorWindow, !value);
            }
            else if (editorWindowType == sceneHierarchyWindowType)
            {
                // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/c6ec7823/Editor/Mono/SceneHierarchyWindow.cs#L34
                PropertyInfo sceneHierarchyPropertyInfo = sceneHierarchyWindowType.GetProperty("sceneHierarchy");
                var sceneHierarchy = sceneHierarchyPropertyInfo.GetValue(editorWindow);

                // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/c6ec7823/Editor/Mono/SceneHierarchy.cs#L88
                Type sceneHierarchyType = editorAssembly.GetType("UnityEditor.SceneHierarchy");
                PropertyInfo propertyInfo = sceneHierarchyType.GetProperty("isLocked", BindingFlags.Instance | BindingFlags.NonPublic);

                bool value = (bool)propertyInfo.GetValue(sceneHierarchy);
                propertyInfo.SetValue(sceneHierarchy, !value);
            }
            else
            {
                return;
            }

            editorWindow.Repaint();
        }
    }
}

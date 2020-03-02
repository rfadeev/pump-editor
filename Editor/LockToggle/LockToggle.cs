using System;
using System.Reflection;
using UnityEditor;

namespace PumpEditor
{
    // Inspired by https://forum.unity.com/threads/shortcut-key-for-lock-inspector.95815/#post-5013983
    public static class LockToggle
    {
        [MenuItem("Window/Pump Editor/Tools/Toggle Lock Focused Window %l")]
        private static void ToggleLockFocusedWindow()
        {
            ToggleLockEditorWindow(EditorWindow.focusedWindow);
        }

        [MenuItem("Window/Pump Editor/Tools/Toggle Lock Mouse Over Window %#l")]
        private static void ToggleLockMouseOverWindow()
        {
            ToggleLockEditorWindow(EditorWindow.mouseOverWindow);
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
                PropertyInfo propertyInfo = projectBrowserType.GetProperty("isLocked", BindingFlags.Instance | BindingFlags.NonPublic);

                bool value = (bool)propertyInfo.GetValue(editorWindow);
                propertyInfo.SetValue(editorWindow, !value);
            }
            else if (editorWindowType == inspectorWindowType)
            {
                PropertyInfo propertyInfo = inspectorWindowType.GetProperty("isLocked");

                bool value = (bool)propertyInfo.GetValue(editorWindow);
                propertyInfo.SetValue(editorWindow, !value);
            }
            else if (editorWindowType == sceneHierarchyWindowType)
            {
                PropertyInfo sceneHierarchyPropertyInfo = sceneHierarchyWindowType.GetProperty("sceneHierarchy");
                var sceneHierarchy = sceneHierarchyPropertyInfo.GetValue(editorWindow);

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

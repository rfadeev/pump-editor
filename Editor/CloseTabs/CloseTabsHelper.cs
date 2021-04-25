using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace PumpEditor
{
    public static class CloseTabsHelper
    {
        private const string CloseOtherTabsText = "Close Other Tabs";
        private const string CloseTabsToTheRigthText = "Close Tabs to the Right";

        public static void AddCloseOtherTabsItem(EditorWindow window, GenericMenu menu)
        {
            if (window.maximized)
            {
                menu.AddDisabledItem(new GUIContent(CloseOtherTabsText));
            }
            else
            {
                var panes = GetAllPanes(window);
                // Disable close tabs item if editor window is the only tab.
                if (panes.Count == 1)
                {
                    menu.AddDisabledItem(new GUIContent(CloseOtherTabsText));
                }
                else
                {
                    menu.AddItem(new GUIContent(CloseOtherTabsText), false, () => OnCloseOtherTabs(window, panes));
                }
            }
        }

        public static void AddCloseTabsToTheRightItem(EditorWindow window, GenericMenu menu)
        {
            if (window.maximized)
            {
                menu.AddDisabledItem(new GUIContent(CloseTabsToTheRigthText));
            }
            else
            {
                var panes = GetAllPanes(window);
                // Disable close tabs item if editor window is the rightest tab.
                if (panes.IndexOf(window) == panes.Count - 1)
                {
                    menu.AddDisabledItem(new GUIContent(CloseTabsToTheRigthText));
                }
                else
                {
                    menu.AddItem(new GUIContent(CloseTabsToTheRigthText), false, () => OnCloseTabsToTheRight(window, panes));
                }
            }
        }

        private static List<EditorWindow> GetAllPanes(EditorWindow window)
        {
            var editorWindowType = typeof(EditorWindow);
            FieldInfo mParentField = editorWindowType.GetField("m_Parent", BindingFlags.Instance | BindingFlags.NonPublic);
            var parentValue = mParentField.GetValue(window);

            // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/61f92bd79ae862c4465d35270f9d1d57befd1761/Editor/Mono/GUI/DockArea.cs#L18
            Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));
            Type dockAreaType = editorAssembly.GetType("UnityEditor.DockArea");

            // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/61f92bd79ae862c4465d35270f9d1d57befd1761/Editor/Mono/GUI/DockArea.cs#L77
            FieldInfo mPanesField = dockAreaType.GetField("m_Panes", BindingFlags.Instance | BindingFlags.NonPublic);
            var mPanesValue = mPanesField.GetValue(parentValue);

            var mPanes = (List<EditorWindow>)mPanesValue;
            return mPanes;
        } 

        private static void OnCloseOtherTabs(EditorWindow window, List<EditorWindow> panes)
        {
            // Store panes to remove in a separate list since panes gets modified by EditorWindow.Close method
            // and throws "InvalidOperationException: Collection was modified; enumeration operation may not execute."
            var panesToRemove = panes.FindAll(x => !ReferenceEquals(x, window));

            foreach (var pane in panesToRemove)
            {
                pane.Close();
            }
        }

        private static void OnCloseTabsToTheRight(EditorWindow window, List<EditorWindow> panes)
        {
            // Store panes to remove in a separate list since panes gets modified by EditorWindow.Close method
            // and throws "InvalidOperationException: Collection was modified; enumeration operation may not execute."
            var index = panes.FindIndex(x => ReferenceEquals(x, window));
            var removeIndex = index + 1;
            var panesToRemove = panes.GetRange(removeIndex, panes.Count - removeIndex);

            foreach (var pane in panesToRemove)
            {
                pane.Close();
            }
        }
    }
}

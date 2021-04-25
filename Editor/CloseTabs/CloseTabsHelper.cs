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

        public static void AddCloseOtherTabsItem(EditorWindow window, GenericMenu menu)
        {
            if (window.maximized)
            {
                menu.AddDisabledItem(new GUIContent(CloseOtherTabsText));
            }
            else
            {
                menu.AddItem(new GUIContent(CloseOtherTabsText), false, () => OnCloseOtherTabs(window));
            }
        }

        private static void OnCloseOtherTabs(EditorWindow window)
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
            // Store panes to remove in a separate list since mPanes gets modified by RemoveTab method
            // and throws "InvalidOperationException: Collection was modified; enumeration operation may not execute."
            var mPanesToRemove = mPanes.FindAll(x => !ReferenceEquals(x, window));

            foreach (var mPane in mPanesToRemove)
            {
                mPane.Close();
            }
        }
    }
}

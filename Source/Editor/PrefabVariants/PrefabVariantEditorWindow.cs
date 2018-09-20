using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace PumpEditor
{
    // Implementation is based on SimpleTreeViewWindow example
    // from TreeView manual: https://docs.unity3d.com/Manual/TreeViewAPI.html
    public class PrefabVariantEditorWindow : EditorWindow
    {
        [SerializeField]
        private TreeViewState treeViewState;

        private PrefabVariantTreeView treeView;
        private SearchField searchField;

        [MenuItem("Window/Pump Editor/Prefab Variants")]
        private static void ShowWindow()
        {
            var window = GetWindow<PrefabVariantEditorWindow>();
            window.titleContent = new GUIContent("Prefab Variants");
            window.Show();
        }

        private void DoToolbar()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Space(100);
            GUILayout.FlexibleSpace();
            treeView.searchString = searchField.OnToolbarGUI(treeView.searchString);
            GUILayout.EndHorizontal();
        }

        private void DoTreeView()
        {
            var rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);
            treeView.OnGUI(rect);
        }

        private void OnEnable()
        {
            // Check if we already had a serialized view state (state
            // that survived assembly reloading)
            if (treeViewState == null)
            {
                treeViewState = new TreeViewState();
            }

            treeView = new PrefabVariantTreeView(treeViewState);
            searchField = new SearchField();
            searchField.downOrUpArrowKeyPressed += treeView.SetFocusAndEnsureSelectedItem;
        }

        private void OnGUI()
        {
            DoToolbar();
            DoTreeView();
        }
    }
}

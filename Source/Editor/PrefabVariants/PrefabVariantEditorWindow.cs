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

        private static TreeViewItem GetRoot()
        {
            var root = new TreeViewItem
            {
                id = 0,
                depth = -1
            };

            var inheritanceChains = PrefabInheritanceHelper.GetInheritanceChains();
            foreach (var inheritanceChain in inheritanceChains)
            {
                if (inheritanceChain.Count == 0)
                {
                    continue;
                }

                var prefabAsset = inheritanceChain[0];
                var prefabItem = new TreeViewItem
                {
                    id = prefabAsset.GetInstanceID(),
                    displayName = prefabAsset.name
                };
                root.AddChild(prefabItem);

                var i = 1;
                while (i < inheritanceChain.Count)
                {
                    var nestedPrefabAsset = inheritanceChain[i];
                    var nestedPrefabItem = new TreeViewItem
                    {
                        id = nestedPrefabAsset.GetInstanceID(),
                        displayName = nestedPrefabAsset.name
                    };

                    prefabItem.AddChild(nestedPrefabItem);
                    prefabItem = nestedPrefabItem;

                    ++i;
                }
            }

            return root;
        }

        public void ReloadTreeView()
        {
            treeView.Reload();
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

        private void DoAssetInfo()
        {
            var selectedIDs = treeViewState.selectedIDs;
            if (selectedIDs.Count == 0)
            {
                return;
            }

            Debug.Assert(selectedIDs.Count == 1);

            GUILayout.BeginVertical(Styles.inspectorBigTitleInner);
            EditorGUILayout.Space();

            // TODO: [rfadeev] - Migrate to separate type instead of
            // plain TreeViewItem and get instance id from tree model.
            var instanceId = selectedIDs[0];
            var assetPath = AssetDatabase.GetAssetPath(instanceId);
            var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

            // If asset is deleted, selectedIDs are not reset so need to
            // check that load asset at path succeeded.
            if (asset == null)
            {
                return;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledGroupScope(true))
                {
                    EditorGUILayout.ObjectField("Prefab Asset", asset, typeof(GameObject), false);
                }
            }

            var variantBase = PrefabUtility.GetCorrespondingObjectFromSource(asset);
            if (variantBase != null)
            {
                using (new EditorGUI.DisabledGroupScope(true))
                {
                    EditorGUILayout.ObjectField("Base", variantBase, typeof(GameObject), false);
                }
            }

            EditorGUILayout.Space();
            GUILayout.EndVertical();
        }

        private void OnEnable()
        {
            // Check if we already had a serialized view state (state
            // that survived assembly reloading)
            if (treeViewState == null)
            {
                treeViewState = new TreeViewState();
            }

            var root = GetRoot();
            treeView = new PrefabVariantTreeView(treeViewState, root);
            searchField = new SearchField();
            searchField.downOrUpArrowKeyPressed += treeView.SetFocusAndEnsureSelectedItem;
        }

        private void OnGUI()
        {
            DoToolbar();
            DoTreeView();
            DoAssetInfo();
        }

        private static class Styles
        {
            public static readonly GUIStyle inspectorBigTitleInner = "IN BigTitle inner";
        }
    }
}

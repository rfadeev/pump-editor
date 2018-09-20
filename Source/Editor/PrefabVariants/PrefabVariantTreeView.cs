using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace PumpEditor
{
    public class PrefabVariantTreeView : TreeView
    {
        public PrefabVariantTreeView(TreeViewState treeViewState)
        : base(treeViewState)
        {
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem
            {
                id = 0,
                depth = -1
            };

            var inheritancePaths = GetInheritancePaths();
            foreach (var inheritancePath in inheritancePaths)
            {
                if (inheritancePath.Count == 0)
                {
                    continue;
                }

                var prefabAsset = inheritancePath[0];
                var prefabItem = new TreeViewItem
                {
                    id = prefabAsset.GetInstanceID(),
                    displayName = prefabAsset.name
                };
                root.AddChild(prefabItem);

                var i = 1;
                while (i < inheritancePath.Count)
                {
                    var nestedPrefabAsset = inheritancePath[i];
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

            SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        private static List<List<Object>> GetInheritancePaths()
        {
            var inheritancePaths = new List<List<Object>>();

            var guids = AssetDatabase.FindAssets("t:Prefab");
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

                var inheritancePath = new List<Object>();
                while (PrefabUtility.GetPrefabAssetType(asset) == PrefabAssetType.Variant)
                {
                    inheritancePath.Add(asset);
                    asset = PrefabUtility.GetCorrespondingObjectFromSource(asset);
                }

                inheritancePath.Add(asset);
                inheritancePaths.Add(inheritancePath);
            }

            return inheritancePaths;
        }
    }
}

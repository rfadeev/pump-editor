using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace PumpEditor
{
    public class PrefabVariantTreeView : TreeView
    {
        private const float SpaceBeforeIconAndLabel = 18.0f;
        private const float AssetIconWidth = 16.0f;

        private static Texture2D PrefabIcon = EditorGUIUtility.FindTexture("prefab icon");
        private static Texture2D PrefabVariantIcon = EditorGUIUtility.FindTexture("prefabvariant icon");

        public PrefabVariantTreeView(TreeViewState treeViewState)
        : base(treeViewState)
        {
            extraSpaceBeforeIconAndLabel = SpaceBeforeIconAndLabel;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
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

            SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var assetIconRect = args.rowRect;
            assetIconRect.x += GetContentIndent(args.item);
            assetIconRect.width = AssetIconWidth;

            var instanceId = args.item.id;
            var assetPath = AssetDatabase.GetAssetPath(instanceId);
            var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            var isVariant = PrefabUtility.GetPrefabAssetType(asset) == PrefabAssetType.Variant;

            var assetIconTexture = isVariant ? PrefabVariantIcon : PrefabIcon;
            GUI.DrawTexture(assetIconRect, assetIconTexture);

            base.RowGUI(args);
        }
    }
}

#if UNITY_2017_3_OR_NEWER

using System;
using UnityEditor;
using UnityEngine;

namespace PumpEditor
{
    public class ForceReserializeAssetsEditorWindow : EditorWindow
    {
        [MenuItem("Window/Pump Editor/Force Reserialize Assets")]
        private static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<ForceReserializeAssetsEditorWindow>();
            var icon = EditorGUIUtility.Load("scriptableobject icon") as Texture2D;
            window.titleContent = new GUIContent("Force Reserialize Assets", icon);
            window.Show();
        }

        private static void ForceReserializeAllAssets()
        {
            if (!EditorUtility.DisplayDialog("Attention", "Do you want to force reserialize all assets? This can be time heavy operation and result in massive list of changes.", "Ok", "Cancel"))
            {
                return;
            }

            AssetDatabase.ForceReserializeAssets();
        }

        private static void ForceReserializeSelectedAssets()
        {
            var assetGUIDs = Selection.assetGUIDs;
            if (assetGUIDs.Length == 0)
            {
                EditorUtility.DisplayDialog("Attention", "No assets are selected.", "Ok");
                return;
            }

            var assetPaths = Array.ConvertAll<string, string>(assetGUIDs, AssetDatabase.GUIDToAssetPath);
            AssetDatabase.ForceReserializeAssets(assetPaths);
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                if (GUILayout.Button("Force Reserialize All Assets"))
                {
                    ForceReserializeAllAssets();
                }

                if (GUILayout.Button("Force Reserialize Selected Assets"))
                {
                    ForceReserializeSelectedAssets();
                }
            }
        }
    }
}

#endif

using System;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace PresetsBrowser
{
    public class PresetsBrowserWindow : EditorWindow
    {
        private Vector2 m_scrollPosition;

        [MenuItem("Window/Presets Browser")]
        private static void Init()
        {
            var window = EditorWindow.GetWindow<PresetsBrowserWindow>();
            window.titleContent = new GUIContent("Presets");
            window.Show();
        }

        private void OnGUI()
        {
            var presetGuids = AssetDatabase.FindAssets("t:preset");

            m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);

            foreach (var presetGuid in presetGuids)
            {
                var presetPath = AssetDatabase.GUIDToAssetPath(presetGuid);
                var preset = AssetDatabase.LoadAssetAtPath<Preset>(presetPath);
                var presetType = Type.GetType(preset.GetTargetFullTypeName());
                EditorGUILayout.ObjectField(preset, presetType, false);
            }

            EditorGUILayout.EndScrollView();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace PresetsBrowser
{
    public class PresetsBrowserWindow : EditorWindow
    {
        private List<Preset> m_presets = new List<Preset>();
        private HashSet<string> m_presetTargetFullTypeNames = new HashSet<string>();
        private List<Preset> m_presetsToDraw = new List<Preset>();
        private bool m_filterByPresetType = false;
        private Vector2 m_scrollPosition;
        private string m_filterPresetType;

        [MenuItem("Window/Presets Browser")]
        private static void Init()
        {
            var window = EditorWindow.GetWindow<PresetsBrowserWindow>();
            window.titleContent = new GUIContent("Presets");
            window.Show();
        }

        private void OnGUI()
        {
            m_presets.Clear();
            m_presetTargetFullTypeNames.Clear();
            m_presetsToDraw.Clear();

            var presetGuids = AssetDatabase.FindAssets("t:preset");
            foreach (var presetGuid in presetGuids)
            {
                var presetPath = AssetDatabase.GUIDToAssetPath(presetGuid);
                var preset = AssetDatabase.LoadAssetAtPath<Preset>(presetPath);
                m_presets.Add(preset);

                var targetFullTypeName = preset.GetTargetFullTypeName();
                m_presetTargetFullTypeNames.Add(targetFullTypeName);
            }

            m_filterByPresetType = EditorGUILayout.Toggle("Filter by preset type", m_filterByPresetType);

            if (m_filterByPresetType)
            {
                var typeNames = m_presetTargetFullTypeNames.ToArray<string>();
                var index = Array.IndexOf(typeNames, m_filterPresetType);
                index = EditorGUILayout.Popup(index, typeNames);
                if (index >= 0 && index < typeNames.Length)
                {
                    m_filterPresetType = typeNames[index];
                    var selectedPresetType = TypeUtility.GetType(m_filterPresetType);

                    foreach (var preset in m_presets)
                    {
                        var targetFullTypeName = preset.GetTargetFullTypeName();
                        var presetType = TypeUtility.GetType(targetFullTypeName);
                        if (selectedPresetType == presetType)
                        {
                            m_presetsToDraw.Add(preset);
                        }
                    }
                }
                else
                {
                    m_filterPresetType = null;
                }
            }
            else
            {
                m_presetsToDraw.AddRange(m_presets);
            }

            m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);
            foreach (var preset in m_presetsToDraw)
            {
                EditorGUILayout.ObjectField(preset, null, false);
            }
            EditorGUILayout.EndScrollView();
        }
    }
}

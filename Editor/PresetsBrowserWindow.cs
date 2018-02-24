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
        private static readonly string[] VALIDITY_TOOLBAR_STRINGS = new string[3]
        {
            "All",
            "Only Valid",
            "Only Invalid",
        };

        private const int ALL_VALIDITY_TOOLBAR_INDEX = 0;
        private const int ONLY_VALID_VALIDITY_TOOLBAR_INDEX = 1;
        private const int ONLY_INVALID_VALIDITY_TOOLBAR_INDEX = 2;

        private List<Preset> m_presets = new List<Preset>();
        private HashSet<string> m_presetTargetFullTypeNames = new HashSet<string>();
        private List<Preset> m_presetsToDraw = new List<Preset>();
        private int m_validityToolbarIndex;
        private bool m_filterByPresetType = false;
        private Vector2 m_scrollPosition;
        private string m_filterPresetType = null;

        [MenuItem("Window/Presets Browser")]
        private static void Init()
        {
            var window = EditorWindow.GetWindow<PresetsBrowserWindow>();
            window.titleContent = new GUIContent("Presets");
            window.Show();
        }

        // If preset is not valid, GetTargetFullTypeName returns empty string
        private static bool TryGetValidPresetTargetFullTypeName(Preset preset, out string targetFullTypeName)
        {
            targetFullTypeName = preset.GetTargetFullTypeName();
            return targetFullTypeName != String.Empty;
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

                string targetFullTypeName;
                if (TryGetValidPresetTargetFullTypeName(preset, out targetFullTypeName))
                {
                    m_presetTargetFullTypeNames.Add(targetFullTypeName);
                }
            }

            m_validityToolbarIndex = GUILayout.Toolbar(m_validityToolbarIndex, VALIDITY_TOOLBAR_STRINGS);

            // Do not show filter by preset type for invalid presets since for them
            // coressponding class is not present:
            // https://forum.unity.com/threads/presets-feature.491263/#post-3210492
            if (m_validityToolbarIndex != ONLY_INVALID_VALIDITY_TOOLBAR_INDEX)
            {
                m_filterByPresetType = EditorGUILayout.Toggle("Filter by preset type", m_filterByPresetType);
                if (m_filterByPresetType)
                {
                    DrawFilterByType();
                }
                else
                {
                    m_presetsToDraw.AddRange(m_presets);
                }
            }
            else
            {
                m_presetsToDraw.AddRange(m_presets);
            }

            FilterByValidity();

            m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);
            foreach (var preset in m_presetsToDraw)
            {
                EditorGUILayout.ObjectField(preset, null, false);
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawFilterByType()
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
                    string targetFullTypeName;
                    if (TryGetValidPresetTargetFullTypeName(preset, out targetFullTypeName))
                    {
                        var presetType = TypeUtility.GetType(targetFullTypeName);
                        if (selectedPresetType == presetType)
                        {
                            m_presetsToDraw.Add(preset);
                        }
                    }
                }
            }
            else
            {
                m_filterPresetType = null;
            }
        }

        private void FilterByValidity()
        {
            switch (m_validityToolbarIndex)
            {
                case ALL_VALIDITY_TOOLBAR_INDEX:
                    break;
                case ONLY_VALID_VALIDITY_TOOLBAR_INDEX:
                    m_presetsToDraw.RemoveAll(p => !p.IsValid());
                    break;
                case ONLY_INVALID_VALIDITY_TOOLBAR_INDEX:
                    m_presetsToDraw.RemoveAll(p => p.IsValid());
                    break;
            }
        }
    }
}

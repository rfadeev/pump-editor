#if UNITY_2018_1_OR_NEWER

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace PumpEditor
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

        [MenuItem("Window/Pump Editor/Presets Browser")]
        private static void Init()
        {
            var window = EditorWindow.GetWindow<PresetsBrowserWindow>();
            var icon = EditorGUIUtility.FindTexture("preset.context");
            window.titleContent = new GUIContent("Presets", icon);
            window.Show();
        }

        // If preset is not valid, GetTargetFullTypeName returns empty string
        private static bool TryGetValidPresetTargetFullTypeName(Preset preset, out string targetFullTypeName)
        {
            targetFullTypeName = preset.GetTargetFullTypeName();
            return targetFullTypeName != String.Empty;
        }

        private static void DrawPresetItem(Preset preset)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.ObjectField(preset, null, false);
                }

                if (GUILayout.Button("Select", GUILayout.Width(100)))
                {
                    Selection.activeObject = preset;
                }
            }
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
            DrawPresets();
        }

        private void DrawFilterByType()
        {
            var typeNames = m_presetTargetFullTypeNames.ToArray();
            Array.Sort(typeNames);
            var index = Array.IndexOf(typeNames, m_filterPresetType);
            index = EditorGUILayout.Popup("Preset type", index, typeNames);
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

        private void DrawPresets()
        {
            if (m_presetsToDraw.Count != 0)
            {
                using (var scrollView = new EditorGUILayout.ScrollViewScope(m_scrollPosition))
                {
                    m_scrollPosition = scrollView.scrollPosition;

                    foreach (var preset in m_presetsToDraw)
                    {
                        DrawPresetItem(preset);
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("No presets.");
            }
        }
    }
}

#endif

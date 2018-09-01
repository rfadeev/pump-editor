using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace PumpEditor
{
    public class SceneOpenEditorWindow : EditorWindow
    {
        private Vector2 windowScrollPosition;

        [MenuItem("Window/Pump Editor/Scene Open")]
        private static void Init()
        {
            var window = EditorWindow.GetWindow<SceneOpenEditorWindow>();
            var icon = EditorGUIUtility.Load("buildsettings.editor.small") as Texture2D;
            window.titleContent = new GUIContent("Scenes", icon);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Scenes In Project", EditorStyles.boldLabel);
            windowScrollPosition = EditorGUILayout.BeginScrollView(windowScrollPosition);

            var sceneAssetGuids = AssetDatabase.FindAssets("t:scene");
            foreach (var sceneAssetGuid in sceneAssetGuids)
            {
                var sceneAssetPath = AssetDatabase.GUIDToAssetPath(sceneAssetGuid);
                if (GUILayout.Button(sceneAssetPath))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(sceneAssetPath);
                    }
                }
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}

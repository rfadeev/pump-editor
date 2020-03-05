using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PumpEditor
{
    public class SaveProjectAsTemplateEditorWindow : EditorWindow
    {
        private static readonly string UnityEditorApplicationProjectTemplatesPath = Path.Combine(
            Path.GetDirectoryName(EditorApplication.applicationPath),
            "Data",
            "Resources",
            "PackageManager",
            "ProjectTemplates"
        );

        private string targetPath;
        private string templateName;
        private string templateDisplayName;
        private string templateDescription;
        private string templateDefaultScene;
        private string templateVersion;

        [MenuItem("Window/Pump Editor/Save Project As Template")]
        private static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<SaveProjectAsTemplateEditorWindow>();
            window.titleContent = new GUIContent("Save Project As Template");
            window.Show();
        }

        private void InvokeSaveProjectAsTemplate()
        {
            Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));
            Type editorUtilityType = editorAssembly.GetType("UnityEditor.EditorUtility");

            MethodInfo methodInfo = editorUtilityType.GetMethod("SaveProjectAsTemplate", BindingFlags.Static | BindingFlags.NonPublic);
            methodInfo.Invoke(editorUtilityType, new object[]{ targetPath, templateName, templateDisplayName, templateDescription, templateDefaultScene, templateVersion});
        }

        private void DeleteProjectVersionTxt()
        {
            var projectVersionTxtPath = Path.Combine(targetPath, "ProjectSettings", "ProjectVersion.txt");

            if (File.Exists(projectVersionTxtPath))
            {
                File.Delete(projectVersionTxtPath);
            }
            else
            {
                Debug.LogErrorFormat("File ProjectVersion.txt does not exist at path: {0}", projectVersionTxtPath);
            }
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Select Target Folder"))
            {
                targetPath = EditorUtility.SaveFolderPanel("Choose target folder", UnityEditorApplicationProjectTemplatesPath, "");
            }

            targetPath = EditorGUILayout.TextField("Path:", targetPath);

            templateName = EditorGUILayout.TextField("Name:", templateName);
            templateDisplayName = EditorGUILayout.TextField("Display name:", templateDisplayName);
            templateDescription = EditorGUILayout.TextField("Description:", templateDescription);
            templateDefaultScene = EditorGUILayout.TextField("Default scene:", templateDefaultScene);
            templateVersion = EditorGUILayout.TextField("Version:", templateVersion);

            if (GUILayout.Button("Save"))
            {
                AssetDatabase.SaveAssets();
                InvokeSaveProjectAsTemplate();
                DeleteProjectVersionTxt();
            }
        }
    }
}

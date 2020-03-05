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

        private static readonly string[] TemplateFolderNames = new string[]
        {
            "Assets",
            "Packages",
            "ProjectSettings",
        };

        private string targetPath;
        private string templateName;
        private string templateDisplayName;
        private string templateDescription;
        private string templateDefaultScene;
        private string templateVersion;
        private bool replaceTemplate;

        [MenuItem("Window/Pump Editor/Project Templates/Save Project As Template")]
        private static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<SaveProjectAsTemplateEditorWindow>();
            var icon = EditorGUIUtility.Load("saveas@2x") as Texture2D;
            window.titleContent = new GUIContent("Save Project As Template", icon);
            window.Show();
        }

        private void DeleteTemplateFolders()
        {
            try
            {
                foreach (var tempateFolderName in TemplateFolderNames)
                {
                    var path = Path.Combine(targetPath, tempateFolderName);
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("Failed to delete template folders, exception: {0}", e.Message);
            }
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

        private void SetTemplateDataFromPackageJson()
        {
            var packageJsonPath = Path.Combine(targetPath, "package.json");
            if (File.Exists(packageJsonPath))
            {
                var packageJson = File.ReadAllText(packageJsonPath);
                var templateData = JsonUtility.FromJson<TemplateData>(packageJson);
                templateName = templateData.Name;
                templateDisplayName = templateData.DisplayName;
                templateDescription = templateData.Description;
                templateDefaultScene = templateData.DefaultScene;
                templateVersion = templateData.Version;
            }
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Select Target Folder"))
            {
                targetPath = EditorUtility.SaveFolderPanel("Choose target folder", UnityEditorApplicationProjectTemplatesPath, "");
                SetTemplateDataFromPackageJson();
            }

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                targetPath = EditorGUILayout.TextField("Path:", targetPath);
                if (check.changed)
                {
                    SetTemplateDataFromPackageJson();
                }
            }

            templateName = EditorGUILayout.TextField("Name:", templateName);
            templateDisplayName = EditorGUILayout.TextField("Display name:", templateDisplayName);
            templateDescription = EditorGUILayout.TextField("Description:", templateDescription);
            templateDefaultScene = EditorGUILayout.TextField("Default scene:", templateDefaultScene);
            templateVersion = EditorGUILayout.TextField("Version:", templateVersion);
            replaceTemplate = EditorGUILayout.Toggle("Replace Template:", replaceTemplate);

            if (GUILayout.Button("Save"))
            {
                if (replaceTemplate)
                {
                    DeleteTemplateFolders();
                }
                AssetDatabase.SaveAssets();
                InvokeSaveProjectAsTemplate();
                DeleteProjectVersionTxt();
            }
        }

        [Serializable]
        private class TemplateData
        {
#pragma warning disable 0649
            [SerializeField]
            private string name;
            [SerializeField]
            private string displayName;
            [SerializeField]
            private string description;
            [SerializeField]
            private string defaultScene;
            [SerializeField]
            private string version;
#pragma warning restore 0649

            public string Name { get { return name; } }
            public string DisplayName { get { return displayName; } }
            public string Description { get { return description; } }
            public string DefaultScene { get { return defaultScene; } }
            public string Version { get { return version; } }
        }
    }
}

using System;
using System.Reflection;
using UnityEditor;

namespace PumpEditor
{
    public class UnityProjectTemplateWindowMenuItem
    {
        [MenuItem("Window/Pump Editor/Project Templates/Unity Project Template Window")]
        private static void ShowWindow()
        {
            Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));
            Type projectTemplateWindowType = editorAssembly.GetType("UnityEditor.ProjectTemplateWindow");

            MethodInfo methodInfo = projectTemplateWindowType.GetMethod("SaveAsTemplate", BindingFlags.Static | BindingFlags.NonPublic);
            methodInfo.Invoke(projectTemplateWindowType, null);
        }
    }
}

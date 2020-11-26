using System;

namespace PumpEditor
{
    // File paths based on https://docs.unity3d.com/2019.4/Documentation/Manual/LogFiles.html
    public static class LogFilePathsAPI
    {
        private const string EDITOR_LOG_PATH = "C:\\Users\\{0}\\AppData\\Local\\Unity\\Editor\\Editor.log";

        public static string GetEditorLogPath()
        {
            return String.Format(EDITOR_LOG_PATH, Environment.UserName);
        }
    }
}

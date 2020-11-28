using System;
using System.IO;
using UnityEngine;

namespace PumpEditor
{
    // File paths based on https://docs.unity3d.com/2019.4/Documentation/Manual/LogFiles.html
    public static class LogFilePathsAPI
    {
        private static readonly string WINDOWS_PACKAGE_MANAGER_LOG_PATH;
        private static readonly string WINDOWS_EDITOR_LOG_PATH;
        private static readonly string WINDOWS_EDITOR_LOG_PREV_PATH;
        private static readonly string WINDOWS_PLAYER_LOG_PATH;

        static LogFilePathsAPI()
        {
            var appDataLocalPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var editorLogsDirectoryPath = Path.Combine(appDataLocalPath, "Unity", "Editor");
            WINDOWS_PACKAGE_MANAGER_LOG_PATH = Path.Combine(editorLogsDirectoryPath, "upm.log");
            WINDOWS_EDITOR_LOG_PATH = Path.Combine(editorLogsDirectoryPath, "Editor.log");
            WINDOWS_EDITOR_LOG_PREV_PATH = Path.Combine(editorLogsDirectoryPath, "Editor-prev.log");
            // While Unity docs state file name as Player.log, it seems to be an error in the docs.
            var appDataLocalLowPath = Path.GetFullPath(Path.Combine(appDataLocalPath, "..", "LocalLow"));
            WINDOWS_PLAYER_LOG_PATH = Path.Combine(appDataLocalLowPath, "{0}", "{1}", "output_log.txt");
        }

        public static string GetPackageManagerLogPath()
        {
            return WINDOWS_PACKAGE_MANAGER_LOG_PATH;
        }

        public static string GetEditorLogPath()
        {
            return WINDOWS_EDITOR_LOG_PATH;
        }

        public static string GetEditorLogPrevPath()
        {
            return WINDOWS_EDITOR_LOG_PREV_PATH;
        }

        public static string GetPlayerLogPath()
        {
            return String.Format(WINDOWS_PLAYER_LOG_PATH, Application.companyName, Application.productName);
        }
    }
}

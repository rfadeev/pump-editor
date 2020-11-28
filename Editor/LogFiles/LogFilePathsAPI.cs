using System;
using System.IO;
using UnityEngine;

namespace PumpEditor
{
    // File paths based on https://docs.unity3d.com/2019.4/Documentation/Manual/LogFiles.html
    public static class LogFilePathsAPI
    {
        private const string MACOS_PACKAGE_MANAGER_LOG_PATH = "~/Library/Logs/Unity/upm.log";
        private const string MACOS_EDITOR_LOG_PATH = "~/Library/Logs/Unity/Editor.log";
        private const string MACOS_EDITOR_LOG_PREV_PATH = "~/Library/Logs/Unity/Editor-prev.log";
        private const string MACOS_PLAYER_LOG_PATH = "~/Library/Logs/{0}/{1}/Player.log";
        private const string MACOS_PLAYER_LOG_PREV_PATH = "~/Library/Logs/{0}/{1}/Player-prev.log";

        private const string LINUX_PACKAGE_MANAGER_LOG_PATH = "~/.config/unity3d/upm.log";
        private const string LINUX_EDITOR_LOG_PATH = "~/.config/unity3d/Editor.log";
        private const string LINUX_EDITOR_LOG_PREV_PATH = "~/.config/unity3d/Editor-prev.log";
        private const string LINUX_PLAYER_LOG_PATH = "~/.config/unity3d/{0}/{1}/Player.log";
        private const string LINUX_PLAYER_LOG_PREV_PATH = "~/.config/unity3d/{0}/{1}/Player-prev.log";

        private static readonly string WINDOWS_PACKAGE_MANAGER_LOG_PATH;
        private static readonly string WINDOWS_EDITOR_LOG_PATH;
        private static readonly string WINDOWS_EDITOR_LOG_PREV_PATH;
        private static readonly string WINDOWS_PLAYER_LOG_PATH;
        private static readonly string WINDOWS_PLAYER_LOG_PREV_PATH;

        private static readonly string PACKAGE_MANAGER_LOG_PATH;
        private static readonly string EDITOR_LOG_PATH;
        private static readonly string EDITOR_LOG_PREV_PATH;
        private static readonly string PLAYER_LOG_PATH;
        private static readonly string PLAYER_LOG_PREV_PATH;

        static LogFilePathsAPI()
        {
            var appDataLocalPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var editorLogsDirectoryPath = Path.Combine(appDataLocalPath, "Unity", "Editor");
            WINDOWS_PACKAGE_MANAGER_LOG_PATH = Path.Combine(editorLogsDirectoryPath, "upm.log");
            WINDOWS_EDITOR_LOG_PATH = Path.Combine(editorLogsDirectoryPath, "Editor.log");
            WINDOWS_EDITOR_LOG_PREV_PATH = Path.Combine(editorLogsDirectoryPath, "Editor-prev.log");
            // While Unity docs state file name as Player.log, it seems to be an error in the docs.
            var appDataLocalLowPath = Path.GetFullPath(Path.Combine(appDataLocalPath, "..", "LocalLow"));
#if UNITY_2019_1_OR_NEWER
            WINDOWS_PLAYER_LOG_PATH = Path.Combine(appDataLocalLowPath, "{0}", "{1}", "Player.log");
            WINDOWS_PLAYER_LOG_PREV_PATH = Path.Combine(appDataLocalLowPath, "{0}", "{1}", "Player-prev.log");
#else
            WINDOWS_PLAYER_LOG_PATH = Path.Combine(appDataLocalLowPath, "{0}", "{1}", "output_log.txt");
            // There is no previous player log file for output_log.txt, so use its path as a stub.
            WINDOWS_PLAYER_LOG_PREV_PATH = Path.Combine(appDataLocalLowPath, "{0}", "{1}", "output_log.txt");
#endif

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    PACKAGE_MANAGER_LOG_PATH = WINDOWS_PACKAGE_MANAGER_LOG_PATH;
                    EDITOR_LOG_PATH = WINDOWS_EDITOR_LOG_PATH;
                    EDITOR_LOG_PREV_PATH = WINDOWS_EDITOR_LOG_PREV_PATH;
                    PLAYER_LOG_PATH = WINDOWS_PLAYER_LOG_PATH;
                    PLAYER_LOG_PREV_PATH = WINDOWS_PLAYER_LOG_PREV_PATH;
                    break;
                case RuntimePlatform.OSXEditor:
                    PACKAGE_MANAGER_LOG_PATH = MACOS_PACKAGE_MANAGER_LOG_PATH;
                    EDITOR_LOG_PATH = MACOS_EDITOR_LOG_PATH;
                    EDITOR_LOG_PREV_PATH = MACOS_EDITOR_LOG_PREV_PATH;
                    PLAYER_LOG_PATH = MACOS_PLAYER_LOG_PATH;
                    PLAYER_LOG_PREV_PATH = MACOS_PLAYER_LOG_PREV_PATH;
                    break;
                case RuntimePlatform.LinuxEditor:
                    PACKAGE_MANAGER_LOG_PATH = LINUX_PACKAGE_MANAGER_LOG_PATH;
                    EDITOR_LOG_PATH = LINUX_EDITOR_LOG_PATH;
                    EDITOR_LOG_PREV_PATH = LINUX_EDITOR_LOG_PREV_PATH;
                    PLAYER_LOG_PATH = LINUX_PLAYER_LOG_PATH;
                    PLAYER_LOG_PREV_PATH = LINUX_PLAYER_LOG_PREV_PATH;
                    break;
                default:
                    break;
            }
        }

        public static string GetPackageManagerLogPath()
        {
            return PACKAGE_MANAGER_LOG_PATH;
        }

        public static string GetEditorLogPath()
        {
            return EDITOR_LOG_PATH;
        }

        public static string GetEditorLogPrevPath()
        {
            return EDITOR_LOG_PREV_PATH;
        }

        public static string GetPlayerLogPath()
        {
            return String.Format(PLAYER_LOG_PATH, Application.companyName, Application.productName);
        }

        public static string GetPlayerLogPrevPath()
        {
            return String.Format(PLAYER_LOG_PREV_PATH, Application.companyName, Application.productName);
        }
    }
}

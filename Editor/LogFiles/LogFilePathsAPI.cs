using System;
using UnityEngine;

namespace PumpEditor
{
    // File paths based on https://docs.unity3d.com/2019.4/Documentation/Manual/LogFiles.html
    public static class LogFilePathsAPI
    {
        private const string WINDOWS_PACKAGE_MANAGER_LOG_PATH = "C:\\Users\\{0}\\AppData\\Local\\Unity\\Editor\\upm.log";
        private const string WINDOWS_EDITOR_LOG_PATH = "C:\\Users\\{0}\\AppData\\Local\\Unity\\Editor\\Editor.log";
        private const string WINDOWS_EDITOR_LOG_PREV_PATH = "C:\\Users\\{0}\\AppData\\Local\\Unity\\Editor\\Editor-prev.log";
        // While Unity docs state file name as Player.log, it seems to be an error in the docs.
        private const string WINDOWS_PLAYER_LOG_PATH = "C:\\Users\\{0}\\AppData\\LocalLow\\{1}\\{2}\\output_log.txt";

        public static string GetPackageManagerLogPath()
        {
            return String.Format(WINDOWS_PACKAGE_MANAGER_LOG_PATH, Environment.UserName);
        }

        public static string GetEditorLogPath()
        {
            return String.Format(WINDOWS_EDITOR_LOG_PATH, Environment.UserName);
        }

        public static string GetEditorLogPrevPath()
        {
            return String.Format(WINDOWS_EDITOR_LOG_PREV_PATH, Environment.UserName);
        }

        public static string GetPlayerLogPath()
        {
            return String.Format(WINDOWS_PLAYER_LOG_PATH, Environment.UserName, Application.companyName, Application.productName);
        }
    }
}

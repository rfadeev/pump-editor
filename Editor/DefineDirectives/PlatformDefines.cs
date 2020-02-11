namespace PumpEditor
{
    public static class PlatformDefines
    {
        public static bool UnityEditorDefined
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityEditorWinDefined
        {
            get
            {
#if UNITY_EDITOR_WIN
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityEditorOsxDefined
        {
            get
            {
#if UNITY_EDITOR_OSX
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityEditorLinuxDefined
        {
            get
            {
#if UNITY_EDITOR_LINUX
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityStandaloneOsxDefined
        {
            get
            {
#if UNITY_STANDALONE_OSX
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityStandaloneWinDefined
        {
            get
            {
#if UNITY_STANDALONE_WIN
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityStandaloneLinuxDefined
        {
            get
            {
#if UNITY_STANDALONE_LINUX
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityStandaloneDefined
        {
            get
            {
#if UNITY_STANDALONE
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityWiiDefined
        {
            get
            {
#if UNITY_WII
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityIosDefined
        {
            get
            {
#if UNITY_IOS
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityIphoneDefined
        {
            get
            {
#if UNITY_IPHONE
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityAndroidDefined
        {
            get
            {
#if UNITY_ANDROID
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityPs4Defined
        {
            get
            {
#if UNITY_PS4
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityXboxoneDefined
        {
            get
            {
#if UNITY_XBOXONE
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityAdsDefined
        {
            get
            {
#if UNITY_ADS
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityAnalyticsDefined
        {
            get
            {
#if UNITY_ANALYTICS
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityAssertionsDefined
        {
            get
            {
#if UNITY_ASSERTIONS
                return true;
#else
                return false;
#endif
            }
        }

        public static bool Unity64Defined
        {
            get
            {
#if UNITY_64
                return true;
#else
                return false;
#endif
            }
        }
    }
}

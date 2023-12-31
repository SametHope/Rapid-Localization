﻿using UnityEngine;

namespace SametHope.RapidLocalization
{
    /// <summary>
    /// This class holds options and data set on the editor and is used by both editor and runtime calls.
    /// </summary>
    [System.Serializable]
    public class LocalizationSettings : ScriptableObject
    {
        // These constants are here instead of LocalizationEditorUtils as they are also used by runtime scripts.
        public const string LOCALIZATION_FOLDER_NAME = "RapidLocalization";
        public const string LOCALIZATION_FOLDER_PATH = "Assets/Resources/RapidLocalization";

        public const string SETTINGS_FILE_NAME = "RapidLocalizationSettings.asset";
        public const string SETTINGS_FILE_PATH = "Assets/Resources/RapidLocalization/RapidLocalizationSettings.asset";

        public const string RESOURCES_FOLDER_NAME = "Resources";
        public const string RESOURCES_FOLDER_PATH = "Assets/Resources";

        public const string ASSETS_FOLDER= "Assets";


        public static LocalizationSettings Instance;

        [Tooltip("Id of the Google Spreadsheet.")]
        public string TableID;

        [Tooltip("Name of the spreadsheet. Only used for saving the file to the disk.")]
        public string SheetName;

        [Tooltip("Id of the spreadsheet. Used for downloading the sheet from the web.")]
        public long SheetID;

        [Tooltip("Folder to save the spreadsheet. This is hardcoded and will recreate itself if it is deleted just like localization settings.")]
        public Object DownloadFolder;

        [Tooltip("Automatically initializes the manager before the first scene is loaded, requiring no manual initialization.")]
        public bool AutoInitialize = true;

        [Tooltip("Automatically updates the language using PlayerPrefs after initialization.\n" +
            "Falls back to the system language if PlayerPrefs entry is absent or not supported.")]
        public bool AutoUpdateLanguage = true;

        [Tooltip("Display localization keys instead of texts on localizer components. Changing the key will update the displayed text accordingly..")]
        public bool UseKeysAsText = false;
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(LocalizationSettings))]
    public class LocalizationSyncronizerDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            UnityEditor.EditorGUILayout.HelpBox("Do not modify values directly from here.", UnityEditor.MessageType.Warning);
        }
    }
#endif
}
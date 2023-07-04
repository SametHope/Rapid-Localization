using UnityEngine;

namespace SametHope.RapidLocalization
{
    public class LocalizationSettings : ScriptableObject
    {
        // Note: PATHs are just folders, FILEs are, well, files. And NAMEs are, names...

        public const string LOCALIZATION_FOLDER_NAME = "RapidLocalization";
        public const string LOCALIZATION_FOLDER_PATH = "Assets/Resources/RapidLocalization";

        public const string SETTINGS_FILE_NAME = "RapidLocalizationSettings.asset";
        public const string SETTINGS_FILE_PATH = "Assets/Resources/RapidLocalization/RapidLocalizationSettings.asset";

        public const string RESOURCES_FOLDER_PATH = "Assets/Resources";

        public static LocalizationSettings Instance;

        [Tooltip("Table id of the Google Spreadsheet.")]
        public string TableID;

        [Tooltip("Name of the spreadsheet. Only used for saving the file to the disk.")]
        public string SheetName;

        [Tooltip("Id of the spreadsheet. Used for downloading the sheet from the web.")]
        public long SheetId;

        [Tooltip("Folder to save the spreadsheet. This is hardcoded and will recreate itself if it is deleted just like localization settings.")]
        public Object DownloadFolder;

        [Tooltip("Automatically initializes the manager before the first scene is loaded.")]
        public bool AutoInitialize = true;

        [Tooltip("Automatically updates the language using PlayerPrefs after initialization. \n" +
            "If there is no PlayerPrefs entry, tries to use system language, if that is not supported, uses the fallback language. \n" +
            "Also saves the language to PlayerPrefs for further use.")]
        public bool AutoUpdateLanguage = true;
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
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace SametHope.RapidLocalization.Editor
{
    public static class LocalizationEditorUtils
    {
        public static bool ResourcesFolderExists => AssetDatabase.IsValidFolder(LocalizationSettings.RESOURCES_FOLDER_PATH) == true;
        public static bool LocalizationFolderExists => AssetDatabase.IsValidFolder(LocalizationSettings.LOCALIZATION_FOLDER_PATH) == true;
        public static bool SettingsAssetExists => LoadSettingsAsset() != null;

        public static LocalizationSettings LoadSettingsAsset()
        {
            return AssetDatabase.LoadAssetAtPath<LocalizationSettings>(LocalizationSettings.SETTINGS_FILE_PATH);
        }
        public static Object LoadLocalizationFolder()
        {
            return AssetDatabase.LoadAssetAtPath<Object>(LocalizationSettings.LOCALIZATION_FOLDER_PATH);
        }

        public static void CreateResourcesFolderAndRefresh()
        {
            AssetDatabase.CreateFolder(LocalizationSettings.ASSETS_FOLDER, LocalizationSettings.RESOURCES_FOLDER_NAME);
            AssetDatabase.Refresh();
        }
        public static void CreateLocalizationFolderAndRefresh()
        {
            AssetDatabase.CreateFolder(LocalizationSettings.RESOURCES_FOLDER_PATH, LocalizationSettings.LOCALIZATION_FOLDER_NAME);
            AssetDatabase.Refresh();
        }
        public static void CreateSettingsAssetAndRefresh(LocalizationSettings settings)
        {
            AssetDatabase.CreateAsset(settings, LocalizationSettings.SETTINGS_FILE_PATH);
            AssetDatabase.Refresh();
        }
    }
}

#endif
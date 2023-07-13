#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace SametHope.RapidLocalization.Editor
{

    public static class LocalizationEditorInitializer
    {
        /// <summary>
        /// This method will automatically check and create localization folder and settings asset, logging relevant messages if needed.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void SecureEditorFilesCreate()
        {
            AssetDatabase.Refresh();

            if (!LocalizationEditorUtils.ResourcesFolderExists)
            {
                Debug.Log($"Creating Resources folder: {LocalizationSettings.RESOURCES_FOLDER_PATH}");
                LocalizationEditorUtils.CreateResourcesFolderAndRefresh();
            }

            if (!LocalizationEditorUtils.LocalizationFolderExists)
            {
                Debug.Log($"Creating Localization folder: {LocalizationSettings.LOCALIZATION_FOLDER_PATH}");
                LocalizationEditorUtils.CreateLocalizationFolderAndRefresh();
            }

            if (!LocalizationEditorUtils.SettingsAssetExists)
            {
                Debug.Log($"Creating settings asset for localization: {LocalizationSettings.SETTINGS_FILE_PATH}");
                LocalizationEditorUtils.CreateSettingsAssetAndRefresh(ScriptableObject.CreateInstance<LocalizationSettings>());
            }

            LocalizationSettings.Instance = LocalizationEditorUtils.LoadSettingsAsset();
            LocalizationSettings.Instance.DownloadFolder = LocalizationEditorUtils.LoadLocalizationFolder();
        }

        /// <summary>
        /// This method will check and create localization folder and settings asset, logging relevant messages if needed.
        /// </summary>
        public static void SecureEditorFilesMissing()
        {
            AssetDatabase.Refresh();

            if (!LocalizationEditorUtils.ResourcesFolderExists)
            {
                Debug.Log($"{LocalizationSettings.RESOURCES_FOLDER_PATH} is missing, creating it.");
                LocalizationEditorUtils.CreateResourcesFolderAndRefresh();
            }

            if (!LocalizationEditorUtils.LocalizationFolderExists)
            {
                Debug.Log($"{LocalizationSettings.LOCALIZATION_FOLDER_PATH} is missing, creating it.");
                LocalizationEditorUtils.CreateLocalizationFolderAndRefresh();
            }

            if (!LocalizationEditorUtils.SettingsAssetExists)
            {
                Debug.Log($"{LocalizationSettings.SETTINGS_FILE_PATH} is missing, creating it.");
                LocalizationEditorUtils.CreateSettingsAssetAndRefresh(ScriptableObject.CreateInstance<LocalizationSettings>());
            }

            LocalizationSettings.Instance = LocalizationEditorUtils.LoadSettingsAsset();
            LocalizationSettings.Instance.DownloadFolder = LocalizationEditorUtils.LoadLocalizationFolder();
        }
    }
}

#endif
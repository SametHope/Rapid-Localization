#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace SametHope.RapidLocalization.Editor
{
    /// <summary>
    /// This class will create localization folder and settings asset for the first time, making <see cref="LocalizationSyncronizer"/> usable.
    /// </summary>
    public static class LocalizationEditorInitializer
    {
        [InitializeOnLoadMethod]
        public static void InitializeEditorFiles()
        {
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
    }
}

#endif
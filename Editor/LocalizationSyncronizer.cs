#if UNITY_EDITOR

using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace SametHope.RapidLocalization.Editor
{
    /// <summary>
    /// Downloads spritesheets from Google Spreadsheet and saves them to Resources as text file(s).
    /// </summary>
    public class LocalizationSyncronizer : EditorWindow
    {
        // This whole editor window and task system could be replaced with an simple gameObject and coroutines
        // but I made it this way so it is not dependent on gameObjects/monoBehaviours.

        #region Create Window
        [MenuItem("Tools/SametHope/Localization Syncronizer")]
        public static void CreateWindow()
        {
            var window = GetWindow(typeof(LocalizationSyncronizer), true, "Localization Syncronizer", true);
            window.minSize = new Vector2(380, 220);
        }
        #endregion

        #region Create/Load Data
        private void OnEnable()
        {
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
        #endregion

        private void OnGUI()
        {
            // If for some reason Localization folder and/or settings is gone
            if (LocalizationSettings.Instance == null)
            {
                OnEnable();
            }

            SerializedObject serDataObj = new SerializedObject(LocalizationSettings.Instance);
            serDataObj.Update();

            GUILayout.BeginVertical("Box");

            // --------------------------- Table ID ---------------------------
            SerializedProperty idSP = serDataObj.FindProperty(nameof(LocalizationSettings.TableID));
            EditorGUILayout.PropertyField(idSP);

            // --------------------------- Sheet Data ---------------------------
            EditorGUILayout.PropertyField(serDataObj.FindProperty(nameof(LocalizationSettings.SheetName)));
            EditorGUILayout.PropertyField(serDataObj.FindProperty(nameof(LocalizationSettings.SheetId)));

            // --------------------------- Folder ---------------------------
            var folderSP = serDataObj.FindProperty(nameof(LocalizationSettings.DownloadFolder));
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(folderSP);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // --------------------------- Options ---------------------------
            EditorGUILayout.PropertyField(serDataObj.FindProperty(nameof(LocalizationSettings.AutoInitialize)));
            if (LocalizationSettings.Instance.AutoInitialize)
            {
                EditorGUILayout.PropertyField(serDataObj.FindProperty(nameof(LocalizationSettings.AutoUpdateLanguage)));
            }
            else
            {
                LocalizationSettings.Instance.AutoUpdateLanguage = false;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(serDataObj.FindProperty(nameof(LocalizationSettings.AutoUpdateLanguage)));
                EditorGUI.EndDisabledGroup();
            }
            if (!LocalizationSettings.Instance.AutoInitialize || !LocalizationSettings.Instance.AutoUpdateLanguage)
            {
                EditorGUILayout.HelpBox("I highly recommend enabling these options. Be sure to hover over them to make sure you know what they do.", MessageType.Info);
            }

            GUILayout.EndVertical();

            serDataObj.ApplyModifiedProperties();

            if (GUILayout.Button("Syncronize!"))
            {
                Syncronize();
            }
        }

        #region Syncronizing
        private static Task _currentTask = null;

        /// <summary>
        /// Syncronize spreadsheets on the project files and the google sheets.
        /// </summary>
        private static async void Syncronize()
        {
            if (_currentTask != null && _currentTask.Status.Equals(TaskStatus.Running))
            {
                Debug.LogError("Syncronization request is ignored as last syncronization request is still being proccessed.</color>");
                return;
            }

            Task syncTask = InternalSyncronize();
            try
            {
                _currentTask = syncTask;
                await syncTask;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        private static async Task InternalSyncronize()
        {
            string url = $"https://docs.google.com/spreadsheets/d/{LocalizationSettings.Instance.TableID}/export?format=csv&gid={LocalizationSettings.Instance.SheetId}";
            UnityWebRequest webReq = UnityWebRequest.Get(url);
            webReq.redirectLimit = 1;
            webReq.timeout = 10;
            
            Debug.Log($"<color=yellow>Downloading spreadsheet: </color><color=white>{url} </color>");
            
            webReq.SendWebRequest();

            while (!webReq.isDone)
            {
                await Task.Yield();
            }

            if (webReq.result == UnityWebRequest.Result.Success)
            {
                string fileSavePath = System.IO.Path.Combine(LocalizationSettings.LOCALIZATION_FOLDER_PATH, LocalizationSettings.Instance.SheetName + ".txt");
                System.IO.File.WriteAllBytes(fileSavePath, webReq.downloadHandler.data);

                Debug.Log($"<color=yellow>Sheet </color><color=white>{LocalizationSettings.Instance.SheetName} </color><color=yellow>has been downloaded to </color><color=white>{fileSavePath} </color>");
            }
            else
            {
                Debug.LogError($"<color=yellow>Download failed: </color><color=white>Result: {webReq.result} - Error: {webReq.error} - Response Code: ({webReq.responseCode}) </color>");
                Debug.LogError($"Are you sure that the sheets are not private and the ids are correct?");
                if (webReq.responseCode.ToString().StartsWith("3"))
                {
                    Debug.LogError($"Redirection limit ({webReq.redirectLimit}) should not be exceeded in normal cases. You probably didn't set the sheet publicly visible.");
                }
            }
            AssetDatabase.Refresh();
        }
        #endregion
    }
}

#endif

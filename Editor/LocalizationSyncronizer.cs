#if UNITY_EDITOR

using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace SametHope.RapidLocalization.Editor
{
    /// <summary>
    /// Downloads spritesheets from Google Spreadsheets and saves them to the disk as text file(s).
    /// </summary>
    public class LocalizationSyncronizer : EditorWindow
    {
        #region Editor Window

        // Note: This whole editor window and task system could be replaced with an simple gameObject and coroutines
        // but I made it this way so it is not dependent on gameObjects/monoBehaviours and will not bloat the scene.

        [MenuItem("Tools/SametHope/Localization Syncronizer")]
        public static void CreateWindow()
        {
            var window = GetWindow(typeof(LocalizationSyncronizer), true, "Localization Syncronizer", true);
            window.minSize = new Vector2(380, 220);
        }

        private void OnEnable()
        {
            LocalizationEditorInitializer.SecureEditorFilesMissing();
        }

        private void OnGUI()
        {
            // If for some reason Localization folder and/or settings is gone, we want to create them again.
            if (LocalizationSettings.Instance == null)
            {
                LocalizationEditorInitializer.SecureEditorFilesMissing();
            }

            SerializedObject serDataObj = new SerializedObject(LocalizationSettings.Instance);
            serDataObj.Update();

            GUILayout.BeginVertical("Box");

            // --------------------------- Table ID ---------------------------
            EditorGUILayout.PropertyField(serDataObj.FindProperty(nameof(LocalizationSettings.TableID)));

            // --------------------------- Sheet Name and ID ---------------------------
            EditorGUILayout.PropertyField(serDataObj.FindProperty(nameof(LocalizationSettings.SheetName)));
            EditorGUILayout.PropertyField(serDataObj.FindProperty(nameof(LocalizationSettings.SheetID)));

            // --------------------------- Folder ---------------------------
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serDataObj.FindProperty(nameof(LocalizationSettings.DownloadFolder)));
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
                EditorGUILayout.HelpBox("I highly recommend enabling these two options. Be sure to hover over them to make sure you know what they do before unticking them.", MessageType.Info);
            }

            GUILayout.EndVertical();

            serDataObj.ApplyModifiedProperties();

            // --------------------------- Buttons ---------------------------

            if (GUILayout.Button("Syncronize!"))
            {
                Syncronize();
            }

            if (EditorGUILayout.LinkButton("Click here if you are having trouble with ids."))
            {
                Application.OpenURL("https://github.com/SametHope/Rapid-Localization#google-sheets");
            }
        }
        #endregion

        #region Syncronization Logic
        private static Task _currentTask = null;

        /// <summary>
        /// Syncronize spreadsheets on the project files and the google sheets.
        /// </summary>
        private static async void Syncronize()
        {
            if (_currentTask != null && _currentTask.Status.Equals(TaskStatus.Running))
            {
                Debug.LogWarning("Ignored syncronization request is  as last syncronization request is still being proccessed.</color>");
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
            string url = $"https://docs.google.com/spreadsheets/d/{LocalizationSettings.Instance.TableID}/export?format=csv&gid={LocalizationSettings.Instance.SheetID}";
            UnityWebRequest webReq = UnityWebRequest.Get(url);
            webReq.redirectLimit = 1; // 1 is just enough to download the file.
            webReq.timeout = 10; // Just in case.

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
                
                if (webReq.responseCode.ToString().StartsWith("3"))
                {
                    Debug.LogError($"Redirection limit ({webReq.redirectLimit}) should not be exceeded in normal cases. You probably didn't set the sheet publicly visible.");
                }
                else
                {
                    Debug.LogError($"Are you sure that the sheets are not private and the ids are correct?");
                }
            }

            AssetDatabase.Refresh();
        }
        #endregion
    }
}

#endif

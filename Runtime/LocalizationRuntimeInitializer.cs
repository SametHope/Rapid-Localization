using UnityEngine;

namespace SametHope.RapidLocalization
{
    public static class LocalizationRuntimeInitializer
    {
        /// <summary>
        /// This method will clear the dictionary if it was not cleaned by the domain reloading automatically.
        /// This is handy when enter play mode options are being used.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void CleanDictionaryIfNeeded()
        {
            if (LocalizationManager.Dictionary.Count > 0)
            {
                LocalizationManager.Dictionary.Clear();
            }
        }

        /// <summary>
        /// This method makes it so the localization will initialize itself 
        /// without need of any manual call or gameobject and remember the last sessions language.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            // If for some reason data file is missing on the resources folder, we can not access options so we will just return.
            var settings = GetSettings();
            if (settings == null) return;
            LocalizationSettings.Instance = settings;

            if (LocalizationSettings.Instance.AutoInitialize)
            {
                Debug.Log("Initializing RapidLocalization.");
                LocalizationManager.InitializeDictionary();

                if (LocalizationSettings.Instance.AutoUpdateLanguage)
                {
                    LocalizationManager.LoadLanguage();
                    LocalizationManager.SaveLanguage();
                }
            }
        }

        private static LocalizationSettings GetSettings()
        {
            var objs = Resources.FindObjectsOfTypeAll<LocalizationSettings>();
            if(objs.Length == 0) return null;
            else return objs[0];
        }
    }
}

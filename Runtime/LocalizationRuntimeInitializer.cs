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
            var objs = Resources.FindObjectsOfTypeAll<LocalizationSettings>();

            if (objs.Length == 0)
            {
                // Should never happen, but it is better to know it if it does.
                Debug.LogWarning("No settings asset for the Rapid Localization was found. Preferences such as auto initialize and auto language update will be ignored.");
            }
            else if (objs.Length == 1)
            {
                LocalizationSettings.Instance = objs[0];

                if (LocalizationSettings.Instance.AutoInitialize)
                {
                    Debug.Log("Initializing Rapid Localization.");
                    LocalizationManager.InitializeDictionary();

                    if (LocalizationSettings.Instance.AutoUpdateLanguage)
                    {
                        LocalizationManager.LoadLanguage();
                        LocalizationManager.SaveLanguage();
                    }
                }
            }
            else
            {
                // Should never happen, but it is better to know it if it does.
                Debug.LogWarning($"More than one Rapid Localization settings asset has been found, this may cause unexpected behaviour. Please find the duplicate and delete it.");
            }
        }
    }
}

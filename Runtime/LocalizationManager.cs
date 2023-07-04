using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SametHope.RapidLocalization
{
    public static class LocalizationManager
    {
        /// <summary>
        /// This value is used as both the default language and the emergency language, meaning it will be used when a key is missing from a language.
        /// </summary>
        public const string FALLBACK_LANGUAGE = "English";

        /// <summary>
        /// Get or set the language. Setting it will fire the <see cref="LanguageChanged"/> event.
        /// </summary>
        public static string Language
        {
            get => _language;
            set
            {
                if (!SupportedLanguages.Contains(value))
                {
                    Debug.LogError($"Could not switch to language ({value}) from ({_language}). This might be because of uninitialized localization dictionary, non-downloaded sheets or simply a typo.");
                    return;
                }
                _language = value;
                LanguageChanged.Invoke();
            }
        }
        private static string _language = FALLBACK_LANGUAGE;

        /// <summary>
        /// Fired every time <see cref="Language"/> is set.
        /// </summary>
        public static System.Action LanguageChanged = delegate { };

        #region Initialization
        /// <summary>
        /// Localization dictionary, the runtime object that holds all the localization data."/>
        /// </summary>
        public static readonly Dictionary<string, Dictionary<string, string>> Dictionary = new();

        /// <summary>
        /// A list of languages provided in the spreadsheets, initialized with the localization dictionary.
        /// </summary>
        public static readonly List<string> SupportedLanguages = new();

        /// <summary>
        /// Initialize the localization dictionary. Reads localization spreadsheets and loads them to the memory into the <see cref="Dictionary"/> for further use in the session.
        /// </summary>
        public static void InitializeDictionary()
        {
            if (Dictionary.Count > 0)
            {
                Debug.LogWarning($"Initialization warning: You are trying to initialize the localization dictionary when it is already initialized.");
                return;
            }

            // Sheets are saved as TextAssets in the Resources/Localization. There should be no other TextAssets in there.
            TextAsset[] textAssets = Resources.LoadAll<TextAsset>(LocalizationSettings.LOCALIZATION_FOLDER_NAME);

            if(textAssets.Length == 0)
            {
                Debug.LogError($"Initialization failed: You are trying to initialize the localization dictionary when there are no text assets provided in the {LocalizationSettings.LOCALIZATION_FOLDER_PATH}");
                return;
            }

            foreach (TextAsset textAsset in textAssets)
            {
                string text = textAsset.text.Replace("\r\n", "\n").Replace("\"\"", "[_quote_]");
                MatchCollection matches = Regex.Matches(text, "\"[\\s\\S]+?\"");

                // We are casting explicitly so if it fails to cast on runtime (for some reason?) we will know the cast was intentional.
                foreach (Match match in matches.Cast<Match>())
                {
                    text = text.Replace(match.Value, match.Value.Replace("\"", null).Replace(",", "[_comma_]").Replace("\n", "[_newline_]"));
                }

                // There are probably more specific cases though that I don't know or will include them.
                text = text.Replace("。", "。 ").Replace("、", "、 ").Replace("：", "： ").Replace("！", "！ ").Replace("（", " （").Replace("）", "） ").Trim();

                List<string> lines = text.Split('\n').Where(i => i != "").ToList();
                List<string> languages = lines[0].Split(',').Select(i => i.Trim()).ToList();

                // Add languages to the main dict.
                for (var i = 1; i < languages.Count; i++)
                {
                    if (!Dictionary.ContainsKey(languages[i]))
                    {
                        // Instantiate a dictionary for each language (which are also in a dictionary as seen).
                        Dictionary.Add(languages[i], new Dictionary<string, string>());
                        SupportedLanguages.Add(languages[i]);
                    }
                }

                // Add translations to the language dicts.
                for (var i = 1; i < lines.Count; i++)
                {
                    var columns = lines[i].Split(',').Select(j => j.Trim()).Select(j => j.Replace("[_quote_]", "\"").Replace("[_comma_]", ",").Replace("[_newline_]", "\n")).ToList();
                    var key = columns[0];

                    if (key == "") continue;

                    for (var j = 1; j < languages.Count; j++)
                    {
                        // Add words and their translations to the relevant language dictionaries.
                        if (Dictionary[languages[j]].ContainsKey(key))
                        {
                            Debug.LogWarning($"Initialization warning: An item with the same key has already been added ({key}) to the ({languages[j]}). Are there duplicate sheets or keys?");
                        }
                        else
                        {
                            Dictionary[languages[j]].Add(key, columns[j]);
                        }
                    }
                }
            }
        }
        #endregion

        #region Localization
        /// <summary>
        /// Get localized value by localization key for the current language.
        /// </summary>
        public static string Localize(string localizationKey)
        {
            if (Dictionary.Count == 0)
            {
                Debug.Log($"The localization dictionary is not initialized. Trying to initialize it now.");
                InitializeDictionary();
            }

            bool languageIsMissing = !SupportedLanguages.Contains(Language);
            if (languageIsMissing)
            {
                throw new KeyNotFoundException($"Failed Translation: Language not found: ({Language})");
            }

            bool translationIsFound = IsEntryValid(Language, localizationKey);
            if (!translationIsFound)
            {
                bool fallbackLanguageSavesTheDay = IsEntryValid(FALLBACK_LANGUAGE, localizationKey);
                if (fallbackLanguageSavesTheDay)
                {
                    Debug.LogWarning($"Translation warning: Localization key entry not found: ({localizationKey}) ({Language}). Using fallback language ({FALLBACK_LANGUAGE}) instead.");
                    return Dictionary[FALLBACK_LANGUAGE][localizationKey];
                }
                else
                {
                    Debug.LogError($"Translation failed: Localization key entry not found: ({localizationKey}) ({Language})." +
                        $" Fallback language ({FALLBACK_LANGUAGE}) also doesn't have the key entry." +
                        $" Returning the localization key ({localizationKey}) as translation so it may ease debugging.");
                    return localizationKey;
                }
            }

            return Dictionary[Language][localizationKey];
        }

        /// <summary>
        /// Get localized value by localization key for the current language with parameters.
        /// </summary>
        public static string Localize(string localizationKey, params object[] args)
        {
            var pattern = Localize(localizationKey);

            return string.Format(pattern, args);
        }

        private static bool IsEntryValid(string language, string key)
        {
            return Dictionary[language].ContainsKey(key) && Dictionary[language][key] != "";
        }
        #endregion

        #region PlayerPrefs
        private const string _PREFKEY_LANGUAGE = "Language";

        /// <summary>
        /// Updates the language to the language that is saved in the PlayerPrefs.
        /// If no PlayerPref is set, tries to use first the system language, then the fallback language.
        /// <br></br>
        /// Needless to say, triggers <see cref="LanguageChanged"/> event.
        /// </summary>
        public static void LoadLanguage()
        {
            Language = PlayerPrefs.GetString(_PREFKEY_LANGUAGE, GetSupportedLanguageFromSystem());
        }

        /// <summary>
        /// Saves the current language to the player prefs.
        /// </summary>
        public static void SaveLanguage()
        {
            PlayerPrefs.SetString(_PREFKEY_LANGUAGE, Language);
        }

        /// <summary>
        /// Saves the given language to the player prefs.
        /// </summary>
        public static void SaveLanguage(string language)
        {
            PlayerPrefs.SetString(_PREFKEY_LANGUAGE, language);
        }

        /// <summary>
        /// Checks if a key for language localization exists.
        /// </summary>
        public static bool HasSavedLanguage()
        {
            return PlayerPrefs.HasKey(_PREFKEY_LANGUAGE);
        }

        /// <summary>
        /// Gets the saved language on the PlayerPrefs.
        /// </summary>
        public static string GetSavedLanguage()
        {
            if (!HasSavedLanguage()) Debug.LogWarning("There were no saved language on the PlayerPrefs when one was requested. \n" +
                "This will return an empty string which may cause unwanted and hard to track behaviours. \n" +
                "Check if there is a saved language first before relying on this method.");

            return PlayerPrefs.GetString(_PREFKEY_LANGUAGE);
        }


        /// <summary>
        /// Returns whether or not if <see cref="Application.systemLanguage"/> is any of the <see cref="SupportedLanguages"/>.
        /// </summary>
        public static bool IsSystemLanguageSupported()
        {
            string systemLanguage = GetSystemLanguage();
            return SupportedLanguages.Any(supportedLanguage => systemLanguage == supportedLanguage);
        }

        /// <summary>
        /// Returns the string version of the systems language.
        /// </summary>
        public static string GetSystemLanguage()
        {
            return Application.systemLanguage.ToString();
        }

        /// <summary>
        /// Checks if the system language is supported by comparing it with the supported languages.
        /// If it does, returns it. If not, returns the fallback language.
        /// </summary>
        public static string GetSupportedLanguageFromSystem()
        {
            if (IsSystemLanguageSupported()) return GetSystemLanguage();
            else return FALLBACK_LANGUAGE;
        }
        #endregion
    }
}
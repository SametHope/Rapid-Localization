﻿using TMPro;
using UnityEngine;

namespace SametHope.RapidLocalization
{
    /// <summary>
    /// This class makes life easier by automating the localizing proccess.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Localizer_TMP : MonoBehaviour
    {
        [Tooltip("Should localize automatically on start.")]
        public bool LocalizeOnStart = true;

        [Tooltip("Should localize automatically each time language changes.")]
        public bool IgnoreLanguageChangeEvent = false;

        [Tooltip("The key to localize text based on.")]
        public string LocalizationKey;

        [HideInInspector] public TextMeshProUGUI TMP;
        private void OnValidate()
        {
            if (LocalizationSettings.Instance != null && LocalizationSettings.Instance.UseKeysAsText)
            {
                if (TMP == null) TMP = GetComponent<TextMeshProUGUI>();
                TMP.text = LocalizationKey;
            }
        }
        protected void Awake()
        {
            TMP = GetComponent<TextMeshProUGUI>();
            LocalizationManager.LanguageChanged += Localize;
        }
        protected void Start()
        {
            if (LocalizeOnStart) Localize();
        }
        private void OnDestroy()
        {
            LocalizationManager.LanguageChanged -= Localize;
        }


        public void Localize()
        {
            if (IgnoreLanguageChangeEvent) return;
            TMP.text = LocalizationManager.Localize(LocalizationKey, gameObject);
        }
        public void Localize(params object[] args)
        {
            if (IgnoreLanguageChangeEvent) return;
            TMP.text = LocalizationManager.Localize(LocalizationKey, gameObject, args);
        }
    }
}
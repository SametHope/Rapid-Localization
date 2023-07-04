using TMPro;
using UnityEngine;

namespace SametHope.RapidLocalization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Localizer_TextMeshProUGUI : MonoBehaviour
    {
        [Tooltip("Should localize automatically on start.")]
        public bool LocalizeOnStart = true;

        [Tooltip("Should localize automatically each time language changes.")]
        public bool IgnoreLanguageChangeEvent = false;

        [Tooltip("The key to localize text based on.")]
        public string LocalizationKey;

        [HideInInspector] public TextMeshProUGUI TMP;

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
            TMP.text = LocalizationManager.Localize(LocalizationKey);
        }
        public void Localize(params object[] args)
        {
            if (IgnoreLanguageChangeEvent) return;
            TMP.text = LocalizationManager.Localize(LocalizationKey, args);
        }
    }
}
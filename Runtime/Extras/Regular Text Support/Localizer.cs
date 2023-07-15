using UnityEngine;
using UnityEngine.UI;

namespace SametHope.RapidLocalization
{
    /// <summary>
    /// This class makes life easier by automating the localizing proccess.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class Localizer : MonoBehaviour
    {
        [Tooltip("Should localize automatically on start.")]
        public bool LocalizeOnStart = true;

        [Tooltip("Should localize automatically each time language changes.")]
        public bool IgnoreEvent = false;

        [Tooltip("The key to localize text based on.")]
        public string LocalizationKey;

        [HideInInspector] public Text Text;

        private void OnValidate()
        {
            if(LocalizationSettings.Instance != null && LocalizationSettings.Instance.UseKeysAsText)
            {
                if(Text == null) Text = GetComponent<Text>();
                Text.text = LocalizationKey;
            }
        }
        protected void Awake()
        {
            Text = GetComponent<Text>();
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
            if (IgnoreEvent) return;
            Text.text = LocalizationManager.Localize(LocalizationKey, gameObject);
        }
        public void Localize(params object[] args)
        {
            if (IgnoreEvent) return;
            Text.text = LocalizationManager.Localize(LocalizationKey, gameObject, args);
        }
    }
}
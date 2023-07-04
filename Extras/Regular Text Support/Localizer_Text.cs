using UnityEngine;
using UnityEngine.UI;

namespace SametHope.RapidLocalization
{
    [RequireComponent(typeof(Text))]
    public class Localizer_Text : MonoBehaviour
    {
        [Tooltip("Should localize automatically on start.")]
        public bool LocalizeOnStart = true;

        [Tooltip("Should localize automatically each time language changes.")]
        public bool IgnoreEvent = false;

        [Tooltip("The key to localize text based on.")]
        public string LocalizationKey;

        [HideInInspector] public Text Text;

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
            Text.text = LocalizationManager.Localize(LocalizationKey);
        }
        public void Localize(params object[] args)
        {
            if (IgnoreEvent) return;
            Text.text = LocalizationManager.Localize(LocalizationKey, args);
        }
    }
}
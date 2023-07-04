using UnityEngine;

namespace SametHope.RapidLocalization
{
    public abstract class LocalizerBase : MonoBehaviour
    {
        // NOTE
        // Unlike most event systems in Unity, we are not subscribing and unsubscribing inside the OnEnable or OnDisable 
        // but instead do it inside the Awake and OnDestroy methods. This is because we want the disabled objects to update themselves too.

        [Tooltip("Whether or not should the localize method be called automatically.")]
        public bool LocalizeOnStart = true;

        [Tooltip("The key to localize text based on.")]
        public string LocalizationKey;

        private void Awake()
        {
            LocalizationManager.LanguageChanged += Localize;
        }
        private void Start()
        {
            if (LocalizeOnStart) Localize();
        }
        private void OnDestroy()
        {
            LocalizationManager.LanguageChanged -= Localize;
        }

        public abstract void Localize();
        public abstract void Localize(params object[] args);
    }
}


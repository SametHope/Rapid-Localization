using TMPro;
using UnityEngine;

namespace SametHope.RapidLocalization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Localizer_TextMeshProUGUI : LocalizerBase
    {
        public TextMeshProUGUI TMP { get; private set; }

        private void Awake()
        {
            TMP = GetComponent<TextMeshProUGUI>();
        }
        public override void Localize()
        {
            TMP.text = LocalizationManager.Localize(LocalizationKey);
        }
        public override void Localize(params object[] args)
        {
            TMP.text = LocalizationManager.Localize(LocalizationKey, args);
        }
    }
}
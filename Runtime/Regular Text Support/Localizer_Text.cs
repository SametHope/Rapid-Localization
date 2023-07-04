using UnityEngine;
using UnityEngine.UI;

namespace SametHope.RapidLocalization
{
    [RequireComponent(typeof(Text))]
    public class Localizer_Text : LocalizerBase
    {
        public Text Text { get; private set; }

        private void Awake()
        {
            Text = GetComponent<Text>();
        }
        public override void Localize()
        {
            Text.text = LocalizationManager.Localize(LocalizationKey);
        }
        public override void Localize(params object[] args)
        {
            Text.text = LocalizationManager.Localize(LocalizationKey, args);
        }
    }
}
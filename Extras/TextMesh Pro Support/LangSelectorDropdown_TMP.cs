using TMPro;
using UnityEngine;

namespace SametHope.RapidLocalization
{
    /// <summary>
    /// This class makes life easier by making a responsive language selection dropdown.
    /// </summary>
    [RequireComponent(typeof(TMP_Dropdown))]
    public class LangSelectorDropdown_TMP : MonoBehaviour
    {
        private TMP_Dropdown _selectorDropdown;

        private void Awake()
        {
            _selectorDropdown = GetComponent<TMP_Dropdown>();
        }
        private void OnEnable()
        {
            SetDropdownItemFromLanguage();
        }

        private void SetDropdownItemFromLanguage()
        {
            int targetOptionIndex = 0;
            for (int i = 0; i < _selectorDropdown.options.Count; i++)
            {
                var currentOption = _selectorDropdown.options[i];

                if (currentOption.text == LocalizationManager.Language)
                {
                    targetOptionIndex = i;
                    break;
                }
            }

            _selectorDropdown.value = targetOptionIndex;
        }

        /// <summary>
        /// This method must be called when the language dropdown value changes to apply and save the new language.
        /// </summary>
        public void SetLanguageToDropdownItem()
        {
            LocalizationManager.Language = _selectorDropdown.options[_selectorDropdown.value].text;
            LocalizationManager.SaveLanguage();
        }
    }

}

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
            _selectorDropdown.onValueChanged.AddListener(SetLanguageToDropdownItem);
        }
        private void OnDestroy()
        {
            _selectorDropdown.onValueChanged.RemoveListener(SetLanguageToDropdownItem);
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
        /// This method is called when the language dropdown value changes to apply and save the new language.
        /// </summary>
        public void SetLanguageToDropdownItem(int newValue)
        {
            LocalizationManager.Language = _selectorDropdown.options[newValue].text;
            LocalizationManager.SaveLanguage();
        }
    }

}


# Rapid Localization

Rapid Localization is a lightweight Unity plugin that makes adding localization to applications way easier, requiring minimal setup.


## How does it work?

Rapid Localization utilizes Google Sheets to create translation sheets, enabling teams of any size and contributors to work on translations for multiple languages simultaneously or asynchronously.

The plugin features the "Localization Synchronizer" custom editor window, accessed on the "Tools" section, allowing you to easily download the translation spreadsheet(s) to the resources folder to be used in both editor and the builds. The Localization Synchronizer also offers configuration options to customize some of the plugin's behavior.

When your application starts, the LocalizationManager class reads the spreadsheet(s) and loads the translations into memory for the session.

At this point, the plugin can optionally check if a language has been previously saved. If a language is saved, it will be set as the current language. If no language is saved, the plugin will retrieve the player's system language and check if it is a supported language. If it is supported, the system language will be set as the current language and saved. If the system language is not supported, the plugin will fallback to a default language.

When the language is set or changed, listeners are notified and update their languages accordingly. If a translation key is missing in the current language, the fallback language is used. If the fallback is also missing, the key itself is used for easier debugging. Any unintended behavior is logged as warnings or errors, along with helpful tips to resolve them.


## Features

- **Single Editor Window:** Convenient setup and configuration through a single editor window.
  
- **Automatic Initialization:** No need for dummy game objects or manual code calls; the plugin sets up and initializes itself automatically without bloating your project.
  
- **Automatic Language Detection:** Automatically detects, uses, and saves the player's language.
  
- **Runtime Localization:** Switch between different localizations at runtime any time you need.
  
- **UI Localization:** Localize both TextMeshPro (TMP) and legacy UI components.
  
- **Dropdown Language Selection:** Includes prewritten code for a dropdown language selection UI element.
  
- **Debugging:** If something goes wrong or encounters an issue, the plugin tries fixing it and provides useful debug messages. These messages contain information about what went wrong and offer probable fixes.
  
- **Platform Compatibility:** Supports all platforms.
  
- **Version Compatibility:** Supports all Unity versions.
  
- **Clean and Simple Code:** The plugin features clean, well-structured, and documented code.
  
- **Open Source:** The code is open source, allowing for customization.
  
- **Lightweight:** Occupies very little space and contains a handful of scripts.


## Setup

- Add the code into your Unity project, ideally into the Plugins folder. The plugin will automatically create its required files which are a folder called RapidLocalization inside Assets/Resources and RapidLocalizationSettings.asset.
- Open the editor window that we will use to configure the plugin and download the spreadsheets from Google Sheets: Tools -> SametHope -> Localization Syncronizer.
- [Fill in informations of your Google Sheets](www.google.com) and click 'Syncronize!' button to download, format and save the sheet.
- You are virtually done, the plugin is ready to be used. Configure it to your liking and remember to syncronize it when you make changes on the Google Sheets.


## Usage

**To translate UI elements, follow these steps:**

1. Add the 'Localizer' component to the UI element you want to translate.

2. Set the localization key for the 'Localizer' component. This key will be used to update the translation of the UI element when necessary.

3. Optionally, you can configure the 'Localizer' component to not automatically translate on the Start method and to not to listen to language change events.

**To use the pre-made dropdown language selection, follow these steps:**
<br />
1. Add the 'LangSelectorDropdown' component to the dropdown UI element.

2. Set the dropdown options to the available languages.
When the dropdown selection changes, the language will be updated and saved automatically.

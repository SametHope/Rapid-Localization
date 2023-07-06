
# Rapid Localization

Rapid Localization is an efficient and lightweight Unity plugin that simplifies adding localization to applications. It requires minimal setup while offering a comprehensive set of features that absolutely surpasses its size.

## Features

- **Single Editor Window:** Convenient setup and configuration through a single editor window.
  
- **Automatic Initialization:** No need for dummy game objects or manual code calls; the plugin sets up and initializes itself automatically without bloating your project.
  
- **Automatic Language Detection:** Automatically detects, uses, and saves the player's language.
  
- **Runtime Localization:** Switch between different localizations at runtime any time as you need.
  
- **UI Localization:** Localize both TextMeshPro (TMP) and legacy UI components.
  
- **Dropdown Language Selection:** Includes prewritten code for a dropdown language selection UI element.

- **Lightweight:** Occupies very little space and contains a handful of scripts. The scripts are contained within assembly definitions and do not clutter the main assembly.

- **Debugging:** If something goes wrong or encounters an issue, plugin tries fixing it and provides useful debug messages. These messages contain information about what went wrong and offer probable fixes.
  
- **Platform Compatibility:** Supports all platforms.
  
- **Version Compatibility:** Supports all Unity versions.
  
- **Clean and Simple Code:**  Plugin features clean, well-structured and documented code.
  
- **Open Source:** The code is open source, allowing customization.
  

## Setup

- [Setup your translation sheet](https://github.com/SametHope/Rapid-Localization/#Google-Sheets) on the Google Sheets. 
- Add the code into your Unity project, ideally into the Plugins folder. The plugin will automatically create its required files which are a folder called RapidLocalization inside Assets/Resources and RapidLocalizationSettings.asset.
- Open the editor window that we will use to configure the plugin and download the spreadsheets from Google Sheets: [Tools -> SametHope -> Localization Syncronizer.](https://github.com/SametHope/Rapid-Localization/#toolbar) <sub>Note: Your Google Sheet must be public so it can be downloaded.</sub>
- [Fill in informations of your Google Sheets](https://github.com/SametHope/Rapid-Localization/#Google-Sheets) and click 'Syncronize!' button to download, format and save the sheet.
- You are virtually done, the plugin is ready to be used. Configure it to your liking and remember to syncronize it when you make changes on the Google Sheets.


## Usage

**To translate UI elements, follow these steps:**

1. Add the '[Localizer](https://github.com/SametHope/Rapid-Localization/#localizer)' component to the UI element you want to translate.

2. Set the localization key for the 'Localizer' component. This key will be used to update the translation of the UI element when necessary.

3. Optionally, you can configure the 'Localizer' component to not automatically translate on the Start method and to not to listen to language change events.

**To use the pre-made dropdown language selection, follow these steps:**
<br />
1. Add the 'LangSelectorDropdown' component to the dropdown UI element.

2. Set the dropdown options to the available languages.
When the dropdown selection changes, the language will be updated and saved automatically.


## How does it work?

Rapid Localization utilizes Google Sheets to create translation sheets, enabling teams of any size and contributors to work on translations for multiple languages simultaneously or asynchronously.

The plugin features the 'Localization Synchronizer' custom editor window, accessed on the '[Tools](https://github.com/SametHope/Rapid-Localization/#toolbar)' section, allowing you to easily download the translation spreadsheet(s) to the resources folder to be used in both editor and the builds. The Localization Synchronizer also offers configuration options to customize some of the plugin's behavior.

When your application starts, the LocalizationManager class reads the spreadsheet(s) and loads the translations into memory for the session.

At this point, the plugin can optionally check if a language has been previously saved. If a language is saved, it will be set as the current language. If no language is saved, the plugin will retrieve the player's system language and check if it is a supported language. If it is supported, the system language will be set as the current language and saved. If the system language is not supported, the plugin will fallback to a default language.

When the language is set or changed, listeners are notified and update their languages accordingly. If a translation key is missing in the current language, the fallback language is used. If the fallback is also missing, the key itself is used for easier debugging. Any unintended behavior is logged as warnings or errors, along with helpful tips to resolve them.



# Extras

### Toolbar
![2023-07-06 02_39_03-Ayarlar](https://github.com/SametHope/Rapid-Localization/assets/85421686/2abfc679-6b50-4bf9-a3cf-dffc4068b7cd)

This is the toolbar, everyone know what a toolbar is.

### Localization syncronizer
![2023-07-06 00_12_15-Mock Jam Project - MenuScene - WebGL - Unity 2021 3 22f1 Personal _DX11_](https://github.com/SametHope/Rapid-Localization/assets/85421686/b50a2261-0426-44a5-b03f-98c933a4a6d7)

You can and probably should hover over properties to read their tooltips.

### Localizer

![2023-07-06 02_54_15-Temp Medias](https://github.com/SametHope/Rapid-Localization/assets/85421686/c543e923-063b-4ce7-ad5b-e724f2e1e2db)

I don't advise changing any default options.

Google Sheets
--------------

![2023-07-06 04_06_41-Mock Localisation - Google E-Tablolar - Opera](https://github.com/SametHope/Rapid-Localization/assets/85421686/f155d19a-615a-435f-a694-6b760bc331ea)

***You must format your Google Sheet like this for the plugin to work.***

**Green** box contents are Localization Keys <br />
**Red** box contents are Languages <br />
**Blue** box contents are Localization entries <br />


![2023-07-06 02_36_00-Mock Localisation - Google E-Tablolar - Opera](https://github.com/SametHope/Rapid-Localization/assets/85421686/1affad3b-ffbe-4e98-b9b6-26690a4ebd84)

***Ensure that your Google Sheet is public so it can be downloaded.***

**Long red line** is the TableID <br />
**Short red line** is the SheetID <br />


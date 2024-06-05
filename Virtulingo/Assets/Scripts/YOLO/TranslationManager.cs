using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class TranslationManager
{
    private Dictionary<string, Dictionary<string, string>> translationDatabase = new();

    public TranslationManager(TextAsset translationJsonFile)
    {
        if (translationJsonFile != null)
        {
            string json = translationJsonFile.text;
            TranslationData data = JsonConvert.DeserializeObject<TranslationData>(json);

            foreach (var languageObject in data.translations)
            {
                string lang = languageObject.language;

                if (!translationDatabase.ContainsKey(lang))
                {
                    translationDatabase.Add(lang, new Dictionary<string, string>());
                }

                foreach (var labelPair in languageObject.translatedLabels)
                {
                    string originalLabel = labelPair.Key;
                    string translatedLabel = labelPair.Value;
                    translationDatabase[lang].Add(originalLabel, translatedLabel);
                }
            }
        }
    }

    public string GetTranslation(string key, string language)
    {
        if (translationDatabase.ContainsKey(language))
        {
            if (translationDatabase[language].ContainsKey(key))
            {
                return translationDatabase[language][key];
            }
            else
            {
                Debug.LogError("Key not found in translation database: " + key);
                return "";
            }
        }
        else
        {
            Debug.LogError("Language not found in translation database: " + language);
            return "";
        }
    }
}

[System.Serializable]
public class TranslationData
{
    public class LanguageData
    {
        public string language;
        public Dictionary<string, string> translatedLabels;
    }

    public List<LanguageData> translations;
}
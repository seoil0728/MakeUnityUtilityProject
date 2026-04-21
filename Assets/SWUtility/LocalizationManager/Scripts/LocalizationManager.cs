using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using SWUtility.Singleton;

namespace SWUtility.Localization
{
    public class LocalizationManager : GlobalSingleton<LocalizationManager>
    {
        [Header("Settings")]
        [SerializeField] private string csvPath = "Localization/LocalizationData";
        [SerializeField] private Language currentLanguage = Language.KO;

        private Dictionary<string, Dictionary<Language, string>> _localizationData = new Dictionary<string, Dictionary<Language, string>>();

        public Language CurrentLanguage
        {
            get => currentLanguage;
            set
            {
                if (currentLanguage != value)
                {
                    currentLanguage = value;
                    OnLanguageChanged?.Invoke();
                }
            }
        }

        public event Action OnLanguageChanged;

        public override void Initialize()
        {
            if (IsInitialized) return;
            LoadCSV();
            base.Initialize();
        }

        protected override void Awake()
        {
            base.Awake();
            if (IsInitialized == false)
            {
                Initialize();
            }
        }

        public void LoadCSV()
        {
            _localizationData.Clear();
            TextAsset csvFile = Resources.Load<TextAsset>(csvPath);

            if (csvFile == null)
            {
                Debug.LogError($"[LocalizationManager] CSV file not found at Resources/{csvPath}");
                return;
            }

            ParseCSV(csvFile.text);
            Debug.Log($"[LocalizationManager] Successfully loaded {_localizationData.Count} keys.");
        }

        private void ParseCSV(string csvText)
        {
            // 정규식을 이용한 CSV 파싱 (쉼표 포함된 문자열 처리 등)
            string pattern = @"(((?<=\n|^)(?:"".*?""|[^,""\n]*|))(?=,|$))|((?<=,|^)(?:"".*?""|[^,""\n]*|)(?=\n|$))";
            MatchCollection matches = Regex.Matches(csvText, pattern);

            List<string> rowData = new List<string>();
            int columnCount = Enum.GetNames(typeof(Language)).Length + 1; // Key + Languages

            int currentIndex = 0;
            foreach (Match match in matches)
            {
                string value = match.Value.Trim('"').Replace("\"\"", "\"");
                rowData.Add(value);
                currentIndex++;

                if (currentIndex >= columnCount)
                {
                    if (rowData.Count > 0 && !string.IsNullOrEmpty(rowData[0]))
                    {
                        string key = rowData[0];
                        if (key != "Key") // 헤더 제외
                        {
                            var translations = new Dictionary<Language, string>();
                            for (int i = 0; i < Enum.GetNames(typeof(Language)).Length; i++)
                            {
                                if (i + 1 < rowData.Count)
                                {
                                    translations[(Language)i] = rowData[i + 1];
                                }
                            }
                            _localizationData[key] = translations;
                        }
                    }
                    rowData.Clear();
                    currentIndex = 0;
                }
            }
        }

        public string GetLocalizedString(string key)
        {
            if (_localizationData.TryGetValue(key, out var translations))
            {
                if (translations.TryGetValue(currentLanguage, out var result))
                {
                    return result;
                }
            }
            return $"[{key}]"; // 키를 못 찾으면 키 이름을 반환
        }
        
        // 에디터 테스트용 파싱 메서드 (TextAsset 직접 전달)
        public Dictionary<string, Dictionary<Language, string>> TestParse(string csvText)
        {
            var testData = new Dictionary<string, Dictionary<Language, string>>();
            string pattern = @"(((?<=\n|^)(?:"".*?""|[^,""\n]*|))(?=,|$))|((?<=,|^)(?:"".*?""|[^,""\n]*|)(?=\n|$))";
            MatchCollection matches = Regex.Matches(csvText, pattern);

            List<string> rowData = new List<string>();
            int columnCount = Enum.GetNames(typeof(Language)).Length + 1;
            int currentIndex = 0;

            foreach (Match match in matches)
            {
                string value = match.Value.Trim('"').Replace("\"\"", "\"");
                rowData.Add(value);
                currentIndex++;

                if (currentIndex >= columnCount)
                {
                    if (rowData.Count > 0 && !string.IsNullOrEmpty(rowData[0]))
                    {
                        string key = rowData[0];
                        if (key != "Key")
                        {
                            var translations = new Dictionary<Language, string>();
                            for (int i = 0; i < Enum.GetNames(typeof(Language)).Length; i++)
                            {
                                if (i + 1 < rowData.Count)
                                {
                                    translations[(Language)i] = rowData[i + 1];
                                }
                            }
                            testData[key] = translations;
                        }
                    }
                    rowData.Clear();
                    currentIndex = 0;
                }
            }
            return testData;
        }
    }
}

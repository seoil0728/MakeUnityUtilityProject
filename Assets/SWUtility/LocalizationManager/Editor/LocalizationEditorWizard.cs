using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SWUtility.Localization.Editor
{
    public class LocalizationEditorWizard : ScriptableWizard
    {
        public TextAsset csvFile;
        private Vector2 _scrollPosition;
        private Dictionary<string, Dictionary<Language, string>> _testData;

        [MenuItem("SWUtility/Localization/Parsing Wizard")]
        public static void CreateWizard()
        {
            DisplayWizard<LocalizationEditorWizard>("Localization CSV Wizard", "Parse & View");
        }

        private void OnWizardCreate()
        {
            if (csvFile == null)
            {
                Debug.LogError("CSV file is null!");
                return;
            }

            Parse();
        }

        private void OnWizardOtherButton()
        {
            Parse();
        }

        private void Parse()
        {
            // LocalizationManager의 TestParse를 사용하여 로직 검증
            // (Manager가 씬에 없어도 로직만 테스트 가능하도록 설계됨)
            LocalizationManager tempManager = new GameObject("Temp").AddComponent<LocalizationManager>();
            _testData = tempManager.TestParse(csvFile.text);
            DestroyImmediate(tempManager.gameObject);
            
            Debug.Log($"Parsed {_testData.Count} keys from {csvFile.name}");
        }

        protected override bool DrawWizardGUI()
        {
            bool changed = base.DrawWizardGUI();

            if (_testData != null && _testData.Count > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Parsed Data Preview", EditorStyles.boldLabel);
                
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(300));
                
                foreach (var item in _testData)
                {
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.LabelField($"Key: {item.Key}", EditorStyles.whiteLargeLabel);
                    
                    foreach (var langData in item.Value)
                    {
                        EditorGUILayout.LabelField($"{langData.Key}: {langData.Value}");
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space(2);
                }
                
                EditorGUILayout.EndScrollView();
            }

            return changed;
        }
    }
}

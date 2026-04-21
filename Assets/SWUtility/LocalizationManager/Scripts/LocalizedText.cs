using TMPro;
using UnityEngine;

namespace SWUtility.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string localizationKey;

        private TextMeshProUGUI _textMeshPro;

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            UpdateText();
            if (LocalizationManager.Instance != null)
            {
                LocalizationManager.Instance.OnLanguageChanged += UpdateText;
            }
        }

        private void OnDisable()
        {
            if (LocalizationManager.Instance != null)
            {
                LocalizationManager.Instance.OnLanguageChanged -= UpdateText;
            }
        }

        public void UpdateText()
        {
            if (_textMeshPro == null) _textMeshPro = GetComponent<TextMeshProUGUI>();
            
            if (LocalizationManager.Instance != null && !string.IsNullOrEmpty(localizationKey))
            {
                _textMeshPro.text = LocalizationManager.Instance.GetLocalizedString(localizationKey);
            }
        }

        // 인스펙터에서 키를 수동으로 바꿨을 때 바로 반영하기 위한 헬퍼
        public void SetKey(string key)
        {
            localizationKey = key;
            UpdateText();
        }
    }
}

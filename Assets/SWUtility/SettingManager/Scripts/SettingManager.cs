using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using SWUtility.Singleton;

namespace SWUtility.Setting
{
    public class SettingManager : GlobalSingleton<SettingManager>
    {
        private SettingData _data;
        private string _savePath;

        // 이벤트 구독 딕셔너리
        private Dictionary<string, Action<float>> _floatEvents = new Dictionary<string, Action<float>>();
        private Dictionary<string, Action<int>> _intEvents = new Dictionary<string, Action<int>>();
        private Dictionary<string, Action<bool>> _boolEvents = new Dictionary<string, Action<bool>>();
        private Dictionary<string, Action<string>> _stringEvents = new Dictionary<string, Action<string>>();

        // 기본 유니티 설정 키 미리 정의
        public const string KEY_FULLSCREEN = "Sys_FullScreen";
        public const string KEY_QUALITY = "Sys_QualityLevel";
        public const string KEY_VSYNC = "Sys_VSync";

        public override void Initialize()
        {
            if (IsInitialized) return;

            _savePath = Path.Combine(Application.persistentDataPath, "SWUtility_Settings.json");
            LoadSettings();
            RegisterSystemEvents();
            
            base.Initialize();
        }

        protected override void Awake()
        {
            base.Awake();
            if (!IsInitialized)
            {
                Initialize();
            }
        }

        private void RegisterSystemEvents()
        {
            // 유니티 기본 설정들에 대한 이벤트 자체 구독
            AddBoolListener(KEY_FULLSCREEN, (isFull) => Screen.fullScreen = isFull);
            AddIntListener(KEY_QUALITY, (level) => QualitySettings.SetQualityLevel(level));
            AddIntListener(KEY_VSYNC, (vSync) => QualitySettings.vSyncCount = vSync);
            
            // 초기값 적용
            ApplySystemSettings();
        }

        private void ApplySystemSettings()
        {
            // 로드된 데이터를 기반으로 초기 설정 적용
            if (_data.BoolSettings.TryGetValue(KEY_FULLSCREEN, out bool isFull)) Screen.fullScreen = isFull;
            if (_data.IntSettings.TryGetValue(KEY_QUALITY, out int quality)) QualitySettings.SetQualityLevel(quality);
            if (_data.IntSettings.TryGetValue(KEY_VSYNC, out int vsync)) QualitySettings.vSyncCount = vsync;
        }

        #region 데이터 로드 및 저장
        public void SaveSettings()
        {
            try
            {
                string json = JsonMapper.ToJson(_data);
                File.WriteAllText(_savePath, json);
                Debug.Log($"[SettingManager] Settings saved to: {_savePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[SettingManager] Failed to save settings: {e.Message}");
            }
        }

        public void LoadSettings()
        {
            if (File.Exists(_savePath))
            {
                try
                {
                    string json = File.ReadAllText(_savePath);
                    _data = JsonMapper.ToObject<SettingData>(json);
                    
                    if (_data == null) _data = new SettingData();
                    Debug.Log("[SettingManager] Settings loaded successfully.");
                }
                catch (Exception e)
                {
                    Debug.LogError($"[SettingManager] Failed to load settings: {e.Message}");
                    _data = new SettingData();
                }
            }
            else
            {
                Debug.Log("[SettingManager] No save file found, creating new settings.");
                _data = new SettingData();
                SetDefaultSettings();
            }
        }

        private void SetDefaultSettings()
        {
            // 기본 설정값 세팅 (최초 실행 시)
            SetFloat("BGM_Volume", 1.0f);
            SetFloat("SFX_Volume", 1.0f);
            SetBool(KEY_FULLSCREEN, true);
            SetInt(KEY_QUALITY, QualitySettings.GetQualityLevel());
            SetInt(KEY_VSYNC, 0); // 0: Don't Sync, 1: Every V Blank
            SaveSettings();
        }
        #endregion

        #region Float 설정
        public void SetFloat(string key, float value)
        {
            if (_data.FloatSettings.TryGetValue(key, out float current) && Mathf.Approximately(current, value)) return;

            _data.FloatSettings[key] = value;
            if (_floatEvents.TryGetValue(key, out var action))
            {
                action?.Invoke(value);
            }
        }

        public float GetFloat(string key, float defaultValue = 0f)
        {
            return _data.FloatSettings.TryGetValue(key, out float value) ? value : defaultValue;
        }

        public void AddFloatListener(string key, Action<float> callback)
        {
            if (!_floatEvents.ContainsKey(key)) _floatEvents[key] = callback;
            else _floatEvents[key] += callback;
        }

        public void RemoveFloatListener(string key, Action<float> callback)
        {
            if (_floatEvents.ContainsKey(key))
            {
                _floatEvents[key] -= callback;
                if (_floatEvents[key] == null) _floatEvents.Remove(key);
            }
        }
        #endregion

        #region Int 설정
        public void SetInt(string key, int value)
        {
            if (_data.IntSettings.TryGetValue(key, out int current) && current == value) return;

            _data.IntSettings[key] = value;
            if (_intEvents.TryGetValue(key, out var action))
            {
                action?.Invoke(value);
            }
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return _data.IntSettings.TryGetValue(key, out int value) ? value : defaultValue;
        }

        public void AddIntListener(string key, Action<int> callback)
        {
            if (!_intEvents.ContainsKey(key)) _intEvents[key] = callback;
            else _intEvents[key] += callback;
        }

        public void RemoveIntListener(string key, Action<int> callback)
        {
            if (_intEvents.ContainsKey(key))
            {
                _intEvents[key] -= callback;
                if (_intEvents[key] == null) _intEvents.Remove(key);
            }
        }
        #endregion

        #region Bool 설정
        public void SetBool(string key, bool value)
        {
            if (_data.BoolSettings.TryGetValue(key, out bool current) && current == value) return;

            _data.BoolSettings[key] = value;
            if (_boolEvents.TryGetValue(key, out var action))
            {
                action?.Invoke(value);
            }
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return _data.BoolSettings.TryGetValue(key, out bool value) ? value : defaultValue;
        }

        public void AddBoolListener(string key, Action<bool> callback)
        {
            if (!_boolEvents.ContainsKey(key)) _boolEvents[key] = callback;
            else _boolEvents[key] += callback;
        }

        public void RemoveBoolListener(string key, Action<bool> callback)
        {
            if (_boolEvents.ContainsKey(key))
            {
                _boolEvents[key] -= callback;
                if (_boolEvents[key] == null) _boolEvents.Remove(key);
            }
        }
        #endregion

        #region String 설정
        public void SetString(string key, string value)
        {
            if (_data.StringSettings.TryGetValue(key, out string current) && current == value) return;

            _data.StringSettings[key] = value;
            if (_stringEvents.TryGetValue(key, out var action))
            {
                action?.Invoke(value);
            }
        }

        public string GetString(string key, string defaultValue = "")
        {
            return _data.StringSettings.TryGetValue(key, out string value) ? value : defaultValue;
        }

        public void AddStringListener(string key, Action<string> callback)
        {
            if (!_stringEvents.ContainsKey(key)) _stringEvents[key] = callback;
            else _stringEvents[key] += callback;
        }

        public void RemoveStringListener(string key, Action<string> callback)
        {
            if (_stringEvents.ContainsKey(key))
            {
                _stringEvents[key] -= callback;
                if (_stringEvents[key] == null) _stringEvents.Remove(key);
            }
        }
        #endregion
        
        // 애플리케이션 종료 또는 백그라운드 진입 시 자동 저장
        protected override void OnApplicationQuit()
        {
            SaveSettings();
            base.OnApplicationQuit();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveSettings();
            }
        }
    }
}

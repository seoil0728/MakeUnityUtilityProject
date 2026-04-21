using System;
using System.Collections.Generic;

namespace SWUtility.Setting
{
    [Serializable]
    public class SettingData
    {
        // LitJson 직렬화를 위해 public Dictionary 사용
        public Dictionary<string, float> FloatSettings = new Dictionary<string, float>();
        public Dictionary<string, int> IntSettings = new Dictionary<string, int>();
        public Dictionary<string, bool> BoolSettings = new Dictionary<string, bool>();
        public Dictionary<string, string> StringSettings = new Dictionary<string, string>();

        public SettingData()
        {
            // 기본 생성자 (LitJson에서 필요)
        }
    }
}

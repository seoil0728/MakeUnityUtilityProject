using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace SWUtility.Benchmark
{
    public class FrameTimeMonitor : MonoBehaviour, IBenchmarkMonitor
    {
        public string MonitorName => "Frame Time Monitor";
        public bool Started => started_;
        
        private List<float> deltaTimeList_ = new List<float>();
        private Dictionary<string, string> realtimeData_ = new Dictionary<string, string>();

        #region Title String
        // Realtime Data Title
        private string realtimeData_FrameTime_ = "Current Frame Time (ms)";
        
        // Result Data Title
        private string resultData_Frame_Total_ = "Total Frame";
        private string resultData_FrameTime_Avg_ = "Average Frame Time (ms)";
        private string resultData_FrameTime_Max_ = "Max Frame Time (ms)";
        private string resultData_FrameTime_Min_ = "Min Frame Time (ms)";
        #endregion

        private bool started_ = false;
        
        private void Initialize()
        {
            deltaTimeList_.Clear();
            realtimeData_.Clear();
        }

        private void Register()
        {
            if (BenchmarkManager.Instance == null)
                return;
            
            BenchmarkManager.Instance.RegisterMonitor(this);
        }

        private void UnRegister()
        {
            if (BenchmarkManager.Instance == null)
                return;
            
            BenchmarkManager.Instance.UnregisterMonitor(this);
        }
        
        
        #region IBenchmarkMonitor implementation
        
        public void OnStartMonitor()
        {
            Initialize();
            
            started_ = true;
        }

        public void OnUpdateMonitor()
        {
            if (!started_)
                return;
            
            deltaTimeList_.Add(Time.unscaledDeltaTime);
        }

        public void OnStopMonitor()
        {
            started_ = false;
        }

        public Dictionary<string, string> GetRealtimeData()
        {
            realtimeData_.Clear();

            var currentFrameTime = Time.unscaledDeltaTime;
            
            realtimeData_[realtimeData_FrameTime_] = (currentFrameTime * 1000).ToString("F2");
            
            return realtimeData_;
        }

        public Dictionary<string, string> GetResultData()
        {
            Dictionary<string, string> resultDict = new Dictionary<string, string>();

            if (deltaTimeList_.Count == 0)
            {
                Debug.LogWarning($"[{MonitorName}]: No Data found");
                return resultDict;
            }
            
            // Logic
            float totalFrameTime = deltaTimeList_.Sum();
            float avgFrameTime = totalFrameTime / deltaTimeList_.Count;
            float maxFrameTime = deltaTimeList_.Max();
            float minFrameTime = deltaTimeList_.Min();
            
            // Result Dictionary
            resultDict[resultData_Frame_Total_] = deltaTimeList_.Count.ToString();
            resultDict[resultData_FrameTime_Avg_] = (avgFrameTime * 1000).ToString("F2");
            resultDict[resultData_FrameTime_Max_] = (maxFrameTime * 1000).ToString("F2");
            resultDict[resultData_FrameTime_Min_] = (minFrameTime * 1000).ToString("F2");
            
            return resultDict;
        }
        
        #endregion
        
        
        #region Unity Event Functions

        private void OnEnable()
        {
            Register();
        }

        private void OnDisable()
        {
            UnRegister();
        }
        
        #endregion
    }
}


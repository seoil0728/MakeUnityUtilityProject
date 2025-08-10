using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SWUtility.Benchmark
{
    public class FrameMonitor : MonoBehaviour, IBenchmarkMonitor
    {
        public string MonitorName => "FPS & Frame Time Monitor";
        public bool Started => started_;
        
        private List<float> deltaTimeList_ = new List<float>();
        private Dictionary<string, string> realtimeData_ = new Dictionary<string, string>();

        #region Title String
        // Realtime Data Title
        private string realtimeData_FPS_ = "Current FPS";
        private string realtimeData_FPS_Avg = "Average FPS";
        private string realtimeData_FrameTime_ = "Current Frame Time (ms)";
        
        // Result Data Title
        private string resultData_FPS_Avg_ = "Average FPS";
        private string resultData_FPS_Low_ = "1% Low FPS";
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
            var currentTotalFrameTime = deltaTimeList_.Sum();
            var currentAverageFrameTime = currentTotalFrameTime / deltaTimeList_.Count;
            
            var currentFPS = 1.0f / currentFrameTime;
            var currentAverageFPS = 1.0f / currentAverageFrameTime;
            
            realtimeData_[realtimeData_FPS_] = currentFPS.ToString("F2");
            realtimeData_[realtimeData_FPS_Avg] = currentAverageFPS.ToString("F2");
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
            float avgFps = 1.0f / avgFrameTime;
            
            var sortedFrameTimes = deltaTimeList_.OrderBy(t => t).ToList();
            int percentileIndex = Mathf.FloorToInt(sortedFrameTimes.Count * 0.99f);
            float p99FrameTime = sortedFrameTimes[percentileIndex];
            float onePercentLowFps = 1.0f / p99FrameTime;
            
            // Result Dictionary
            resultDict[resultData_FPS_Avg_] = avgFps.ToString("F2");
            resultDict[resultData_FPS_Low_] = onePercentLowFps.ToString("F2");
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

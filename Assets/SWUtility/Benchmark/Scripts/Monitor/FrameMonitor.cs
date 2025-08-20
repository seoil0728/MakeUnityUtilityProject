using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SWUtility.Benchmark
{
    public class FrameMonitor : DeltaTimeMonitorBase
    {
        public override string MonitorName => "FPS & Frame Time Monitor";
        
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
        
        private Dictionary<string, string> realtimeData_ = new Dictionary<string, string>();
        
        
        #region DeltaTimeMonitorBase implementation
        
        public override Dictionary<string, string> GetRealtimeData()
        {
            realtimeData_.Clear();

            var currentFrameTime = Time.unscaledDeltaTime;
            var currentTotalFrameTime = TotalDeltaTime;
            var currentAverageFrameTime = currentTotalFrameTime / DeltaTimeList.Count;
            
            var currentFPS = 1.0f / currentFrameTime;
            var currentAverageFPS = 1.0f / currentAverageFrameTime;
            
            realtimeData_[realtimeData_FPS_] = currentFPS.ToString("F2");
            realtimeData_[realtimeData_FPS_Avg] = currentAverageFPS.ToString("F2");
            realtimeData_[realtimeData_FrameTime_] = (currentFrameTime * 1000).ToString("F2");
            
            return realtimeData_;
        }

        public override Dictionary<string, string> GetResultData()
        {
            Dictionary<string, string> resultDict = new Dictionary<string, string>();

            if (DeltaTimeList.Count == 0)
            {
                Debug.LogWarning($"[{MonitorName}]: No Data found");
                return resultDict;
            }
            
            // Logic
            float totalFrameTime = TotalDeltaTime;
            float avgFrameTime = totalFrameTime / DeltaTimeList.Count;
            float maxFrameTime = DeltaTimeList.Max();
            float minFrameTime = DeltaTimeList.Min();
            float avgFps = 1.0f / avgFrameTime;
            
            var sortedFrameTimes = DeltaTimeList.OrderBy(t => t).ToList();
            int percentileIndex = Mathf.FloorToInt(sortedFrameTimes.Count * 0.99f);
            float p99FrameTime = sortedFrameTimes[percentileIndex];
            float onePercentLowFps = 1.0f / p99FrameTime;
            
            // Result Dictionary
            resultDict[resultData_FPS_Avg_] = avgFps.ToString("F2");
            resultDict[resultData_FPS_Low_] = onePercentLowFps.ToString("F2");
            resultDict[resultData_Frame_Total_] = DeltaTimeList.Count.ToString();
            resultDict[resultData_FrameTime_Avg_] = (avgFrameTime * 1000).ToString("F2");
            resultDict[resultData_FrameTime_Max_] = (maxFrameTime * 1000).ToString("F2");
            resultDict[resultData_FrameTime_Min_] = (minFrameTime * 1000).ToString("F2");
            
            return resultDict;
        }
        
        #endregion
    }
}

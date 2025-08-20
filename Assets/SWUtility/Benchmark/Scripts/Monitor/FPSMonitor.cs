using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SWUtility.Benchmark
{
    public class FPSMonitor : DeltaTimeMonitorBase
    {
        public override string MonitorName => "FPSMonitor";
        
        #region Title String
        // Realtime Data Title
        private string realtimeData_FPS_ = "Current FPS";
        private string realtimeData_FPS_Avg = "Average FPS";
        
        // Result Data Title
        private string resultData_FPS_Avg_ = "Average FPS";
        private string resultData_FPS_Low_ = "1% Low FPS";
        private string resultData_Frame_Total_ = "Total Frame";
        #endregion

        private Dictionary<string, string> realtimeData_ = new Dictionary<string, string>();
        
        
        #region DeltaTimeMonitorBase implementation

        public override Dictionary<string, string> GetRealtimeData()
        {
            realtimeData_.Clear();

            var currentFrameTime = Time.unscaledDeltaTime;
            var currentTotalFrameTime = TotalDeltaTime;

            float currentAverageFrameTime = 0f;
            if (FrameCount > 0)
            {
                currentAverageFrameTime = currentTotalFrameTime / FrameCount;
            }
            
            var currentFPS = 1.0f / currentFrameTime;
            var currentAverageFPS = (currentAverageFrameTime > 0) ? (1.0f / currentAverageFrameTime) : 0f;
            
            realtimeData_[realtimeData_FPS_] = currentFPS.ToString("F2");
            realtimeData_[realtimeData_FPS_Avg] = currentAverageFPS.ToString("F2");
            
            return realtimeData_;
        }

        public override Dictionary<string, string> GetResultData()
        {
            Dictionary<string, string> resultDict = new Dictionary<string, string>();

            if (FrameCount == 0)
            {
                Debug.LogWarning($"[{MonitorName}]: No Data found");
                return resultDict;
            }
            
            // Logic
            float totalFrameTime = TotalDeltaTime;
            float avgFrameTime = totalFrameTime / FrameCount;
            float avgFps = 1.0f / avgFrameTime;
            
            var sortedFrameTimes = DeltaTimeList.OrderBy(t => t).ToList();
            int percentileIndex = Mathf.FloorToInt(sortedFrameTimes.Count * 0.99f);
            float p99FrameTime = sortedFrameTimes[percentileIndex];
            float onePercentLowFps = 1.0f / p99FrameTime;
            
            // Result Dictionary
            resultDict[resultData_FPS_Avg_] = avgFps.ToString("F2");
            resultDict[resultData_FPS_Low_] = onePercentLowFps.ToString("F2");
            resultDict[resultData_Frame_Total_] = FrameCount.ToString();
            
            return resultDict;
        }
        
        #endregion
        
    }
}

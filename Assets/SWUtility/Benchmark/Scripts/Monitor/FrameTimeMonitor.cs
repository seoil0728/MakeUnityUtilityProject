using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace SWUtility.Benchmark
{
    public class FrameTimeMonitor : DeltaTimeMonitorBase
    {
        public override string MonitorName => "Frame Time Monitor";
        
        #region Title String
        // Realtime Data Title
        private string realtimeData_FrameTime_ = "Current Frame Time (ms)";
        
        // Result Data Title
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
            
            realtimeData_[realtimeData_FrameTime_] = (currentFrameTime * 1000).ToString("F2");
            
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
            float maxFrameTime = DeltaTimeList.Max();
            float minFrameTime = DeltaTimeList.Min();
            
            // Result Dictionary
            resultDict[resultData_Frame_Total_] = FrameCount.ToString();
            resultDict[resultData_FrameTime_Avg_] = (avgFrameTime * 1000).ToString("F2");
            resultDict[resultData_FrameTime_Max_] = (maxFrameTime * 1000).ToString("F2");
            resultDict[resultData_FrameTime_Min_] = (minFrameTime * 1000).ToString("F2");
            
            return resultDict;
        }
        
        #endregion
    }
}


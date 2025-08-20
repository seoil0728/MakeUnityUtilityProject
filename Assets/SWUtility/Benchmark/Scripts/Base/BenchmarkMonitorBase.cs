using UnityEngine;
using System.Collections.Generic;

namespace SWUtility.Benchmark
{
    public abstract class BenchmarkMonitorBase : MonoBehaviour, IBenchmarkMonitor
    {
        public abstract string MonitorName { get; }
        public abstract Dictionary<string, string> GetRealtimeData();
        public abstract Dictionary<string, string> GetResultData();

        public bool Started { get; private set; }

        public virtual void OnStartMonitor()
        {
            Started = true;
        }
        
        /// <summary>
        /// Need To Override if the monitor uses Update Logic.
        /// </summary>
        public virtual void OnUpdateMonitor()
        {
            
        }
        
        public virtual void OnStopMonitor()
        {
            Started = false;
        }
    }
}
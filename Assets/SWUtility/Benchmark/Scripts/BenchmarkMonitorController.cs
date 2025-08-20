using UnityEngine;
using System.Collections.Generic;

namespace SWUtility.Benchmark
{
    public class BenchmarkMonitorController : MonoBehaviour
    {
        public IReadOnlyList<IBenchmarkMonitor> Monitors => monitors_;
        public bool Inited => inited_;
        
        private IBenchmarkMonitor[] monitors_ = null;
        private bool inited_ = false;

        public void Initialize()
        {
            monitors_ = GetComponentsInChildren<IBenchmarkMonitor>();

            if (monitors_ == null)
            {
                Debug.LogWarning("[BenchmarkMonitorController] No Monitors!");
                return;
            }
             
            Debug.Log("[BenchmarkMonitorController] Initialized.");
            inited_ = true;
        }
        
        /// <summary>
        /// 모든 모니터의 측정을 시작합니다.
        /// </summary>
        public void StartAllMonitors()
        {
            if (!inited_) return;
            
            foreach (var monitor in monitors_)
            {
                monitor.OnStartMonitor();
            }
        }

        /// <summary>
        /// 모든 모니터의 내부 데이터 수집을 업데이트합니다.
        /// </summary>
        public void UpdateAllMonitors()
        {
            if (!inited_) return;
            
            foreach (var monitor in monitors_)
            {
                monitor.OnUpdateMonitor();
            }
        }

        /// <summary>
        /// 모든 모니터의 측정을 중지합니다.
        /// </summary>
        public void StopAllMonitors()
        {
            if (!inited_) return;
            
            foreach (var monitor in monitors_)
            {
                monitor.OnStopMonitor();
            }
        }
    }
}

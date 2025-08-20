using System.Collections.Generic;
using UnityEngine;

namespace SWUtility.Benchmark
{
    public abstract class DeltaTimeMonitorBase : BenchmarkMonitorBase
    {
        protected readonly List<float> DeltaTimeList = new List<float>();
        // RealtimeData는 모니터에 따라 구현하지 않을 수 있으므로 선언하지 않음.
        // protected Dictionary<string, string> realtimeData_ = new Dictionary<string, string>();

        protected float TotalDeltaTime { get; private set; }
        protected int FrameCount { get; private set; }

        public override void OnStartMonitor()
        {
            base.OnStartMonitor(); 
            
            DeltaTimeList.Clear();
            TotalDeltaTime = 0f;
            FrameCount = 0;
        }

        public override void OnUpdateMonitor()
        {
            if (!Started) return;
            
            base.OnUpdateMonitor();

            float dt = Time.unscaledDeltaTime;
            DeltaTimeList.Add(dt);
            TotalDeltaTime += dt;
            FrameCount++;
        }
    }
}
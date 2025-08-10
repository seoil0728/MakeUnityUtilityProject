using System;
using System.Collections.Generic;
using UnityEngine;

namespace SWUtility.Benchmark
{
    /// <summary>
    /// IBenchmarkMonitor 인터페이스를 구현한 모든 측정 모듈을 관리하고 제어하는 싱글톤 클래스입니다.
    /// </summary>
    public class BenchmarkManager : MonoBehaviour
    {
        #region Singleton
        private static BenchmarkManager instance_;

        public static BenchmarkManager Instance
        {
            get
            {
                if (instance_ == null)
                {
                    instance_ = FindFirstObjectByType<BenchmarkManager>();
                    if (instance_ == null)
                    {
                        GameObject go = new GameObject("BenchmarkManager");
                        instance_ = go.AddComponent<BenchmarkManager>();
                    }
                }
                return instance_;
            }
        }
        #endregion

        #region Public Fields
        /// <summary>
        /// Key : 모니터 이름, Value : 실시간 데이터 딕셔너리
        /// </summary>
        public IReadOnlyDictionary<string, Dictionary<string, string>> AllRealtimeData => allRealtimeData_;
        /// <summary>
        /// Key : 모니터 이름, Value : 실시간 데이터 딕셔너리
        /// </summary>
        public IReadOnlyDictionary<string, Dictionary<string, string>> AllResultData => allResultData_;

        /// <summary>
        /// 현재 벤치마크가 실행 중인지 여부를 나타냅니다.
        /// </summary>
        public bool IsBenchmarking => isBenchmarking_;

        #endregion

        #region Private Fields

        private readonly Dictionary<string, Dictionary<string, string>> allRealtimeData_ = new Dictionary<string, Dictionary<string, string>>();
        private readonly Dictionary<string, Dictionary<string, string>> allResultData_ = new Dictionary<string, Dictionary<string, string>>();

        private readonly List<IBenchmarkMonitor> monitors_ = new List<IBenchmarkMonitor>();

        private bool isBenchmarking_ = false;
        #endregion

        #region Events
        /// <summary>
        /// 실시간 데이터가 갱신될 때마다 호출되는 이벤트입니다.
        /// </summary>
        public event Action<IReadOnlyDictionary<string, Dictionary<string, string>>> OnRealtimeDataUpdated;

        /// <summary>
        /// 벤치마크가 종료되고 최종 결과가 집계되었을 때 호출되는 이벤트입니다.
        /// </summary>
        public event Action<IReadOnlyDictionary<string, Dictionary<string, string>>> OnBenchmarkCompleted;
        #endregion

        #region Handlers
        public IBenchmarkResultPrintHandler PrintHandler { get; set; }
        public IBenchmarkResultSaveLoadHandler SaveLoadHandler { get; set; }
        #endregion


        /// <summary>
        /// 측정 모듈을 관리 리스트에 등록합니다.
        /// </summary>
        /// <param name="monitor">등록할 측정 모듈</param>
        public void RegisterMonitor(IBenchmarkMonitor monitor)
        {
            if (!monitors_.Contains(monitor))
            {
                monitors_.Add(monitor);

                allRealtimeData_[monitor.MonitorName] = new Dictionary<string, string>();
                allResultData_[monitor.MonitorName] = new Dictionary<string, string>();

                Debug.Log($"[BenchmarkManager] Monitor '{monitor.MonitorName}' registered.");
            }
        }

        /// <summary>
        /// 측정 모듈을 관리 리스트에서 제거합니다.
        /// </summary>
        /// <param name="monitor">제거할 측정 모듈</param>
        public void UnregisterMonitor(IBenchmarkMonitor monitor)
        {
            if (monitors_.Contains(monitor))
            {
                monitors_.Remove(monitor);

                allRealtimeData_.Remove(monitor.MonitorName);
                allResultData_.Remove(monitor.MonitorName);

                Debug.Log($"[BenchmarkManager] Monitor '{monitor.GetType().Name}' unregistered.");
            }
        }

        /// <summary>
        /// 등록된 모든 모듈의 벤치마크 측정을 시작합니다.
        /// </summary>
        [ContextMenu("Start Benchmark")]
        public void StartBenchmark()
        {
            if (isBenchmarking_)
            {
                Debug.LogWarning("Benchmark is already running.");
                return;
            }

            foreach (var monitor in monitors_)
            {
                allRealtimeData_[monitor.MonitorName].Clear();
                allResultData_[monitor.MonitorName].Clear();
                monitor.OnStartMonitor();
            }

            Debug.Log($"Benchmark started with {monitors_.Count} monitors.");
            isBenchmarking_ = true;
        }

        /// <summary>
        /// 등록된 모든 모듈의 벤치마크 측정을 종료합니다.
        /// </summary>
        [ContextMenu("Stop Benchmark")]
        public void StopBenchmark()
        {
            if (!isBenchmarking_)
            {
                Debug.LogWarning("Benchmark is not running.");
                return;
            }

            foreach (var monitor in monitors_)
            {
                monitor.OnStopMonitor();

                var results = monitor.GetResultData();
                allResultData_[monitor.MonitorName] = results;
            }

            OnBenchmarkCompleted?.Invoke(allResultData_);

            Debug.Log("Benchmark stopped.");
            isBenchmarking_ = false;
        }


        public void PrintCurrentResult()
        {
            if (PrintHandler == null)
            {
                Debug.LogWarning("PrintHandler is not set. Cannot print results.");
                return;
            }

            PrintHandler.OnPrintResults(allResultData_);
        }

        public void SaveResultsToJson(string path)
        {
            if (SaveLoadHandler == null)
            {
                Debug.LogWarning("SaveLoadHandler is not set. Cannot save results.");
                return;
            }

            SaveLoadHandler.SaveResults(allResultData_, path);
        }

        public void LoadResultsFromJson(string path)
        {
            if (SaveLoadHandler == null)
            {
                Debug.LogWarning("SaveLoadHandler is not set. Cannot load results.");
                return;
            }

            var loadedResults = SaveLoadHandler.LoadResults(path);
            
            // TODO: 벤치마크 결과를 로드한 후 처리하는 로직
        }


        /// <summary>
        /// 벤치마크 측정 중에 매 프레임 갱신되는 데이터를 업데이트합니다.
        /// </summary>
        private void UpdateBenchmark()
        {
            foreach (var monitor in monitors_)
            {
                monitor.OnUpdateMonitor();

                var realtimeData = monitor.GetRealtimeData();
                allRealtimeData_[monitor.MonitorName] = realtimeData;
            }

            OnRealtimeDataUpdated?.Invoke(allRealtimeData_);
        }


        #region Unity Event Functions
        private void Awake()
        {
            if (instance_ != null && instance_ != this)
            {
                Destroy(gameObject);
                return;
            }

            instance_ = this;
            DontDestroyOnLoad(gameObject);
        }

        private void LateUpdate()
        {
            // 벤치마크가 실행 중일 때만 모든 모듈의 Update를 호출
            if (!isBenchmarking_) return;

            UpdateBenchmark();
        }
        #endregion
    }
}

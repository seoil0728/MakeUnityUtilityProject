using System;
using System.Collections.Generic;
using UnityEngine;

namespace SWUtility.Benchmark
{
    /// <summary>
    /// IBenchmarkMonitor �������̽��� ������ ��� ���� ����� �����ϰ� �����ϴ� �̱��� Ŭ�����Դϴ�.
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
        /// Key : ����� �̸�, Value : �ǽð� ������ ��ųʸ�
        /// </summary>
        public IReadOnlyDictionary<string, Dictionary<string, string>> AllRealtimeData => allRealtimeData_;
        /// <summary>
        /// Key : ����� �̸�, Value : �ǽð� ������ ��ųʸ�
        /// </summary>
        public IReadOnlyDictionary<string, Dictionary<string, string>> AllResultData => allResultData_;

        /// <summary>
        /// ���� ��ġ��ũ�� ���� ������ ���θ� ��Ÿ���ϴ�.
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
        /// �ǽð� �����Ͱ� ���ŵ� ������ ȣ��Ǵ� �̺�Ʈ�Դϴ�.
        /// </summary>
        public event Action<IReadOnlyDictionary<string, Dictionary<string, string>>> OnRealtimeDataUpdated;

        /// <summary>
        /// ��ġ��ũ�� ����ǰ� ���� ����� ����Ǿ��� �� ȣ��Ǵ� �̺�Ʈ�Դϴ�.
        /// </summary>
        public event Action<IReadOnlyDictionary<string, Dictionary<string, string>>> OnBenchmarkCompleted;
        #endregion

        #region Handlers
        public IBenchmarkResultPrintHandler PrintHandler { get; set; }
        public IBenchmarkResultSaveLoadHandler SaveLoadHandler { get; set; }
        #endregion


        /// <summary>
        /// ���� ����� ���� ����Ʈ�� ����մϴ�.
        /// </summary>
        /// <param name="monitor">����� ���� ���</param>
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
        /// ���� ����� ���� ����Ʈ���� �����մϴ�.
        /// </summary>
        /// <param name="monitor">������ ���� ���</param>
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
        /// ��ϵ� ��� ����� ��ġ��ũ ������ �����մϴ�.
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
        /// ��ϵ� ��� ����� ��ġ��ũ ������ �����մϴ�.
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
            
            // TODO: ��ġ��ũ ����� �ε��� �� ó���ϴ� ����
        }


        /// <summary>
        /// ��ġ��ũ ���� �߿� �� ������ ���ŵǴ� �����͸� ������Ʈ�մϴ�.
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
            // ��ġ��ũ�� ���� ���� ���� ��� ����� Update�� ȣ��
            if (!isBenchmarking_) return;

            UpdateBenchmark();
        }
        #endregion
    }
}

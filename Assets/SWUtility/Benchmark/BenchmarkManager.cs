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
        private static BenchmarkManager instance;

        public static BenchmarkManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindFirstObjectByType<BenchmarkManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("BenchmarkManager");
                        instance = go.AddComponent<BenchmarkManager>();
                    }
                }
                return instance;
            }
        }
        #endregion

        // ��ϵ� ��� ���� ���(�÷�����)�� ��� ����Ʈ
        private readonly List<IBenchmarkMonitor> monitors = new List<IBenchmarkMonitor>();
        private bool isBenchmarking = false;


        /// <summary>
        /// ���� ����� ���� ����Ʈ�� ����մϴ�.
        /// </summary>
        /// <param name="monitor">����� ���� ���</param>
        public void RegisterMonitor(IBenchmarkMonitor monitor)
        {
            if (!monitors.Contains(monitor))
            {
                monitors.Add(monitor);
                Debug.Log($"[BenchmarkManager] Monitor '{monitor.GetType().Name}' registered.");
            }
        }

        /// <summary>
        /// ���� ����� ���� ����Ʈ���� �����մϴ�.
        /// </summary>
        /// <param name="monitor">������ ���� ���</param>
        public void UnregisterMonitor(IBenchmarkMonitor monitor)
        {
            if (monitors.Contains(monitor))
            {
                monitors.Remove(monitor);
                Debug.Log($"[BenchmarkManager] Monitor '{monitor.GetType().Name}' unregistered.");
            }
        }

        /// <summary>
        /// ��ϵ� ��� ����� ��ġ��ũ ������ �����մϴ�.
        /// </summary>
        [ContextMenu("Start Benchmark")]
        public void StartBenchmark()
        {
            if (isBenchmarking)
            {
                Debug.LogWarning("Benchmark is already running.");
                return;
            }

            Debug.Log($"Benchmark started with {monitors.Count} monitors.");
            isBenchmarking = true;

            foreach (var monitor in monitors)
            {
                monitor.StartMonitor();
            }
        }

        /// <summary>
        /// ��ϵ� ��� ����� ��ġ��ũ ������ �����մϴ�.
        /// </summary>
        [ContextMenu("Stop Benchmark")]
        public void StopBenchmark()
        {
            if (!isBenchmarking)
            {
                Debug.LogWarning("Benchmark is not running.");
                return;
            }

            Debug.Log("Benchmark stopped.");
            isBenchmarking = false;

            foreach (var monitor in monitors)
            {
                monitor.StopMonitor();
            }
        }


        #region Unity Event
        private void Awake()
        {
            // �̱��� �ν��Ͻ� ����
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            // ��ġ��ũ�� ���� ���� ���� ��� ����� Update�� ȣ��
            if (!isBenchmarking) return;

            foreach (var monitor in monitors)
            {
                monitor.UpdateMonitor();
            }
        }
        #endregion
    }
}

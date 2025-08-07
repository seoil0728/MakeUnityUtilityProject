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

        // 등록된 모든 측정 모듈(플러그인)을 담는 리스트
        private readonly List<IBenchmarkMonitor> monitors = new List<IBenchmarkMonitor>();
        private bool isBenchmarking = false;


        /// <summary>
        /// 측정 모듈을 관리 리스트에 등록합니다.
        /// </summary>
        /// <param name="monitor">등록할 측정 모듈</param>
        public void RegisterMonitor(IBenchmarkMonitor monitor)
        {
            if (!monitors.Contains(monitor))
            {
                monitors.Add(monitor);
                Debug.Log($"[BenchmarkManager] Monitor '{monitor.GetType().Name}' registered.");
            }
        }

        /// <summary>
        /// 측정 모듈을 관리 리스트에서 제거합니다.
        /// </summary>
        /// <param name="monitor">제거할 측정 모듈</param>
        public void UnregisterMonitor(IBenchmarkMonitor monitor)
        {
            if (monitors.Contains(monitor))
            {
                monitors.Remove(monitor);
                Debug.Log($"[BenchmarkManager] Monitor '{monitor.GetType().Name}' unregistered.");
            }
        }

        /// <summary>
        /// 등록된 모든 모듈의 벤치마크 측정을 시작합니다.
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
        /// 등록된 모든 모듈의 벤치마크 측정을 종료합니다.
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
            // 싱글톤 인스턴스 설정
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
            // 벤치마크가 실행 중일 때만 모든 모듈의 Update를 호출
            if (!isBenchmarking) return;

            foreach (var monitor in monitors)
            {
                monitor.UpdateMonitor();
            }
        }
        #endregion
    }
}

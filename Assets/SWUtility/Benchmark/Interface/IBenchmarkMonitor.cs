namespace SWUtility.Benchmark
{
    /// <summary>
    /// 모든 벤치마크 측정 모듈이 구현해야 하는 인터페이스입니다.
    /// 벤치마크 시스템의 '플러그인' 규격 역할을 합니다.
    /// </summary>
    public interface IBenchmarkMonitor
    {
        string MonitorName { get; }

        /// <summary>
        /// 벤치마크 측정 시작 시 호출됩니다.
        /// </summary>
        void StartMonitor();

        /// <summary>
        /// 벤치마크 측정 중 매 프레임 호출됩니다.
        /// </summary>
        void UpdateMonitor();

        /// <summary>
        /// 벤치마크 측정 종료 시 호출됩니다.
        /// </summary>
        void StopMonitor();
    }
}

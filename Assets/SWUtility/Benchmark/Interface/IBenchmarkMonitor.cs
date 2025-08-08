using System.Collections.Generic;

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
        void OnStartMonitor();

        /// <summary>
        /// 벤치마크 측정 중 매 프레임 호출됩니다.
        /// </summary>
        void OnUpdateMonitor();

        /// <summary>
        /// 벤치마크 측정 종료 시 호출됩니다.
        /// </summary>
        void OnStopMonitor();

        /// <summary>
        /// 현재 프레임의 실시간 측정치를 Key-Value 형태로 반환합니다.
        /// </summary>
        /// <returns>실시간 데이터 딕셔너리</returns>
        Dictionary<string, string> GetRealtimeData();

        /// <summary>
        /// 측정이 끝난 후, 수집된 데이터를 바탕으로 최종 결과물을 Key-Value 형태로 계산하여 반환합니다.
        /// </summary>
        /// <returns>최종 결과 데이터 딕셔너리</returns>
        Dictionary<string, string> GetResultData();
    }
}

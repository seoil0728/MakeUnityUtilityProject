using System.Collections.Generic;

namespace SWUtility.Benchmark
{
    /// <summary>
    /// 벤치마크 결과를 표시하는 핸들러입니다. (예: 콘솔, UI)
    /// </summary>
    public interface IBenchmarkResultPrintHandler
    {
        void OnPrintResults(IReadOnlyDictionary<string, Dictionary<string, string>> results);
    }
}
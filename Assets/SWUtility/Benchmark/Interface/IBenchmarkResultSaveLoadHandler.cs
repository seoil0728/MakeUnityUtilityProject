using System.Collections.Generic;

namespace SWUtility.Benchmark
{
    /// <summary>
    /// 벤치마크 결과를 저장, 로드하는 핸들러입니다. (예: Json 파일, DB 등)
    /// </summary>
    public interface IBenchmarkResultSaveLoadHandler
    {
        void SaveResults(IReadOnlyDictionary<string, Dictionary<string, string>> results, string path);
        Dictionary<string, Dictionary<string, string>> LoadResults(string path);
    }
}
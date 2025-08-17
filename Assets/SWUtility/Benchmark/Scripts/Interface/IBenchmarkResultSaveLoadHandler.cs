using System.Collections.Generic;

namespace SWUtility.Benchmark
{
    /// <summary>
    /// ��ġ��ũ ����� ����, �ε��ϴ� �ڵ鷯�Դϴ�. (��: Json ����, DB ��)
    /// </summary>
    public interface IBenchmarkResultSaveLoadHandler
    {
        void SaveResults(IReadOnlyDictionary<string, Dictionary<string, string>> results, string path);
        Dictionary<string, Dictionary<string, string>> LoadResults(string path);
    }
}
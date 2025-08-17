using System.Collections.Generic;

namespace SWUtility.Benchmark
{
    /// <summary>
    /// ��ġ��ũ ����� ǥ���ϴ� �ڵ鷯�Դϴ�. (��: �ܼ�, UI)
    /// </summary>
    public interface IBenchmarkResultPrintHandler
    {
        void OnPrintResults(IReadOnlyDictionary<string, Dictionary<string, string>> results);
    }
}
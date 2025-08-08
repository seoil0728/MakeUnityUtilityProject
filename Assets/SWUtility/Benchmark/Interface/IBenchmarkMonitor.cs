using System.Collections.Generic;

namespace SWUtility.Benchmark
{
    /// <summary>
    /// ��� ��ġ��ũ ���� ����� �����ؾ� �ϴ� �������̽��Դϴ�.
    /// ��ġ��ũ �ý����� '�÷�����' �԰� ������ �մϴ�.
    /// </summary>
    public interface IBenchmarkMonitor
    {
        string MonitorName { get; }

        /// <summary>
        /// ��ġ��ũ ���� ���� �� ȣ��˴ϴ�.
        /// </summary>
        void OnStartMonitor();

        /// <summary>
        /// ��ġ��ũ ���� �� �� ������ ȣ��˴ϴ�.
        /// </summary>
        void OnUpdateMonitor();

        /// <summary>
        /// ��ġ��ũ ���� ���� �� ȣ��˴ϴ�.
        /// </summary>
        void OnStopMonitor();

        /// <summary>
        /// ���� �������� �ǽð� ����ġ�� Key-Value ���·� ��ȯ�մϴ�.
        /// </summary>
        /// <returns>�ǽð� ������ ��ųʸ�</returns>
        Dictionary<string, string> GetRealtimeData();

        /// <summary>
        /// ������ ���� ��, ������ �����͸� �������� ���� ������� Key-Value ���·� ����Ͽ� ��ȯ�մϴ�.
        /// </summary>
        /// <returns>���� ��� ������ ��ųʸ�</returns>
        Dictionary<string, string> GetResultData();
    }
}

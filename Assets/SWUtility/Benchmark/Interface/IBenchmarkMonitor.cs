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
        void StartMonitor();

        /// <summary>
        /// ��ġ��ũ ���� �� �� ������ ȣ��˴ϴ�.
        /// </summary>
        void UpdateMonitor();

        /// <summary>
        /// ��ġ��ũ ���� ���� �� ȣ��˴ϴ�.
        /// </summary>
        void StopMonitor();
    }
}

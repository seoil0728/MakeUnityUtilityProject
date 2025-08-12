using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class BenchmarkUIPrinter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text monitorText = null;

    [SerializeField]
    private TMP_Text dataText = null;

    private StringBuilder builder_ = new StringBuilder();

    public void GetBenchmarkDatas(string monitorName, Dictionary<string, string> data)
    {
        if (monitorText == null || dataText == null)
            return;

        monitorText.text = monitorName;

        if (data == null || data.Count == 0)
        {
            dataText.text = "No Data";
            return;
        }

        builder_.Clear();

        foreach (var kvp in data)
        {
            builder_.AppendLine($"{kvp.Key}: {kvp.Value}");
        }

        dataText.text = builder_.ToString();
    }
}

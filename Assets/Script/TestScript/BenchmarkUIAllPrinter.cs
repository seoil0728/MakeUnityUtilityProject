using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class BenchmarkUIAllPrinter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text_ = null;

    private StringBuilder builder_ = new StringBuilder();

    public void UpdateResultDataToText(IReadOnlyDictionary<string, Dictionary<string, string>> resultData)
    {
        if (text_ == null)
            return;

        builder_.Clear();

        var keys = resultData.Keys;

        foreach (var key in keys)
        {
            builder_.AppendLine($"Monitor: {key}");
            var data = resultData[key];
            if (data == null || data.Count == 0)
            {
                builder_.AppendLine("No Data");
                continue;
            }

            foreach (var kvp in data)
            {
                builder_.AppendLine($"{kvp.Key}: {kvp.Value}");
            }
            builder_.AppendLine(); // Add a blank line between monitors
        }

        text_.text = builder_.ToString();
    }
}

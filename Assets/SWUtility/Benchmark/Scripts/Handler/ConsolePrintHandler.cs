using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SWUtility.Benchmark
{
    public class ConsolePrintHandler : IBenchmarkResultPrintHandler
    {
        public void OnPrintResults(IReadOnlyDictionary<string, Dictionary<string, string>> results)
        {
            if (results == null || results.Count == 0)
            {
                Debug.Log("[ConsolePrintHandler] There is no result Data.");
                return;
            }
            
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("=====Benchmark Results=====");

            foreach (var result in results)
            {
                builder.AppendLine(result.Key);
                
                if (result.Value.Count == 0)
                {
                    builder.AppendLine("No data collected.");
                    builder.AppendLine();
                    continue;
                }
                
                foreach (var dataPoint in result.Value)
                {
                    // 각 항목을 "이름: 값" 형태로 추가
                    builder.AppendLine($"  {dataPoint.Key}: {dataPoint.Value}");
                }

                builder.AppendLine();
            }

            builder.AppendLine("====================");
            
            Debug.Log(builder.ToString());
        }
    }
}


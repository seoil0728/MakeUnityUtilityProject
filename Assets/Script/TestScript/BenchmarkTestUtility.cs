using UnityEngine;
using SWUtility.Benchmark;
using System.Collections.Generic;
using System.Linq;

public class BenchmarkTestUtility : MonoBehaviour
{
    [SerializeField]
    private BenchmarkUIAllPrinter uiAllPrinter_ = null;

    private bool isValid_ = false;

    private void Init()
    {
        if (BenchmarkManager.Instance == null)
            return;

        BenchmarkManager.Instance.Initialize();
        
        ConsolePrintHandler consolePrintHandler = new ConsolePrintHandler();
        BenchmarkManager.Instance.OnBenchmarkCompleted += consolePrintHandler.OnPrintResults;
        isValid_ = true;
    }

    private void StartBenchmark()
    {
        if (!isValid_)
            return;

        BenchmarkManager.Instance.OnRealtimeDataUpdated += OnRealtimeDataUpdated;
        BenchmarkManager.Instance.OnBenchmarkCompleted += OnBenchmarkCompleted;

        BenchmarkManager.Instance.StartBenchmark();
    }

    private void StopBenchmark()
    {
        if (!isValid_)
            return;

        BenchmarkManager.Instance.OnRealtimeDataUpdated -= OnRealtimeDataUpdated;
        BenchmarkManager.Instance.OnBenchmarkCompleted -= OnBenchmarkCompleted;

        BenchmarkManager.Instance.StopBenchmark();
    }

    private void OnRealtimeDataUpdated(IReadOnlyDictionary<string, Dictionary<string, string>> realtimeData)
    {
        if (uiAllPrinter_ != null)
        {
            uiAllPrinter_.UpdateResultDataToText(realtimeData);
        }

        //for (int i = 0; i < uiPrinters_.Length; i++)
        //{
        //    if (i >= realtimeData.Count)
        //        break;

        //    var monitorName = realtimeData.Keys.ElementAt(i);
        //    var data = realtimeData[monitorName];

        //    uiPrinters_[i].GetBenchmarkDatas(monitorName, data);
        //}
    }

    private void OnBenchmarkCompleted(IReadOnlyDictionary<string, Dictionary<string, string>> resultData)
    {
        Debug.Log("Benchmark Complete.");
    }


    #region UI Callback

    public void OnStartButtonClicked()
    {
        Debug.Log("Start Button Clicked");

        StartBenchmark();
    }

    public void OnStopButtonClicked()
    {
        Debug.Log("Stop Button Clicked");

        StopBenchmark();
    }

    #endregion


    #region Unity Events

    private void Start()
    {
        Init();
    }

    #endregion

}

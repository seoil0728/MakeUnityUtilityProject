using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

namespace SWUtility.Benchmark
{
    public class JsonSaveLoadHandler : IBenchmarkResultSaveLoadHandler
    {
        /// <summary>
        /// 전달받은 결과 데이터를 지정된 경로에 JSON 파일로 저장합니다.
        /// </summary>
        /// <param name="results">저장할 벤치마크 결과 데이터</param>
        /// <param name="path">저장할 파일의 전체 경로 (파일명 포함)</param>
        public void SaveResults(IReadOnlyDictionary<string, Dictionary<string, string>> results, string path)
        {
            if (results == null || results.Count == 0)
            {
                Debug.LogWarning("[JsonSaveLoadHandler] 저장할 벤치마크 결과 데이터가 없습니다.");
                return;
            }

            try
            {
                // JsonWriter를 사용하여 보기 좋게 들여쓰기된(pretty-printed) JSON을 생성합니다.
                var sb = new System.Text.StringBuilder();
                var writer = new JsonWriter(sb);
                writer.PrettyPrint = true; // JSON을 보기 좋게 포맷팅
                
                // IReadOnlyDictionary를 LitJson으로 직접 변환
                JsonMapper.ToJson(results, writer);

                // 파일 저장
                File.WriteAllText(path, sb.ToString());
                
                Debug.Log($"<color=green>[JsonSaveLoadHandler]</color> 벤치마크 결과가 다음 경로에 저장되었습니다: {path}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[JsonSaveLoadHandler] JSON 저장 중 오류 발생: {e.Message}");
            }
        }

        /// <summary>
        /// 지정된 경로에서 JSON 파일을 읽어와 벤치마크 결과 데이터로 변환하여 반환합니다.
        /// </summary>
        /// <param name="path">불러올 파일의 전체 경로 (파일명 포함)</param>
        /// <returns>불러온 결과 데이터. 실패 시 null을 반환합니다.</returns>
        public Dictionary<string, Dictionary<string, string>> LoadResults(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    Debug.LogError($"[JsonSaveLoadHandler] 지정한 경로에 파일이 없습니다: {path}");
                    return null;
                }

                // 파일에서 JSON 문자열 읽기
                string jsonString = File.ReadAllText(path);

                // JSON 문자열을 딕셔너리 객체로 변환
                var loadedData = JsonMapper.ToObject<Dictionary<string, Dictionary<string, string>>>(jsonString);
                
                Debug.Log($"<color=cyan>[JsonSaveLoadHandler]</color> 다음 경로에서 벤치마크 결과를 불러왔습니다: {path}");
                return loadedData;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[JsonSaveLoadHandler] JSON 로드 중 오류 발생: {e.Message}");
                return null;
            }
        }
    }
}

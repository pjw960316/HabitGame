using System;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public sealed class NotionManager : ManagerBase<NotionManager>
{
    #region 1. Fields

    private string _notionToken;

    #endregion

    #region 1-1. Notion API
    
    private const string NOTION_CURRENT_USER_URL = "https://api.notion.com/v1/users/me";
    private const string NOTION_SEARCH_URL = "https://api.notion.com/v1/search";
    
    #endregion
    
    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public override void Initialize()
    {
        InitializeNotionToken();
        
        RequestNotionUserAsync().Forget();

        //test
        var requestJson = "{\"query\":\"2026년 10월\",\"filter\":{\"property\":\"object\",\"value\":\"page\"}}";
        RequestNotionPage(requestJson).Forget();
    }

    // NOTE
    // Windows면 시스템 환경변수에 등록해야 합니다.
    private void InitializeNotionToken()
    {
        _notionToken = Environment.GetEnvironmentVariable("NotionToken", EnvironmentVariableTarget.User);
        
        if(_notionToken == null)
        {
            throw new NullReferenceException("NotionToken is null");
        }
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Methods

    private async UniTask<string> RequestNotionUserAsync()
    {
        using (var request = UnityWebRequest.Get(NOTION_CURRENT_USER_URL))
        {
            request.timeout = 30;
            
            request.SetRequestHeader("Authorization", $"Bearer {_notionToken}");
            request.SetRequestHeader("Notion-Version", "2026-03-11");
            request.SetRequestHeader("Accept", "application/json");

            var operation = request.SendWebRequest();

            await UniTask.WaitUntil(() => operation.isDone, cancellationToken: Application.exitCancellationToken);

            var responseJson = request.downloadHandler.text;

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"[Notion] HTTP {request.responseCode}\n{responseJson}");
            }
            else
            {
                Debug.LogError($"[Notion] HTTP {request.responseCode} | {request.error}\n{responseJson}");
            }

            // 실패 응답도 원본 JSON을 반환하여 관찰할 수 있게 한다.
            return responseJson;
        }
    }

    private async UniTask<string> RequestNotionPage(string requestJson)
    {
        using (var request = UnityWebRequest.Post(
                   NOTION_SEARCH_URL,
                   requestJson,
                   "application/json"))
        {
            request.timeout = 30;

            // var byteData = Encoding.UTF8.GetBytes(requestJson);
            // request.uploadHandler = new UploadHandlerRaw(byteData);
            // request.downloadHandler = new DownloadHandlerBuffer();
            //
            // Debug.Log($"Original JSON: {requestJson}");
            // Debug.Log($"Upload Bytes: {byteData.Length}");
            // Debug.Log($"Upload JSON: {Encoding.UTF8.GetString(request.uploadHandler.data)}");
            
            request.SetRequestHeader("Authorization", $"Bearer {_notionToken}");
            request.SetRequestHeader("Notion-Version", "2026-03-11");
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();

            await UniTask.WaitUntil(() => operation.isDone, cancellationToken: Application.exitCancellationToken);

            var responseJson = request.downloadHandler.text;
            Debug.Log($"[Notion Page] HTTP {request.responseCode}\n{FormatJsonForLog(responseJson)}");

            return responseJson;
        }
    }

    // TEST
    // 정상적으로 쿼리 찾아지는 지 확인
    private string FormatJsonForLog(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return json;
        }

        try
        {
            var response = JObject.Parse(json);

            if (!(response["results"] is JArray results))
            {
                return json;
            }

            var summary = new StringBuilder();
            summary.AppendLine($"검색 결과: {results.Count}개 (이번 응답 기준)");

            for (var index = 0; index < results.Count; index++)
            {
                var page = results[index];
                summary.AppendLine($"{index + 1}. 이름: {GetNotionPageTitle(page)} | ID: {page["id"]?.Value<string>()}");
            }

            return summary.ToString();
        }
        catch (JsonReaderException)
        {
            // JSON이 아닌 응답도 원문을 로그로 확인할 수 있게 한다.
            return json;
        }
    }

    private string GetNotionPageTitle(JToken page)
    {
        if (page["properties"] is JObject properties)
        {
            foreach (var property in properties.Properties())
            {
                if (property.Value["type"]?.Value<string>() != "title" || !(property.Value["title"] is JArray titleParts))
                {
                    continue;
                }

                var title = new StringBuilder();

                foreach (var part in titleParts)
                {
                    title.Append(part["plain_text"]?.Value<string>() ?? part["text"]?["content"]?.Value<string>());
                }

                return title.Length > 0 ? title.ToString() : "(제목 없음)";
            }
        }

        return "(제목 없음)";
    }

    #endregion
}

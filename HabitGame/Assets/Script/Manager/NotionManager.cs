using System;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
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
        using (var request = new UnityWebRequest(NOTION_SEARCH_URL, "POST"))
        {
            request.timeout = 30;
            
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(requestJson));
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Authorization", $"Bearer {_notionToken}");
            request.SetRequestHeader("Notion-Version", "2026-03-11");
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();

            await UniTask.WaitUntil(() => operation.isDone, cancellationToken: Application.exitCancellationToken);

            var responseJson = request.downloadHandler.text;
            Debug.Log($"[Notion Page] HTTP {request.responseCode}\n{responseJson}");

            return responseJson;
        }
    }

    #endregion
}

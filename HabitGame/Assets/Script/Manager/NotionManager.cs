using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public sealed class NotionManager : ManagerBase<NotionManager>
{
    #region 1. Fields
    //

    #endregion

    #region 1-1. Notion API
    
    private const string NOTION_CURRENT_USER_URL = "https://api.notion.com/v1/users/me";
    
    #endregion
    
    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public override void Initialize()
    {
        RequestNotionUserAsync().Forget();
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Methods

    private async UniTask<string> RequestNotionUserAsync()
    {
        var notionToken = Environment.GetEnvironmentVariable("NotionToken", EnvironmentVariableTarget.User);

        if (notionToken == null)
        {
            Debug.LogError("[Notion] PAT이 시스템 환경 변수에서 존재하지 않습니다.");
            return string.Empty;
        }

        using (var request = UnityWebRequest.Get(NOTION_CURRENT_USER_URL))
        {
            request.timeout = 30;
            
            request.SetRequestHeader("Authorization", $"Bearer {notionToken}");
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

    #endregion
}

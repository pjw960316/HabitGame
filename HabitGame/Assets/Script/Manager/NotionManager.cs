using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public sealed class NotionManager : ManagerBase<NotionManager>
{
    #region 1. Fields

    //

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    public override void Initialize()
    {
        PracticeNotion();
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Methods

    private void PracticeNotion()
    {
        // 이곳에서 자유롭게 연습한다.
    }

    public async UniTask<string> RequestNotionUserAsync(string patFilePath)
    {
        if (!File.Exists(patFilePath))
        {
            Debug.LogError($"[Notion] PAT 파일을 찾을 수 없습니다: {patFilePath}");
            return string.Empty;
        }

        var pat = File.ReadAllText(patFilePath).Trim();

        if (string.IsNullOrWhiteSpace(pat))
        {
            Debug.LogError("[Notion] PAT 파일이 비어 있습니다.");
            return string.Empty;
        }

        using (var request = UnityWebRequest.Get("https://api.notion.com/v1/users/me"))
        {
            request.timeout = 30;
            request.SetRequestHeader("Authorization", $"Bearer {pat}");
            request.SetRequestHeader("Notion-Version", "2026-03-11");
            request.SetRequestHeader("Accept", "application/json");

            var operation = request.SendWebRequest();

            // HTTP 오류도 응답 내용을 확인할 수 있도록 완료 후 직접 결과를 확인한다.
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

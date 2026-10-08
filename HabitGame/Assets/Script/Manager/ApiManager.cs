using System;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

// NOTE
// Notion과 OpenAI의 HTTP 요청만 담당한다. 응답 해석과 게임 규칙은 호출부에서 결정한다.
// API 키를 사용하는 직접 호출은 Unity Editor에서 확인하는 용도로 제한한다.
public sealed class ApiManager : ManagerBase<ApiManager>
{
    private const string NOTION_API_URL = "https://api.notion.com/v1";
    private const string NOTION_API_VERSION = "2026-03-11";
    private const string OPENAI_API_URL = "https://api.openai.com/v1/responses";
    private const string NOTION_API_KEY_NAME = "NOTION_API_KEY";
    private const string OPENAI_API_KEY_NAME = "OPENAI_API_KEY";

    [Serializable]
    private sealed class NotionSearchRequest
    {
        public string query;
        public string start_cursor;
        public NotionSearchFilter filter = new();
    }

    [Serializable]
    private sealed class NotionSearchFilter
    {
        public string property = "object";
        public string value = "page";
    }

    [Serializable]
    private sealed class OpenAiResponseRequest
    {
        public string model;
        public string input;
        public bool store = false;
    }

    // NOTE
    // Notion 검색 결과는 여러 페이지로 나뉜다. 반환된 next_cursor로 다음 페이지를 요청한다.
    public async UniTask<string> SearchNotionPagesAsync(string titleQuery = null, string startCursor = null)
    {
        var apiKey = GetApiKey(NOTION_API_KEY_NAME);
        var body = JsonUtility.ToJson(new NotionSearchRequest
        {
            query = titleQuery,
            start_cursor = startCursor
        });

        using var request = CreatePostRequest($"{NOTION_API_URL}/search", body);
        SetNotionHeaders(request, apiKey);

        return await SendRequestAsync(request);
    }

    // NOTE
    // 문서 본문은 page의 하위 block이다. 중첩 block은 해당 block ID로 다시 조회한다.
    public async UniTask<string> GetNotionBlockChildrenAsync(string blockId, string startCursor = null)
    {
        if (!Guid.TryParse(blockId, out _))
        {
            throw new ArgumentException("올바른 Notion 블록 ID가 필요합니다.", nameof(blockId));
        }

        var apiKey = GetApiKey(NOTION_API_KEY_NAME);
        var url = $"{NOTION_API_URL}/blocks/{blockId}/children";
        if (!string.IsNullOrWhiteSpace(startCursor))
        {
            url += $"?start_cursor={UnityWebRequest.EscapeURL(startCursor)}";
        }

        using var request = UnityWebRequest.Get(url);
        request.timeout = 30;
        SetNotionHeaders(request, apiKey);

        return await SendRequestAsync(request);
    }

    public async UniTask<string> CreateGptResponseAsync(string model, string prompt)
    {
        if (string.IsNullOrWhiteSpace(model))
        {
            throw new ArgumentException("OpenAI 모델 이름이 필요합니다.", nameof(model));
        }

        if (string.IsNullOrWhiteSpace(prompt))
        {
            throw new ArgumentException("프롬프트가 필요합니다.", nameof(prompt));
        }

        var apiKey = GetApiKey(OPENAI_API_KEY_NAME);
        var body = JsonUtility.ToJson(new OpenAiResponseRequest
        {
            model = model,
            input = prompt
        });

        using var request = CreatePostRequest(OPENAI_API_URL, body);
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        return await SendRequestAsync(request);
    }

    private static UnityWebRequest CreatePostRequest(string url, string body)
    {
        var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST)
        {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body)),
            downloadHandler = new DownloadHandlerBuffer(),
            timeout = 120
        };

        request.SetRequestHeader("Content-Type", "application/json");
        return request;
    }

    private static void SetNotionHeaders(UnityWebRequest request, string apiKey)
    {
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        request.SetRequestHeader("Notion-Version", NOTION_API_VERSION);
    }

    private static async UniTask<string> SendRequestAsync(UnityWebRequest request)
    {
        try
        {
            await request.SendWebRequest().ToUniTask();
            return request.downloadHandler.text;
        }
        catch (UnityWebRequestException exception)
        {
            throw new InvalidOperationException($"API 요청에 실패했습니다. HTTP 상태 코드: {request.responseCode}", exception);
        }
    }

    private static string GetApiKey(string variableName)
    {
#if UNITY_EDITOR
        var apiKey = Environment.GetEnvironmentVariable(variableName);
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException($"환경 변수 {variableName}가 설정되지 않았습니다.");
        }

        return apiKey;
#else
        throw new NotSupportedException("API 키를 사용하는 직접 호출은 Unity Editor에서만 지원합니다.");
#endif
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

// Note 1
// 실제로 서버도, 패킷 처리도 존재하지 않는다.
// 클라이언트에서 요청을 했을 때 100%로 Success를 리턴하는 책임 수행만 한다.

// Note 2
// 원래 클라는 기초적인 검증 후, 서버에 실제 로직을 요청하지만
// 현재는 서버에 일단 요청을 하고, Success 처리 시에 클라에서 로직을 수행하는 구색만 갖춘 구현을 한다.

// Note 3
// 서버의 DB 대신, 클라의 Xml이 DB를 대체하므로
// Xml을 Serialize 할 책임은 MockServerManager가 갖고 있다.
public class MockServerManager : ManagerBase<MockServerManager>, IManager
{
    #region 1. Fields

    private const float SERVER_DELAY = 0.5f;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
        Test();
    }

    #endregion

    #region 4. Methods

    public void SetModel(IEnumerable<IModel> models)
    {
    }

    public async UniTask UpdateXmlDataAsync()
    {
        Debug.Log("1");
        await UniTask.Delay(1000);
        Debug.Log("2");
    }
    //test
    private void Test()
    {
        UpdateResult().Forget();

        Debug.Log("Main : Run Event Loop");
    }

    private async UniTaskVoid UpdateResult()
    {
        Debug.Log("UpdateResult");

        var result = await SumAsync(1, 200);

        Debug.Log($"{result}");
    }

    private async UniTask<int> SumAsync(int n1, int n2)
    {
        await UniTask.Delay(1000);

        var ret = n1 + n2;

        return ret;
    }

    
    #endregion

    #region 5. EventHandlers

    // default

    #endregion
}

public enum EServerResult
{
    SUCCESS,
    FAIL
}
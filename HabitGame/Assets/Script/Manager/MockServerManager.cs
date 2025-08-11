using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Cysharp.Threading.Tasks;

// Note 1
// 실제로 서버도, 패킷 처리도 존재하지 않는다.
// 클라이언트에서 요청을 했을 때 100%로 Success를 리턴하는 책임 수행만 한다.

// Note 2
// 원래 클라는 기초적인 검증 후, 서버에 실제 로직을 요청하지만
// 현재는 서버에 일단 요청을 하고, Success 처리 시에 클라에서 로직을 수행하는 구색만 갖춘 구현을 한다.
public class MockServerManager : ManagerBase<MockServerManager>, IManager
{
    #region 1. Fields

    private const int SERVER_DELAY_MILLISECONDS = 500;

    #endregion

    #region 2. Properties

    // default

    #endregion

    #region 3. Constructor

    public void Initialize()
    {
        //
    }

    #endregion

    #region 4. Methods

    public void SetModel(IEnumerable<IModel> models)
    {
    }

    // Note
    // 지연 + 무조건 SUCCESS return 하는 가짜 서버 구조
    // SUCCESS 하면 클라에서 데이터 변경하고, 그걸 xml네 저장하는 걸 허락한다.
    public async UniTask<EServerResult> RequestServerValidation(int totalReward)
    {
        var serverResult = await GenerateMockServerLogic();

        if (serverResult == EServerResult.SUCCESS)
        {
            return serverResult;
        }
        
        return EServerResult.FAIL;
    }

    // note
    // 실제 서버에서는 서버로직에 따라서 다양한 enum이 return 될 것
    private async UniTask<EServerResult> GenerateMockServerLogic()
    {
        await GenerateServerDelay();
        
        return EServerResult.SUCCESS;
    }

    private async UniTask GenerateServerDelay()
    {
        await UniTask.Delay(SERVER_DELAY_MILLISECONDS);
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
## :fire: Test

<br><br>

## :fire: MockServer
#### 기존 Client-Server 구조 : Server가 실제 데이터 변경
- :one: 게임에서 유저 인풋
- :two: 데이터 변경 요청 발생
- :three: Presenter가 서버에 **상응하는 패킷으로 데이터 변경** 요청
- :four: 서버가 비동기적으로 정상 패킷을 확인하고, 검증 로직 수행
- :five: 검증 완료 시에 server의 데이터를 수정하고, packet에 수정된 데이터와 함께 SUCCESS를 리턴한다.
- :six: 클라는 패킷의 결과를 받아 콜백으로 Data 관리 Manager를 통해 Data 갱신
- :seven: Presenter는 View에 변경된 Data를 업데이트
- :eight: 유저는 변경된 데이터 확인

<br>

#### HabitGame의 Client-MockServer 구조 : Server는 비동기만 구현, 클라에서 실제 데이터 변경
- :one: 게임에서 유저 인풋
- :two: 데이터 변경 요청 발생
- :three: Presenter가 Mock서버에 **동일한 검증 메서드** 요청
- :four: 서버는 비동기적으로 0.5초의 딜레이를 발생시키고, 무조건 적인 SUCCESS를 리턴
- :five: 클라에서 Manager를 통해 직접 데이터를 갱신하고 이를 Xml에 Serialize 한다.
- :six: 클라는 패킷의 결과를 받아 콜백으로 Data 관리 Manager를 통해 Data 갱신
- :seven: Presenter는 View에 변경된 Data를 업데이트
- :eight: 유저는 변경된 데이터 확인

<br><br>

## :fire: MVP 구조
- View Initialize() -> View안에 존재하는 작은 Widget View들 Initialize() -> CreatePresenterByManager -> Presenter Initialize() -> Presenter SetView -> View BindEvent

<br><br>

## 리팩터링 하면서
- 처음에는 Manager를 Model처럼 사용도 했지만, 생각해보면 Manager의 코드가 늘어나기 쉬운 구조다. 그래서 Presenter에 직접 Model을 두도록 AlarmPresenter에서 리팩터링을 해 보았다.
  - Alarm은 Sound니까 SoundManager를 뒀지만, Alarm에서만 사용하는 Sound가 존재할 것 이고, Alarm에서만 사용하는 Enum이 존재할 것 이다. <br> 그걸 SoundManager에서 모두 들고 있는 게 이상하다 느껴 리팩터링을 진행했다.
  - 결론적으로, SoundManager는 지금 재생하고 되는 Sound를 관리하는 책임만 하도록 변경
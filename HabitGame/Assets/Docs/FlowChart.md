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
- View가 Presenter를 무조건 먼저 생성하고, Presenter의 Initialize()로 데이터 세팅과 로직 세팅을 완료하면, View가 Initialize()한다.
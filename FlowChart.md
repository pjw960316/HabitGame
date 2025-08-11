## :fire: Test

<br><br>

## :fire: MockServer
#### :one: 기존 Client-Server 구조 : Server가 실제 데이터 변경
- 서버가 실제 Server DB를 보유
- 1. 게임에서 유저 인풋
- 2. 데이터 변경 요청 발생
- 3. Presenter가 서버에 **상응하는 패킷으로 데이터 변경** 요청
- 4. 서버가 비동기적으로 정상 패킷을 확인하고, 검증 로직 수행
- 5. 검증 완료 시에 server의 데이터를 수정하고, packet에 수정된 데이터와 함께 SUCCESS를 리턴한다.
- 6. 클라는 패킷의 결과를 받아 콜백으로 Data 관리 Manager를 통해 Data 갱신
- 7. Presenter는 View에 변경된 Data를 업데이트
- 8. 유저는 변경된 데이터 확인

<br>

#### :two: HabitGame의 Client-MockServer 구조 : Server는 비동기만 구현, 클라에서 실제 데이터 변경
- 클라가 실제 DB를 Xml로 간단하게 보유
- 1. 게임에서 유저 인풋
- 2. 데이터 변경 요청 발생
- 3. Presenter가 Mock서버에 **동일한 검증 메서드** 요청
- 4. 서버는 비동기적으로 0.5초의 딜레이를 발생시키고, 무조건 적인 SUCCESS를 리턴
- 5. 클라에서 Manager를 통해 직접 데이터를 갱신하고 이를 Xml에 Serialize 한다.
- 6. 클라는 패킷의 결과를 받아 콜백으로 Data 관리 Manager를 통해 Data 갱신
- 7. Presenter는 View에 변경된 Data를 업데이트
- 8. 유저는 변경된 데이터 확인
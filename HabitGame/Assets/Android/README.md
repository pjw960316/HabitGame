# Android

Unity 프로젝트에서 사용하는 Android 전용 코드와 빌드 설정을 관리한다.

## 구조

- `HealthConnect.androidlib`
  - Unity가 Android Library 플러그인으로 인식하는 모듈
  - `build.gradle`: Android 라이브러리 의존성 관리
  - `src/main/AndroidManifest.xml`: 앱 빌드 시 병합할 Android 설정
  - `src/main/java`: 이후 추가할 Unity ↔ Android 브리지 코드

`.androidlib` 확장자가 붙은 폴더는 Unity가 Android 빌드에 자동으로 포함한다.

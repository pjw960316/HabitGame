# :fire: Rider 2026.2 설치 플러그인 목록

- 확인 경로: `C:\Users\pjw96\AppData\Roaming\JetBrains\Rider2026.2`
- 확인 날짜: 2026-08-18
- 번들 플러그인 (bundled_plugins.txt) : 140개
- 사용자 플러그인 경로에서 확인된 항목: 5개
- 비활성화된 플러그인 (disabled_plugins.txt) : 65개

> 비활성화는 플러그인을 삭제한 것이 아니다. 필요해지면 Rider 설정에서 다시 활성화할 수 있다.

<br>

# :fire: 필수 plugin -> 삭제하면 Rider가 켜지지 않는다.

| 플러그인 | Plugin ID | 이유 |
|---|---|---|
| IntelliJ Platform Core | `com.intellij` | Rider 기본 동작 또는 플랫폼 런타임에 사용 |
| .NET / ReSharper 공통 엔진 | `com.jetbrains.dotCommon` | Rider 기본 동작 또는 플랫폼 런타임에 사용 |
| Rider Frontend | `com.jetbrains.rider.frontend` | Rider 기본 동작 또는 플랫폼 런타임에 사용 |
| Rider IntelliJ 연동 구성요소 | `rider.intellij.plugin.appender` | Rider 기본 동작 또는 플랫폼 런타임에 사용 |
| JCEF 웹 UI 런타임 | `com.intellij.modules.jcef` | Rider 기본 동작 또는 플랫폼 런타임에 사용 |
| JSON 플랫폼 지원 | `com.intellij.modules.json` | Rider 기본 동작 또는 플랫폼 런타임에 사용 |
| IDE 이미지 지원 | `com.intellij.platform.images` | Rider 기본 동작 또는 플랫폼 런타임에 사용 |

<br>

# :fire: 내가 설치한 plugin

| 플러그인 | Plugin ID | 버전 | 용도 |
|---|---|---:|---|
| Gerry Themes | `com.jetbrains.gerryPurpleTheme` | 2026.1.0806 | 에디터 테마 |
| Gradle | `com.intellij.gradle` | 262.8665.369 | Gradle 빌드 도구 지원 |
| JetBrains AI Assistant | `com.intellij.ml.llm` | 262.8665.411 | AI 코딩 보조 |
| CognitiveComplexity | `cognitivecomplexity-rider` | 2026.2.0 | C# 코드의 인지 복잡도 분석 |
| Subversion | `Subversion` | 262.8665.369 | SVN 버전 관리 |

<br>

# :fire: unity & C# 관련 plugin

- 중요도 기준: `⭐⭐⭐` Unity/C# 개발 핵심, `⭐⭐` 분석·디버깅에 유용, `⭐` 현재 프로젝트에서는 선택 사항

| 플러그인 | Plugin ID | 설명 | 중요도 |
|---|---|---|:---:|
| Unity Support | `com.intellij.resharper.unity` | Unity 프로젝트 인식, 코드 분석, 에디터 연동 및 디버깅 지원 | ⭐⭐⭐ |
| Unity Debugger Texture Visualizer | `com.jetbrains.rider.plugins.unity.debugger.textureVisualizer` | 디버깅 중 Unity 텍스처 내용을 시각적으로 확인 | ⭐⭐ |
| dotCover | `com.jetbrains.dotCover` | .NET 테스트의 코드 커버리지 측정 | ⭐⭐ |
| dotTrace / dotMemory | `com.jetbrains.dotTrace.dotMemory` | CPU 성능과 메모리 사용량 프로파일링 | ⭐⭐ |
| Dynamic Program Analysis | `com.jetbrains.dpa` | 실행 중인 .NET 프로그램의 성능과 메모리 문제 탐지 | ⭐⭐ |
| Entity Framework Core | `me.seclerp.rider.plugins.efcore` | EF Core 마이그레이션과 데이터베이스 작업 지원 | ⭐ |
| .NET Aspire | `me.rafaelldi.aspire` | .NET Aspire 분산 애플리케이션 개발 지원 | ⭐ |
| dotnet watch | `com.jetbrains.rider.plugins.dotnetwatch` | .NET 애플리케이션 변경 감지 및 자동 재실행 | ⭐ |
| NuGet Package Manager Console | `com.jetbrains.rider.plugins.pmc` | NuGet 명령을 콘솔에서 실행 | ⭐ |
| Razor Support | `com.jetbrains.rider.razor` | ASP.NET Razor 파일 편집과 코드 분석 | ⭐ |
| XAML Previewer | `com.jetbrains.xaml.previewer` | XAML UI 미리보기 | ⭐ |
| SQL Project Support | `com.jetbrains.sqlproj` | SQL 프로젝트 파일과 빌드 작업 지원 | ⭐ |
| .NET Diagrams | `com.jetbrains.rider.diagram` | .NET 타입과 의존 관계를 다이어그램으로 확인 | ⭐⭐ |

<br>

# :fire: 비활성화한 plugin

### 현재 비활성화된 플러그인

- `AngularJS`
- `com.deadlock.scsyntax`
- `com.dmarcotte.handlebars`
- `com.intellij.database`
- `com.intellij.dev`
- `com.intellij.ja`
- `com.intellij.ko`
- `com.intellij.kubernetes`
- `com.intellij.microservices.ui`
- `com.intellij.plugins.watcher`
- `com.intellij.plugins.webcomponents`
- `com.intellij.properties`
- `com.intellij.react`
- `com.intellij.rider.godot`
- `com.intellij.rider.godot.community`
- `com.intellij.rider.godot.gdscript`
- `com.intellij.stylelint`
- `com.intellij.swagger`
- `com.intellij.tailwindcss`
- `com.intellij.zh`
- `com.jetbrains.gateway`
- `com.jetbrains.plugins.ini4idea`
- `com.jetbrains.plugins.jade`
- `com.jetbrains.plugins.rider.privateFeedAuth`
- `com.jetbrains.plugins.webDeployment`
- `com.jetbrains.remoteDevServer`
- `com.jetbrains.restClient`
- `com.jetbrains.rider.diagram`
- `com.jetbrains.rider.fsharp`
- `com.jetbrains.rider.plugins.noesis`
- `com.jetbrains.rider.plugins.verse`
- `com.jetbrains.rider.publish.webDeploy`
- `com.jetbrains.sqlproj`
- `com.jetbrains.station`
- `Docker`
- `HtmlTools`
- `idea.plugin.protoeditor`
- `intellij.git.commit.modal`
- `intellij.javascript.eslint`
- `intellij.nextjs`
- `intellij.prettierJS`
- `intellij.vitejs`
- `intellij.webpack`
- `JavaScript`
- `JavaScriptDebugger`
- `jshint`
- `JSIntentionPowerPack`
- `Karma`
- `me.rafaelldi.aspire`
- `me.seclerp.rider.plugins.efcore`
- `NodeJS`
- `org.intellij.plugins.postcss`
- `org.jetbrains.plugins.docker.gateway`
- `org.jetbrains.plugins.less`
- `org.jetbrains.plugins.node-remote-interpreter`
- `org.jetbrains.plugins.remote-run`
- `org.jetbrains.plugins.sass`
- `org.jetbrains.plugins.vagrant`
- `org.jetbrains.plugins.vue`
- `PerforceDirectPlugin`
- `Refactor-X`
- `tslint`
- `unreal-link`
- `W3Validators`
- `XPathView`

<br>

# :fire: 그 외 plugin

> 아래는 Rider에 포함된 플러그인 목록이며, `비활성화한 plugin`에 기록된 항목도 포함한다.

### 미분류

- `com.intellij.cidr.debugger`
- `com.intellij.cidr.parallelStacks`
- `com.intellij.mermaid`
- `com.intellij.plugins.watcher`
- `com.intellij.rider.godot`
- `com.intellij.rider.godot.community`
- `com.intellij.rider.godot.gdscript`
- `com.jetbrains.plugins.rider.privateFeedAuth`
- `com.jetbrains.rider-cpp`
- `com.jetbrains.rider.livePlusPlus`
- `com.jetbrains.rider.plugins.noesis`
- `com.jetbrains.rider.plugins.verse`
- `com.jetbrains.rider.publish.webDeploy`
- `intellij.bookmarks.plugin`
- `intellij.debuggerMcp`
- `intellij.execution.serviceView.plugin`
- `intellij.grid.core.plugin`
- `intellij.grid.plugin`
- `intellij.java.aetherDependencyResolver.plugin`
- `intellij.libraries.misc.plugin`
- `intellij.navbar.plugin`
- `intellij.recentFiles.plugin`
- `intellij.ssh.plugin`
- `intellij.structuralSearch.plugin`
- `intellij.structureView.plugin`
- `intellij.testRunner.plugin`
- `intellij.todo.plugin`
- `intellij.vcs.split.plugin`
- `org.intellij.qodana`
- `org.jetbrains.fortea`
- `org.jetbrains.plugins.renderdoc`
- `Rider UI Theme Pack`
- `unreal-link`

### AI-Powered

- `com.intellij.mcpServer`

### Build Tools

- `com.intellij.cmake`

### Database

- `com.intellij.database`

### Deployment

- `com.intellij.kubernetes`
- `com.jetbrains.plugins.webDeployment`
- `Docker`
- `org.jetbrains.plugins.vagrant`

### HTML and XML

- `HtmlTools`
- `Refactor-X`
- `XPathView`

### IDE Localization

- `com.intellij.ja`
- `com.intellij.ko`
- `com.intellij.zh`

### IDE Settings

- `com.intellij.platform.acp`
- `com.intellij.platform.daemon`
- `com.intellij.settingsSync`
- `org.editorconfig.editorconfigjetbrains`

### JavaScript Frameworks and Tools

- `AngularJS`
- `com.deadlock.scsyntax`
- `com.intellij.plugins.webcomponents`
- `com.intellij.react`
- `com.intellij.stylelint`
- `intellij.javascript.eslint`
- `intellij.nextjs`
- `intellij.prettierJS`
- `intellij.vitejs`
- `intellij.webpack`
- `JavaScript`
- `JavaScriptDebugger`
- `jshint`
- `JSIntentionPowerPack`
- `Karma`
- `NodeJS`
- `org.jetbrains.plugins.node-remote-interpreter`
- `org.jetbrains.plugins.vue`
- `tslint`

### JVM Tools

- `org.jetbrains.debugger.streams`

### Keymap

- `com.intellij.plugins.resharperkeymap`
- `com.intellij.plugins.visualassistkeymap`
- `com.intellij.plugins.visualstudio2022keymap`
- `com.intellij.plugins.visualstudiokeymap`
- `com.intellij.plugins.vscodekeymap`

### Languages

- `com.intellij.groovy.scripting`
- `com.intellij.jsonpath`
- `com.intellij.properties`
- `com.jetbrains.plugins.ini4idea`
- `com.jetbrains.rider.fsharp`
- `com.jetbrains.sh`
- `idea.plugin.protoeditor`
- `org.intellij.plugins.markdown`
- `org.jetbrains.plugins.textmate`
- `org.jetbrains.plugins.yaml`
- `tanvd.grazi`

### Local AI/ML Tools

- `com.intellij.completion.ml.ranking`
- `com.intellij.marketplace.ml`
- `com.intellij.searcheverywhere.ml`
- `org.jetbrains.completion.full.line`

### Microservices

- `com.intellij.microservices.ui`
- `com.intellij.swagger`
- `com.jetbrains.restClient`

### Other Tools

- `com.intellij.diagram`
- `com.jetbrains.rider.plugins.debuggerGrid`
- `com.jetbrains.rider.plugins.debuggerLinq`
- `org.jetbrains.plugins.terminal`

### Platform Development

- `com.intellij.dev`
- `com.jetbrains.performancePlugin`

### Remote Development

- `com.jetbrains.gateway`
- `com.jetbrains.remoteDevelopment`
- `com.jetbrains.remoteDevServer`
- `com.jetbrains.station`
- `intellij.platform.ijent.bundledBinaries`
- `org.jetbrains.plugins.docker.gateway`
- `org.jetbrains.plugins.remote-run`

### Style Sheets

- `com.intellij.css`
- `com.intellij.tailwindcss`
- `org.intellij.plugins.postcss`
- `org.jetbrains.plugins.less`
- `org.jetbrains.plugins.sass`
- `W3Validators`

### Template Languages

- `com.dmarcotte.handlebars`
- `com.jetbrains.plugins.jade`

### Version Controls

- `com.intellij.azureDevops`
- `Git4Idea`
- `intellij.git.commit.modal`
- `org.jetbrains.plugins.github`
- `org.jetbrains.plugins.gitlab`
- `PerforceDirectPlugin`

# Necrosoul

Unity로 제작한 2D 액션 게임 프로젝트입니다. 방을 탐험하고 적을 처치하며, 아이템과 특성으로 플레이어를 강화해 다음 스테이지로 진행합니다.

## 개발 환경과 실행

- Unity **2020.3.17f1** (`ProjectSettings/ProjectVersion.txt` 기준)
- C# / Unity 2D

1. 저장소를 내려받습니다.
2. Unity Hub에서 이 저장소의 최상위 폴더를 프로젝트로 추가합니다.
3. Unity 2020.3.17f1로 열고 패키지 복원과 에셋 임포트가 끝날 때까지 기다립니다.
4. `Assets/Scenes/Main 1.unity`를 열고 Play를 실행합니다.

빌드는 Unity의 **File → Build Settings**에서 진행합니다. 기존에 등록된 씬 순서는 유지되어 있습니다. 생성한 빌드는 `Build/` 또는 `Builds/`에 저장합니다.

## 기본 조작

| 키 | 동작 |
| --- | --- |
| 방향키 | 이동 |
| Z | 공격 |
| X | 점프 |
| Shift | 대시 |
| Esc | 메뉴 |

기존 안내에는 Esc 메뉴 기능이 일부 미완성이라고 명시되어 있습니다. 아이템 장착은 `Esc → 인벤토리`에서 안내하고 있습니다.

## 프로젝트 구조

```text
Assets/           게임 소스, 씬, 프리팹, 그래픽, 사운드
  Scenes/         메인 화면, 스테이지 선택, 스테이지, 로딩 씬
  Script/         게임 로직
Packages/         Unity 패키지 의존성과 잠금 파일
ProjectSettings/  Unity 버전, 빌드 씬, 입력 등 프로젝트 설정
```

주요 코드 경로:

- [`Assets/Script/player`](Assets/Script/player): 플레이어
- [`Assets/Script/enemy`](Assets/Script/enemy): 적의 AI와 공격
- [`Assets/Script/room`](Assets/Script/room): 방 생성과 기능
- [`Assets/Script/item`](Assets/Script/item): 아이템 생성과 효과
- [`Assets/Script/system`](Assets/Script/system): 게임 진행과 공통 기능
- [`Assets/Script/ui`](Assets/Script/ui): UI

상세 분류와 작업 범위는 [`Assets/Script/★readme★.txt`](Assets/Script/★readme★.txt)를 참고하세요. 기존 설명에서 `yl` 폴더는 본인이 작업하지 않은 코드로 구분되어 있습니다.

## 구현 상태

기존 프로젝트 안내를 기준으로 1스테이지가 존재하며, 2스테이지와 보스는 미완성으로 기록되어 있습니다. 실제 실행으로 추가 검증한 내용은 아닙니다.

## 저장소 관리

이 저장소는 Necrosoul 프로젝트를 중심으로 정리했습니다. 기존 다른 프로젝트와 실행 빌드는 저장소 밖에 별도 보관하며, 과거 커밋 이력은 유지합니다.

`Assets`와 `.meta` 파일, `Packages`, `ProjectSettings`는 함께 버전 관리합니다. `Library`, `Temp`, `Logs`, `obj`, `UserSettings`, 빌드 결과와 로컬 세이브는 제외합니다. 캐시는 Unity에서 프로젝트를 열 때 다시 생성됩니다.

외부 에셋의 원본 안내와 관련 파일은 각 에셋 폴더에 유지했습니다.

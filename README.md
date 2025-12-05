# Gloomy

내일배움 캠프 스타르타 1조 gloomy 프로토타입

## 프로젝트 개요

- 장르: 3D 내러티브 어드벤쳐
- 범위: Gloomy 프로토타입 버전 — 챕터 2
- 엔진: Unity 2022.3.62f2 (LTS)
- 빌드 플랫폼: PC

## 플레이 방법

### 기본

- 기본 이동: WASD

### 그림자 추격 (기본 이동에 추가)

- 점프: Space
- 대시: F
- 슬라이딩: R

## 프로젝트 구조

```
Assets/
 ├─ 00_Scenes/              # 씬
 ├─ 01_Scripts/             # 게임 로직 및 C# 스크립트
 ├─ 02_Prefabs/             # 프리팹
 ├─ 05_Animations/          # 애니메이션 및 Animator Controller
 ├─ 10_Artworks/            # 원본 이미지, 일러스트, 아트 리소스
 ├─ 11_Material/            # 머티리얼
 ├─ 12_Meshes/              # 메시, FBX 등 3D 모델
 ├─ 13_Textures/            # 텍스처
 ├─ 14_Fonts/               # 폰트 파일
 ├─ 30_InputAction/         # Input System
 └─ Settings/               # 프로젝트 설정 에셋(URP Settings 포함 가능)

```

## 기술 스택

- Unity 202X.x LTS
- C#

### 핵심 기능 패키지

- **Input System**
- **URP (Universal Render Pipeline)**
- **Cinemachine**
- **TextMeshPro**
- **Timeline**
- **UGUI**

### 제작/편의 패키지

- **Unity Recorder** — 녹화 기능
- **Visual Scripting** — Bolt 기반 시각적 스크립트
- **IDE 관련 패키지** — Rider, Visual Studio, VSCode 플러그인
- **Collaborate Proxy** — 협업(사용 여부 알 수 없음)

### 기본 Unity 모듈

- Physics / Physics2D
- Animation
- Audio
- ParticleSystem
- Video
- AI / NavMesh
- 등(모두 Unity 기본 제공 모듈)

## 팀원 및 역할

| 팀원       | 역할                                                                                                                                                              |
| ---------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **신주은** | • 그림자 시스템 전반<br>• 잡힘 상태 Vignette + 대시키 연타 UI<br>• 플레이어·그림자 근접 거리 UI<br>• 맵 지형 구성<br>• 빛(완료 지점) 체크 로직<br>• 투명인간 처리 |
| **이요한** | • 플레이어 이동(기본 이동 + 추가 이동) 구현<br>• 그림자에게 잡혔을 때 Vignette 연출<br>• 대시키 입력으로 탈출하는 로직                                            |
| **유가영** | • 기획, 일정 관리                                                                                                                                                 |
| **전수라** | • 카드게임 및 일기장 UI(사진·줄글 작성)<br>• 추격 씬 빛(완료 지점) 연출<br>• 발표                                                                                 |
| **함효승** | • 그림자 추격 씬의 장애물 생성 및 구성                                                                                                                            |

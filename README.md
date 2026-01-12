# 2D Tower Defense
<img width="676" height="371" alt="Image" src="https://github.com/user-attachments/assets/6009fcc6-08e3-42f6-bf55-927cdbc46509" />

#  2D Tower Defense
[1인 개발 프로젝트] My TIL link is https://youtu.be/hG7qmccQNg0

## 1. 프로젝트 개요

좀비vs식물을 레퍼런스로 제작한 모작입니다.

Unity를 활용하여 2D로 제작하였습니다

개발기간 : 2025.07.07 ~ 2525.07.18

## 2. 플로우차트 및 다이어그램

### 2.1 플로우 차트

<img width="2916" height="1193" alt="Image" src="https://github.com/user-attachments/assets/1df6d1ea-7b20-479d-b959-aa6bc4ac9e22" />

------------------------------------------------------------------------------

### 2.2 다이어그램

#### 게임플레이 다이어그램

<img width="814" height="376" alt="Image" src="https://github.com/user-attachments/assets/357c2c1a-e8e2-4dab-97a2-aca81e645cdf" />

------------------------------------------------------------------------------

#### 데이터 다이어그램

<img width="598" height="344" alt="Image" src="https://github.com/user-attachments/assets/9e8cfe01-2902-4f40-97c6-d7723b17dfa7" />

------------------------------------------------------------------------------

#### 상점 다이어그램

<img width="332" height="174" alt="Image" src="https://github.com/user-attachments/assets/c715cf2a-f1b7-4786-8de5-c9331aa00035" />

## 3. 주요 기능
   
### 3.1 게임시스템

### [TileGenerator.cs](Assets/Project/Scripts/TileGenerator.cs)

#### 💡 역할

* Grid 기준 좌표계를 사용해 플레이 시작 지점을 중심으로 일정 범위(genDistx, genDisty)만큼 타일을 자동 생성합니다.

* 가장자리에는 별도 프리팹으로 타일을 배치하여 맵의 끝을 시각적으로 구분합니다.
  
#### 📌 주요 메서드

* GenerteTiles() : genDistx, genDisty 범위를 기준으로 이중 반복문 실행, 셀 좌표 기준으로 타일 존재 여부 확인

### [GameManager](Assets/Project/Scripts/GameManager.cs)

#### 💡 역할

* 게임 전체의 라운드 진행, 적 스폰, 재화 관리, 승패 판정을 총괄하는 클래스입니다.
  
#### 📌 주요 메서드

* IEnumerator CoinPlus() : 일정 시간마다 코인 자동 증가

* IEnumerator EnemySpawnCoroutine() : 일정 시간 대기 후 적을 주기적으로 스폰

* EnemySpawn() : 라운드 및 랜덤 값에 따라 적 타입 결정

* OnNextRound() : 다음 라운드 전환 처리

### [DropTower](Assets/Project/Scripts/DropTower.cs)

#### 💡 역할

* GUI 드래그 앤 드롭 기반으로 타워를 타일 위에 배치, 교체, 회수하는 클래스입니다.
  
* 드래그된 UI 스프라이트를 실제 타워 프리팹과 매핑하여 월드에 스폰하고, 코인 소모ㅡ환급, 타일 점유 상태, 풀링(Lean Pool)을 함께 관리합니다.
  
#### 📌 주요 메서드

* OnDrop(PointerEventData eventData) : 드래그된 UI 스프라이트 추출, 스프라이트에 대응하는 타워 프리팹 탐색

* GetPrefabFromSprite(Sprite sprite) : 스프라이트 이름 기준으로 프리팹 매핑 검색

* GetWorldPositon(RectTransform rectTransform) : UI RectTransform의 월드 코너 좌표를 기반으로 정확한 타워 월드 스폰 위치 계산

### [DragTower](Assets/Project/Scripts/DragTower.cs)

#### 💡 역할

* UI 상의 아이콘(타워, 아이템 등)을 드래그 가능한 상태로 변환하고, 드래그 중 시각적 아이콘을 생성하여 마우스를 따라 이동시키는 공통 드래그 클래스입니다.
  
#### 📌 주요 메서드

*SetDraggedPosition(PointerEventData eventData) : 현재 포인터 좌표를 기준으로 아이콘의 월드 위치 계산

### 3.2 데이터

### [TowerDatabase](Assets/Project/Scripts/Json/TowerDatabase.cs)

#### 💡 역할

* 타워 디펜스 게임에서 사용되는 모든 타워의 데이터 저장소 클래스입니다.

### [TowerSlotSave](Assets/Project/Scripts/Json/TowerSlotSave.cs)

#### 💡 역할

* 플레이어의 성장 데이터를 PlayerPrefs 기반으로 저장,조회하는 저장 클래스입니다.

### [ScoreSave](Assets/Project/Scripts/ScoreSave.cs)

#### 💡 역할

* 플레이 결과 데이터와 재화를 PlayerPrefs에 저장, 조회하는 결과 저장 및 누적 보상 관리 클래스입니다.

### 3.3 상점

### [ShopManager](Assets/Project/Scripts/ShopManager.cs)

#### 💡 역할

* 상점(UI)와 업그레이드 로직을 총괄하는 관리자 클래스입니.
  
#### 📌 주요 메서드

* UpdateUI() : 슬롯 수 및 각 타워 강화 단계에 따라 버튼 활성/비활성, 가격 텍스트 / “최대 강화” 문구 처리

* BuySlotExpansion() : Gold를 소모해 최대 타워 슬롯 수 증가
  
* SaveTowerDatabase() : 수정된 TowerDatabase를 JSON으로 직렬화

------------------------------------------------------------------------------

## 4. 기술 스택
   
* C#
* Unity
* Fork + Github(형상 관리)
* Drag & Drop
* LeanPool 기반 ObjectPooling
* Json 데이터 관리

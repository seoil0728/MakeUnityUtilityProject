# Manual: LocalizationManager

`LocalizationManager`는 게임 내의 텍스트 데이터를 다국어로 관리하고, 런타임에 실시간으로 언어를 변경할 수 있게 해주는 시스템입니다. CSV 파일을 기반으로 데이터를 로드하며, `TextMeshPro`와 긴밀하게 연동됩니다.

## 🎯 주요 기능
- **CSV 데이터 로드:** `Resources` 폴더 내의 CSV 파일을 파싱하여 번역 데이터를 관리합니다.
- **실시간 언어 변경:** 게임 실행 중 언어를 변경하면 등록된 모든 UI 텍스트가 즉시 업데이트됩니다.
- **자동 업데이트 컴포넌트:** `LocalizedText` 컴포넌트를 통해 코드 작성 없이 인스펙터 설정만으로 다국어를 적용할 수 있습니다.
- **에디터 위자드:** 실제 게임을 실행하지 않고도 에디터 상에서 CSV 파싱 결과를 미리 보기 할 수 있습니다.

## 🛠️ 사용 방법

### 1. 데이터 준비 (CSV)
1. `Assets/SWUtility/Resources/Localization/` 경로에 CSV 파일을 생성합니다.
2. 파일 형식은 아래와 같아야 합니다.
   - 첫 번째 줄: `Key,KO,EN,JA` (헤더)
   - 데이터: `UI_START,시작,Start,開始`
3. 파일 이름이 `LocalizationData`가 아닐 경우, `LocalizationManager` 인스펙터에서 `Csv Path`를 수정해야 합니다.

### 2. UI에 다국어 적용
1. `TextMeshProUGUI` 컴포넌트가 있는 오브젝트에 `LocalizedText.cs` 컴포넌트를 추가합니다.
2. `Localization Key` 필드에 CSV에 정의한 키 값(예: `UI_START`)을 입력합니다.
3. 게임 실행 시 또는 언어 변경 시 해당 키에 맞는 번역문으로 자동 변경됩니다.

### 3. 코드에서 사용
스크립트에서 직접 번역된 문자열을 가져오거나 언어를 변경할 수 있습니다.

```csharp
// 번역된 문자열 가져오기
string welcomeMsg = LocalizationManager.Instance.GetLocalizedString("MSG_WELCOME");

// 런타임에 언어 변경 (이후 모든 LocalizedText가 자동 갱신됨)
LocalizationManager.Instance.CurrentLanguage = Language.EN;
```

## 🔍 에디터 위자드 사용법
1. 상단 메뉴에서 `SWUtility > Localization > Parsing Wizard`를 선택합니다.
2. `Csv File` 슬롯에 테스트하고 싶은 CSV 파일을 드래그합니다.
3. `Parse & View` 버튼을 클릭하여 파싱 결과가 올바른지 확인합니다.

## ⚠️ 주의 사항
- CSV 파싱 시 쉼표(,)가 포함된 문장은 반드시 큰따옴표("")로 감싸야 합니다. (예: `"안녕, 하세요"`)
- 지원하는 언어 종류를 변경하려면 `LocalizationEnums.cs`의 `Language` 열거형을 수정해야 합니다.

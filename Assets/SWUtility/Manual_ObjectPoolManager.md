# Manual: ObjectPoolManager

`ObjectPoolManager`는 잦은 생성(`Instantiate`)과 파괴(`Destroy`)로 인한 메모리 가비지(Garbage) 발생과 프레임 드랍을 막아주는 시스템입니다.

## 🎯 주요 기능
- **이름 기반 딕셔너리 관리:** 프리팹의 이름을 키(Key)로 사용하여 큐(Queue) 자료구조로 관리합니다.
- **제네릭(Generic) 컴포넌트 반환:** 스폰 시 `GetComponent`를 내부적으로 처리하여 코드를 깔끔하게 만들어줍니다.
- **자동 하이어라키 정리:** 카테고리별로 씬 계층(Hierarchy)을 폴더처럼 깔끔하게 정리해 줍니다.
- **자동 반환(지연 반환):** 사운드나 이펙트처럼 일정 시간 뒤에 사라져야 하는 오브젝트를 쉽게 처리합니다.

## 🛠️ 사용 방법

### 1. 사전 준비
어떤 프리팹이든 풀링의 대상이 될 수 있습니다. **프리팹에 특별한 스크립트를 미리 붙일 필요가 없습니다.** (매니저가 스폰할 때 `PooledObject`라는 꼬리표를 알아서 붙여 관리합니다.)

### 2. 스폰 (Spawn)
프리팹 참조를 매니저에 넘겨주기만 하면 됩니다.

```csharp
public GameObject bulletPrefab;
public ParticleSystem hitEffectPrefab;

void Shoot()
{
    // 기본 게임오브젝트 스폰 (총알)
    GameObject bullet = ObjectPoolManager.Instance.Spawn(bulletPrefab, firePoint.position, firePoint.rotation);
    
    // 컴포넌트 바로 받기 + 카테고리 지정 (이펙트)
    // 하이어라키 창에 "[VFX]" 라는 부모 아래에 생성됩니다.
    ParticleSystem effect = ObjectPoolManager.Instance.Spawn<ParticleSystem>(hitEffectPrefab, hitPoint, Quaternion.identity, "VFX");
}
```

### 3. 반환 (Despawn)
오브젝트의 사용이 끝났다면 `Destroy` 대신 `Despawn`을 호출하세요.

```csharp
// 즉시 풀로 돌려보내기
ObjectPoolManager.Instance.Despawn(bullet);

// 2초 뒤에 알아서 풀로 돌려보내기 (파티클 재생이 끝날 때 유용)
ObjectPoolManager.Instance.Despawn(effect.gameObject, 2.0f);
```

### 4. 사전 생성 (Pre-Warm)
게임 렉을 방지하기 위해 로딩 화면 등에서 오브젝트를 미리 왕창 만들어둘 수 있습니다.

```csharp
ObjectPoolManager.Instance.PreWarm(bulletPrefab, 50, "Bullets");
```

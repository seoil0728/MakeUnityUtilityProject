using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : GlobalSingleton<ObjectPoolManager>
{
    // 풀 데이터 관리용 딕셔너리 (Key: 프리팹 이름)
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    
    // 하이어라키 정리용 Transform 딕셔너리
    private Dictionary<string, Transform> categoryRoots = new Dictionary<string, Transform>(); // 예: [VFX], [Sound]
    private Dictionary<string, Transform> poolRoots = new Dictionary<string, Transform>();     // 예: LaserHit, Footstep

    // ==========================================
    // --- 핵심 API 1 : 스폰 (Spawn) ---
    // ==========================================
    
    /// <summary>
    /// 프리팹을 스폰합니다. 컴포넌트 타입을 직접 반환받아 GetComponent를 생략할 수 있습니다.
    /// </summary>
    /// <param name="prefab">스폰할 원본 프리팹</param>
    /// <param name="position">위치</param>
    /// <param name="rotation">회전</param>
    /// <param name="category">하이어라키에 묶일 카테고리 폴더명 (예: "VFX", "Sounds")</param>
    public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, string category = "Default") where T : Component
    {
        GameObject obj = Spawn(prefab.gameObject, position, rotation, category);
        return obj.GetComponent<T>();
    }

    /// <summary>
    /// 프리팹(GameObject)을 스폰합니다.
    /// </summary>
    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, string category = "Default")
    {
        string poolKey = prefab.name;

        // 1. 해당 프리팹의 풀이 없다면 새로 생성
        if (!poolDictionary.ContainsKey(poolKey))
        {
            CreatePool(prefab, category);
        }

        GameObject spawnObj = null;

        // 2. 큐에 여유분이 있으면 꺼내고, 없으면 새로 인스턴스화하여 부족한 풀을 늘림
        if (poolDictionary[poolKey].Count > 0)
        {
            spawnObj = poolDictionary[poolKey].Dequeue();
        }
        else
        {
            spawnObj = InstantiateNewObject(prefab, poolKey, category);
        }

        // 3. 위치 설정 및 활성화
        spawnObj.transform.position = position;
        spawnObj.transform.rotation = rotation;
        
        // 하이어라키에서 깔끔하게 보이도록 부모 종속성 해제 (카테고리 폴더 밖으로 꺼냄)
        spawnObj.transform.SetParent(null); 
        spawnObj.SetActive(true);

        return spawnObj;
    }

    // ==========================================
    // --- 핵심 API 2 : 반환 (Despawn) ---
    // ==========================================
    
    /// <summary>
    /// 오브젝트를 즉시 풀로 반환합니다.
    /// </summary>
    public void Despawn(GameObject obj)
    {
        PooledObject pooledObj = obj.GetComponent<PooledObject>();

        if (pooledObj != null && poolDictionary.ContainsKey(pooledObj.poolKey))
        {
            obj.SetActive(false);
            // 원래 있던 하이어라키 폴더(poolRoot)로 다시 집어넣어 정리
            obj.transform.SetParent(poolRoots[pooledObj.poolKey]); 
            poolDictionary[pooledObj.poolKey].Enqueue(obj);
        }
        else
        {
            // 풀링된 객체가 아니면 그냥 파괴
            Destroy(obj); 
        }
    }

    /// <summary>
    /// 지정된 시간(초) 후에 오브젝트를 자동으로 반환합니다. (VFX, 사운드에 유용)
    /// </summary>
    public void Despawn(GameObject obj, float delay)
    {
        StartCoroutine(DespawnRoutine(obj, delay));
    }

    private IEnumerator DespawnRoutine(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Despawn(obj);
    }

    // ==========================================
    // --- 내부 관리 로직 (하이어라키 정리 및 인스턴스화) ---
    // ==========================================
    
    private void CreatePool(GameObject prefab, string category)
    {
        string poolKey = prefab.name;
        poolDictionary.Add(poolKey, new Queue<GameObject>());

        // 카테고리 루트 확인 및 생성 (예: "[VFX]")
        if (!categoryRoots.ContainsKey(category))
        {
            GameObject catObj = new GameObject($"[{category}]");
            catObj.transform.SetParent(transform);
            categoryRoots.Add(category, catObj.transform);
        }

        // 개별 프리팹 풀 루트 생성 (예: "LaserHit_Pool")
        GameObject poolObj = new GameObject($"{poolKey}_Pool");
        poolObj.transform.SetParent(categoryRoots[category]);
        poolRoots.Add(poolKey, poolObj.transform);
    }

    private GameObject InstantiateNewObject(GameObject prefab, string poolKey, string category)
    {
        GameObject newObj = Instantiate(prefab);
        newObj.name = poolKey; // "Prefab(Clone)" 대신 원본 이름 유지

        // 풀링 꼬리표 부착
        PooledObject pooledObj = newObj.AddComponent<PooledObject>();
        pooledObj.poolKey = poolKey;

        // 생성 직후에는 풀 폴더 안에 비활성화 상태로 둠
        newObj.transform.SetParent(poolRoots[poolKey]);
        newObj.SetActive(false);

        return newObj;
    }

    // ==========================================
    // --- (선택적) 미리 로드 (Pre-Warm) ---
    // ==========================================
    
    /// <summary>
    /// 게임 시작 시 렉을 방지하기 위해 풀을 미리 일정 개수만큼 채워둡니다.
    /// </summary>
    public void PreWarm(GameObject prefab, int count, string category = "Default")
    {
        string poolKey = prefab.name;
        if (!poolDictionary.ContainsKey(poolKey)) CreatePool(prefab, category);

        for (int i = 0; i < count; i++)
        {
            GameObject newObj = InstantiateNewObject(prefab, poolKey, category);
            poolDictionary[poolKey].Enqueue(newObj);
        }
    }
}
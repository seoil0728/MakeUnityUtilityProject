using UnityEngine;

/// <summary>
/// ObjectPoolManager에 의해 생성된 오브젝트에 자동으로 붙는 꼬리표입니다.
/// </summary>
public class PooledObject : MonoBehaviour
{
    // 자신이 속한 풀의 고유 키 (보통 프리팹 이름)
    public string poolKey;
    
    // 코루틴을 통한 자동 반환용 변수
    private Coroutine autoDespawnCoroutine;
}
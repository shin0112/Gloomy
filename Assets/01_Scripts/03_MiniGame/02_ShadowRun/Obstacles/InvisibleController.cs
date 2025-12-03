using UnityEngine;

public class InvisibleController : MonoBehaviour
{
    #region 필드
    [Header("이동 세팅")]
    [Range(3f, 6f)][SerializeField] private float _moveSpeedX = 3f;
    [Range(3f, 6f)][SerializeField] private float _moveSpeedZ = 5f;
    [SerializeField] private bool _xTrackingEnabled = true;
    [SerializeField] private bool zBackwardRestrictio = true;

    [Header("값 확인용")]
    [SerializeField] private Transform _curTarget;

    // 정보 캐싱
    private ObstacleManager.ObstacleInfo _invisibleInfo;
    private float _roadWidth;

    // 컴포넌트
    private Rigidbody _rigidbody;
    #endregion

    #region 초기화

    private void Awake()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>(true);
    }

    /// <summary>
    /// [public] 해당 오브젝트 생성 시 정보 초기화
    /// </summary>
    /// <param name="invisibleInfo"></param>
    public void InitInfo(ObstacleManager.ObstacleInfo invisibleInfo, float roadWidth)
    {
        _invisibleInfo = invisibleInfo;
        _roadWidth = roadWidth;
    }

    private void Start()
    {
        // 타겟 탐색 및 지정
        _curTarget = FindObjectOfType<PlayerController>().transform;
    }
    #endregion

    #region Unity API
    private void FixedUpdate()
    {
        Move();
    }
    #endregion

    #region 이동
    /// <summary>
    /// 타겟과 자신의 위치를 비교한 후 속도 비율을 맞춰서 이동
    /// </summary>
    private void Move()
    {
        Vector3 delta = _curTarget.position - transform.position;

        if (!_xTrackingEnabled)     // x축 추적이 불가능할 때
        {
            delta.x = 0f;
        }

        // 1) 축별 속도 먼저 반영
        float scaledX = delta.x * _moveSpeedX;
        float scaledZ = delta.z * _moveSpeedZ;

        Vector3 scaled = new Vector3(scaledX, 0, scaledZ);

        // 2) 속도차 반영 후 방향 정규화
        Vector3 dir = scaled.normalized;

        // 3) 전체 속도 크기 결정
        float maxSpeed = Mathf.Max(_moveSpeedX, _moveSpeedZ);

        Vector3 velocity = dir * maxSpeed;

        Vector3 pos = _rigidbody.position + velocity * Time.fixedDeltaTime;

        // 조건 후처리
        pos.x = Mathf.Clamp(pos.x, -_roadWidth / 2, _roadWidth / 2);

        _rigidbody.MovePosition(pos);
    }
    #endregion
}

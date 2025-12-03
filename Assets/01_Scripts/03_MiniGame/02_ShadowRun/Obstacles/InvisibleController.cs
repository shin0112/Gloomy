using UnityEngine;

/// <summary>
/// 투명 인간 움직임 관리
/// </summary>
public class InvisibleController : MonoBehaviour
{
    #region 필드
    [Header("이동 세팅")]
    [Range(3f, 6f)][SerializeField] private float _moveSpeedX = 3f;
    [Range(3f, 6f)][SerializeField] private float _moveSpeedZ = 5f;
    [SerializeField] private bool _xTrackingEnabled = true;
    [SerializeField] private bool _zBackwardRestrictio = true;

    private float _prevZSign;               // z방향 벡터 부호 캐싱용
    private bool _allowBackward = true;     // z방향 움직임 플래그

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
        _rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// [public] 해당 오브젝트 생성 시 정보 초기화
    /// </summary>
    /// <param name="invisibleInfo"></param>
    public void InitInfo(
        ObstacleManager.ObstacleInfo invisibleInfo,
        float roadWidth,
        float moveSpeedX = 3f,
        float moveSpeedZ = 5f,
         bool xTrackingEnabled = true,
         bool zBackwardRestrictio = true
        )
    {
        _invisibleInfo = invisibleInfo;
        _roadWidth = roadWidth;
        _moveSpeedX = moveSpeedX;
        _moveSpeedZ = moveSpeedZ;
        _xTrackingEnabled = xTrackingEnabled;
        _zBackwardRestrictio = zBackwardRestrictio;
    }

    private void Start()
    {
        // 타겟 탐색 및 지정
        _curTarget = FindObjectOfType<PlayerController>().transform;
        _prevZSign = Mathf.Sign((_curTarget.position - transform.position).normalized.z);
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
        // 방향 구하기
        Vector3 dir = (_curTarget.position - transform.position).normalized;

        // z 벡터의 부호가 변경되는지 = 플레이어가 통과했는지
        float zSign = Mathf.Sign(dir.z);
        if (_prevZSign != zSign)
        {
            Logger.Log("투명인간 장애물 통과");
            _allowBackward = false;
        }
        _prevZSign = zSign; // 캐싱

        if (!_xTrackingEnabled)         // x축 추적이 불가능할 때
        {
            dir.x = 0f;
        }

        if (!_allowBackward)     // z축 방향으로 이동해야하기 때문
        {
            dir.z = -1f;
        }

        // 속도 보정
        float velocityX = dir.x * _moveSpeedX;
        float velocityZ = dir.z * _moveSpeedZ;

        // 점프는 없다고 가정
        // 조건 후처리
        Vector3 pos = transform.position + new Vector3(velocityX, 0f, velocityZ) * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -_roadWidth / 2, _roadWidth / 2);
        _rigidbody.MovePosition(pos);
    }
    #endregion
}

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
        _rigidbody = GetComponent<Rigidbody>();
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
        // 방향 구하기
        Vector3 dir = (_curTarget.position - transform.position).normalized;

        if (!_xTrackingEnabled)     // x축 추적이 불가능할 때
        {
            dir.x = 0f;
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

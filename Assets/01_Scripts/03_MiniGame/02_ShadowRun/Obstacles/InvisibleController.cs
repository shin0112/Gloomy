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

        // 비율 계산 (거리 기반)
        float ratioX = Mathf.Abs(delta.x);
        float ratioZ = Mathf.Abs(delta.z);

        float sum = ratioX + ratioZ;
        if (sum < Mathf.Epsilon) return;

        ratioX /= sum;
        ratioZ /= sum;

        // 부호 적용
        float sx = Mathf.Sign(delta.x);
        float sz = Mathf.Sign(delta.z);

        // 축별 속도 반영
        float velocityX = ratioX * _moveSpeedX * sx;
        float velocityZ = ratioZ * _moveSpeedZ * sz;

        Vector3 velocity = new Vector3(velocityX, 0f, velocityZ);

        Vector3 pos = _rigidbody.position + velocity * Time.fixedDeltaTime;

        // 조건 후처리
        pos.x = Mathf.Clamp(pos.x, -_roadWidth / 2, _roadWidth / 2);

        _rigidbody.MovePosition(pos);
    }
    #endregion
}

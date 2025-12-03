using UnityEngine;

public class InvisibleController : MonoBehaviour
{
    [Header("이동 세팅")]
    [Range(3f, 6f)][SerializeField] private float _moveSpeedX = 3f;
    [Range(3f, 6f)][SerializeField] private float _moveSpeedZ = 5f;
    [SerializeField] private bool _xTrackingEnabled = true;
    [SerializeField] private bool zBackwardRestrictio = true;

    [Header("값 확인용")]
    [SerializeField] private Transform _curTarget;

    // 정보 캐싱
    private ObstacleManager.ObstacleInfo _invisibleInfo;

    // 컴포넌트
    private Rigidbody _rigidbody;

    #region 초기화
    /// <summary>
    /// [public] 해당 오브젝트 생성 시 정보 초기화
    /// </summary>
    /// <param name="invisibleInfo"></param>
    public void InitInfo(ObstacleManager.ObstacleInfo invisibleInfo)
    {
        _invisibleInfo = invisibleInfo;
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
        // 속도에 따른 이동 비율 구하기
        float xRatio = (_curTarget.position.x - transform.position.x) * _moveSpeedX;
        float zRatio = (_curTarget.position.z - transform.position.z) * _moveSpeedZ;

        if (!_xTrackingEnabled)
        {
            xRatio = 0f;
        }

        if (!zBackwardRestrictio)
        {
            zRatio = Mathf.Min(0f, zRatio);
        }

        // 점프는 없다고 가정
        Vector3 direction = new Vector3(xRatio, 0, zRatio).normalized;
        // 비율을 맞추기 위해 더 큰 속도 값으로 보정
        direction *= Mathf.Max(_moveSpeedZ, _moveSpeedX);
        _rigidbody.velocity = direction;

        // 조건 후처리
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, _invisibleInfo.xRange.x, _invisibleInfo.xRange.y);

        transform.position = pos;
    }
    #endregion
}

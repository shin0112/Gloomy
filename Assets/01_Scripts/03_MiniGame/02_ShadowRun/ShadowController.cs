using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 그림자 생성 및 플레이어 추적 로직 
/// </summary>
public class ShadowController : MonoBehaviour
{
    // 플레이어가 이동하다가 아마 매니저 측에서 shadowcontroller를 instantiate
    // 그러면 그 오브젝트를 받아서 바로 init할 수 있음
    [Header("타겟 정보")]
    [SerializeField] private Transform _curTarget;
    [field: SerializeField] public float Distance { get; private set; }
    [field: SerializeField] public bool HasCaughtTarget { get; private set; }

    [Header("움직임 세팅")]
    // 이동
    [SerializeField] private float _speed = 6f;               // 기본 움직임. 플레이어와 동일
    [SerializeField] private float _speedModifier = 1f;       // 플레이어가 대시를 썼을 때 가속도를 위함
    [SerializeField] private float _roadWidth = 10f;

    // 회전
    private float _rotateDamping = 1f;

    // 연출
    [Header("그림자 추적 세팅")]
    [SerializeField] private float _vignetteTriggerDistance = 1f;

    private Camera _camera;
    private Vignette _vignette;
    private Rigidbody _rigidbody;

    #region 초기화
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ConnectPlayer();
        _camera = Camera.main;
        _camera.GetComponent<Volume>().profile.TryGet(out _vignette);
    }

    /// <summary>
    /// [public] 그림자가 따라갈 플레이어를 연동
    /// </summary>
    public void ConnectPlayer()
    {
        if (_curTarget == null)
        {
            _curTarget = FindObjectOfType<PlayerController>().transform;
        }

        // 추가 세팅들
        _speedModifier = 1f;
    }
    #endregion

    private void Update()
    {
        if (_curTarget == null) return;

        CalcDistance();
        ManageVignette();
    }

    private void FixedUpdate()
    {
        Move();
    }

    #region 움직임 구현
    private void Move()
    {
        Vector3 direction = (_curTarget.transform.position - this.transform.position).normalized;
        Rotate(direction);
        Move(direction);
    }

    private void Move(Vector3 direction)
    {
        Vector3 shadowPos = this.transform.position;
        direction = direction * _speed * _speedModifier;

        _rigidbody.velocity = direction;

        float maxX = _roadWidth / 2;

        // -maxX <= shadowPos.x <= maxX
        shadowPos.x = Mathf.Min(shadowPos.x, maxX);
        shadowPos.x = Mathf.Max(shadowPos.x, -maxX);

        shadowPos.y = this.transform.position.y;

        this.transform.position = shadowPos;
    }

    /// <summary>
    /// 방향대로 회전하기
    /// </summary>
    /// <param name="direction"></param>
    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Transform shadowTransform = this.transform;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            shadowTransform.rotation = Quaternion.Slerp(
                shadowTransform.rotation,
                targetRotation,
                _rotateDamping * Time.deltaTime);
        }
    }

    /// <summary>
    /// 플레이어와 거리 측정
    /// </summary>
    private void CalcDistance()
    {
        float distance = Vector3.Distance(_curTarget.position, this.transform.position);
        Distance = Mathf.Max(distance, 1);

        HasCaughtTarget = Distance == 1f;
    }
    #endregion

    #region 효과
    private float _vigVelocity;

    /// <summary>
    /// 플레이어와의 거리를 측정해서 비네틱 효과 주기
    /// </summary>
    private void ManageVignette()
    {
        float value = (_vignetteTriggerDistance + 1f) - Distance;
        value = Mathf.Max(value, 0f);

        float target = Mathf.Lerp(0f, 1f, value);

        _vignette.intensity.value = Mathf.SmoothDamp(
            _vignette.intensity.value,
            target,
            ref _vigVelocity,
            0.08f
        );
    }
    #endregion
}

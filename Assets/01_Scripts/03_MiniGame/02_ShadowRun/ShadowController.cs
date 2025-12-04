using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 그림자 생성 및 플레이어 추적 로직 
/// </summary>
public class ShadowController : MonoBehaviour
{
    #region 필드
    // 테스트
    [field: Header("테스트")]
    [field: SerializeField] public bool IsTest { get; set; }

    [Header("타겟 정보")]
    [Tooltip("그림자가 따라가는 타겟")]
    [SerializeField] private Transform _curTarget;
    [field: SerializeField] public float Distance { get; private set; }
    [field: SerializeField] public bool HasCaughtTarget { get; private set; }
    public event Action OnCaughtTarget;
    public event Action OnEscapeTarget;

    // 캐싱
    private Vector3 _prevTargetPos;             // modifier 계산
    private bool _doneCaughtDirection = false;  // 잡힘 연출

    [Header("게임 세팅")]
    [Tooltip("추격 게임이 시작한 후 그림자가 출발을 기다리는 시간(초)")]
    [Range(1f, 10f)][SerializeField] private float _startDelay = 3f;
    [Tooltip("플레이어를 잡았다고 처리되는 시작 간격")]
    [SerializeField] private float _caughtDistance = 0.05f;
    private float _timer;

    [Header("그림자 움직임")]
    [Tooltip("기본 움직임. 플레이어와 동일")]
    [SerializeField] private float _speed = 6f;
    [Tooltip("그림자 움직임 보정값")]
    [SerializeField] private float _speedModifier = 1f;
    [Tooltip("플레이어가 대각선 이동할 때 그림자 속도 감속 강도")]
    [Range(1f, 5f)][SerializeField] private float _diagonalSlowFactor = 3.0f;
    [Tooltip("그림자가 이동할 수 있는 너비 (도로 너비와 동일)")]

    [Range(0f, 10f)][SerializeField] private float _roadWidth = 10f;
    // 회전
    private float _rotateDamping = 1f;

    // 연출
    [Header("비네팅 연출 세팅")]
    [Tooltip("비네팅 효과가 시작되는 거리 (기본값: 1m)")]
    [Range(0f, 2f)][SerializeField] private float _vignetteTriggerDistance = 1f;
    [Tooltip("비네팅 효과 강도 (기본값: 1)")]
    [Range(0f, 1f)][SerializeField] private float _vigetteIntensity = 1f;

    // 컴포넌트
    private Camera _camera;
    private Vignette _vignette;
    private Rigidbody _rigidbody;
    #endregion

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
    /// 그림자가 따라갈 플레이어(타겟)를 연동
    /// </summary>
    private void ConnectPlayer()
    {
        if (_curTarget == null)
        {
            _curTarget = FindObjectOfType<PlayerController>().transform;
        }

        // todo: 추가 세팅들

        _prevTargetPos = _curTarget.transform.position;
    }
    #endregion

    private void Update()
    {
        CalcDistance();
        ManageVignette();
    }

    private void FixedUpdate()
    {
        if (IsTest) return;

        if (_timer < _startDelay)
        {
            _timer += Time.deltaTime;
            return;
        }

        CalcSpeedModifier();
        MoveAndRot();
    }

    #region 움직임 구현
    /// <summary>
    /// 이동 및 회전 
    /// </summary>
    private void MoveAndRot()
    {
        Vector3 direction = (_curTarget.transform.position - this.transform.position).normalized;
        Rotate(direction);
        Move(direction);
    }

    /// <summary>
    /// 방향대로 움직이기
    /// </summary>
    /// <param name="direction"></param>
    private void Move(Vector3 direction)
    {
        direction = direction * _speed * _speedModifier;
        _rigidbody.velocity = direction;

        // 좌표 조건 처리
        Vector3 shadowPos = this.transform.position;

        // 1) x 좌표: -maxX <= shadowPos.x <= maxX
        float maxX = _roadWidth / 2;
        shadowPos.x = Mathf.Clamp(shadowPos.x, -maxX, maxX);

        // 2) y 좌표: 유지
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
    /// 플레이어와 거리 측정. 
    /// 연출 여부 & 잡힌 상태를 보고 UI 이벤트 컨트롤
    /// </summary>
    private void CalcDistance()
    {
        float nowDistance = Vector3.Distance(_curTarget.position, this.transform.position);
        // 오브젝트 부피로 인한 값(1f) 감산
        Distance = Mathf.Max(nowDistance - 1, 0);

        HasCaughtTarget = Distance < _caughtDistance;

        if (!_doneCaughtDirection && HasCaughtTarget)   // 잡힌 연출 x & 잡혔을 때
        {
            OnCaughtTarget?.Invoke();
            _doneCaughtDirection = true;
        }

        if (_doneCaughtDirection && !HasCaughtTarget)   // 잡힌 연출 o & 벗어났을 때
        {
            OnEscapeTarget?.Invoke();
            _doneCaughtDirection = false;
        }
    }

    /// <summary>
    /// 그림자 이동 속도 보정
    /// </summary>
    private void CalcSpeedModifier()
    {
        // 이동량 계산
        Vector3 targetDelta = _curTarget.position - _prevTargetPos;
        float deltaX = Mathf.Abs(targetDelta.x);
        deltaX = deltaX > 0.001f ? deltaX : 0;

        // 반비례 감속
        float modifier = 1f / (1f + _diagonalSlowFactor * deltaX);

        // 최소 속도 제한
        modifier = Mathf.Clamp(modifier, 0.7f, 1f);
        _speedModifier = modifier;

        // 캐싱
        _prevTargetPos = _curTarget.position;
    }
    #endregion

    #region 효과
    private float _vigVelocity;

    /// <summary>
    /// 플레이어와의 거리를 측정해서 비네틱 효과 주기
    /// </summary>
    private void ManageVignette()
    {
        // 비네팅 범위 거리 체크
        float raw = _vignetteTriggerDistance - Distance;
        raw = Mathf.Max(raw, 0f) / _vignetteTriggerDistance * _vigetteIntensity;

        float target = Mathf.Lerp(0f, _vigetteIntensity, raw);

        _vignette.intensity.value = Mathf.SmoothDamp(
            _vignette.intensity.value,
            target,
            ref _vigVelocity,
            0.08f
        );
    }
    #endregion
}

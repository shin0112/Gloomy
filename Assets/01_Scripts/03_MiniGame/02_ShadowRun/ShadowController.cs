using UnityEngine;

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

    // 움직임 필드 
    [Header("움직임 세팅")]
    [SerializeField] private float _speed = 5f;               // 기본 움직임. 플레이어와 동일
    [SerializeField] private float _speedModifier = 1f;       // 플레이어가 대시를 썼을 때 가속도를 위함
    [SerializeField] private Vector3 _movementDirection;

    private float _rotateDamping = 1f;

    private void Update()
    {
        if (_curTarget == null) return;

        CalcDistance();
        Move();
    }

    /// <summary>
    /// [public] 그림자가 따라갈 플레이어를 연동
    /// </summary>
    public void ConnectPlayer()
    {
        if (_curTarget == null) return;

        // todo: 플레이어 정보 초기화할 때 받기
        GameObject player = new();
        _curTarget = player.transform;

        // 플레이어의 속도 복사
        //_speed = ...
        _speedModifier = 1f;
    }

    /// <summary>
    /// 플레이어와 거리 측정
    /// </summary>
    private void CalcDistance()
    {
        Distance = Vector3.Distance(_curTarget.position, this.transform.position);
    }

    #region 움직임 구현
    private void Move()
    {
        _movementDirection = _curTarget.transform.position - this.transform.position;
        Rotate(_movementDirection);
        Move(_movementDirection);
    }

    private void Move(Vector3 direction)
    {
        Transform shadowTransform = this.transform;
        //direction * _speed * 
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
    #endregion
}

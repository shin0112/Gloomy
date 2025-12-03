using System.Collections;
using UnityEngine;

/// <summary>
/// 투명 인간 스폰 관리
/// </summary>
public class InvisibleSpawnController : MonoBehaviour
{
    #region 필드
    [Header("생성 세팅")]
    [SerializeField] private Transform _root;
    [SerializeField] private GameObject _invisible;
    [Tooltip("한 스테이지에서 등장하는 투명 인간의 생성 횟수")]
    [SerializeField] private int _spawnCount = 3;
    [Tooltip("X축 랜덤 생성 범위(m)")]
    [SerializeField] private Vector2 _xRange;
    [Tooltip("도로 전체 폭. X축 이동 가능 범위")]
    [SerializeField] private float _roadWitdh = 10f;
    [Tooltip("투명 인간 생성 간 Z축 최소 거리 간격(m)")]
    [SerializeField] private float _minSpawnInterval = 100f;
    [Tooltip("플레이어 기준 투명 인간 생성 제한 구간(m)")]
    [SerializeField] private float _spawnStartOffsetZ = 30f;
    [Tooltip("투명 인간 생성 z 시작 지점(m)")]
    [SerializeField] private float _spawnZRangeStart = 500f;
    [Tooltip("투명 인간 생성 y 시작 지점(m)")]
    [SerializeField] private float _spawnYRangeStart = 0f;

    [Header("이동 세팅 - 오버라이드 할 경우에만 적용")]
    [SerializeField] private bool _isOverrideMovementSetting = false;
    [Range(3f, 6f)][SerializeField] private float _moveSpeedX = 3f;
    [Range(3f, 6f)][SerializeField] private float _moveSpeedZ = 5f;
    [SerializeField] private bool _xTrackingEnabled = true;
    [SerializeField] private bool _zBackwardRestrictio = true;

    private bool _doneSpawn = false;
    // 코루틴
    private Coroutine _spawn;
    #endregion

    private void OnDisable()
    {
        if (_spawn != null)
        {
            StopCoroutine(_spawn);
            _spawn = null;
        }
    }

    /// <summary>
    /// 플레이어가 500M 콜라이더에 들어올 시 투명 인간 소환 코루틴 수행
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!_doneSpawn && other.TryGetComponent<PlayerController>(out var player))
        {
            Logger.Log("투명 인간 스폰 시작");
            _spawn = StartCoroutine(nameof(SpawnInvisible));
        }
    }

    /// <summary>
    /// 스폰 횟수만큼 투명 인간 소환
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnInvisible()
    {
        int count = 0;
        float x;
        float y;
        float z = _spawnZRangeStart + Random.Range(0, _spawnStartOffsetZ);

        do
        {
            yield return null;

            ObstacleManager.ObstacleInfo info = new()
            {
                prefab = _invisible,
                xRange = new Vector2(_xRange.x, _xRange.y),
                startZ = _spawnZRangeStart,
                spacing = _minSpawnInterval,
                y = _spawnYRangeStart
            };

            // 투명인간 생성
            GameObject invisibleObj = Instantiate(_invisible, _root.transform);
            var invisible = invisibleObj.GetComponent<InvisibleController>();

            if (_isOverrideMovementSetting)     // 오버라이딩할 경우
            {
                invisible.InitInfo(
                    info,
                    _roadWitdh,
                    _moveSpeedX,
                    _moveSpeedZ,
                    _xTrackingEnabled,
                    _zBackwardRestrictio);
            }
            else
            {
                invisible.InitInfo(info, _roadWitdh);
            }

            // 투명인간 위치 지정
            x = Random.Range(_xRange.x, _xRange.y);
            y = 0f;
            z += _spawnStartOffsetZ + Random.Range(0, _spawnStartOffsetZ);

            invisible.transform.position = new(x, y, z);
            count++;

            Logger.Log($"투명인간 스폰 {count}");
            _doneSpawn = true;
        } while (count < _spawnCount);
    }
}

using System.Collections;
using UnityEngine;

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

    private void OnTriggerEnter(Collider other)
    {
        if (!_doneSpawn && other.TryGetComponent<PlayerController>(out var player))
        {
            Logger.Log("투명 인간 스폰 시작");
            _spawn = StartCoroutine(nameof(SpawnInvisible));
        }
    }

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
            invisible.InitInfo(info, _roadWitdh);

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

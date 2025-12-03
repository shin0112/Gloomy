using UnityEngine;

public class InvisibleSpawnController : MonoBehaviour
{
    [Header("생성 세팅")]
    [SerializeField] private GameObject _invisible;
    [Tooltip("한 스테이지에서 등장하는 투명 인간의 생성 횟수")]
    [SerializeField] private int _spawnCount = 3;
    [Tooltip("도로 전체 폭. X축 랜덤 생성 범위(m)")]
    [SerializeField] private float _roadWitdh = 10f;
    [Tooltip("투명 인간 생성 간 Z축 최소 거리 간격(m)")]
    [SerializeField] private float _minSpawnInterval = 100f;
    [Tooltip("플레이어 기준 투명 인간 생성 제한 구간(m)")]
    [SerializeField] private float _spawnStartOffsetZ = 30f;
    [Tooltip("투명 인간 생성 z 시작 지점(m)")]
    [SerializeField] private float _spawnZRangeStart = 500f;
    [Tooltip("투명 인간 생성 y 시작 지점(m)")]
    [SerializeField] private float _spawnYRangeStart = 0f;

    private ObstacleManager.ObstacleInfo _invisibleInfo;

    private void Awake()
    {
        _invisibleInfo = new()
        {
            prefab = _invisible,
            xRange = new Vector2(-_roadWitdh / 2, _roadWitdh / 2),
            startZ = _spawnZRangeStart,
            spacing = _minSpawnInterval,
            y = _spawnYRangeStart
        };
    }
}

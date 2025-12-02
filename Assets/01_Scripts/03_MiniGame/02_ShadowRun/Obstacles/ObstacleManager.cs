using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [System.Serializable]
    public class ObstacleInfo
    {
        public GameObject prefab;
        public int maxCount;
        [HideInInspector] public int currentCount;

        // 스폰 범위
        public Vector2 xRange;
        public float y;
        public Vector2 zRange;
    }

    public ObstacleInfo owl;
    public ObstacleInfo rock;
    public ObstacleInfo hurdle;
    public ObstacleInfo invisible;
    public ObstacleInfo star;

    public float interval = 1.2f; // 스폰 간격
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        ObstacleInfo[] list = { owl, rock, hurdle, invisible, star };

        // 생성 가능 항목 필터링
        var available = System.Array.FindAll(list, o => o.currentCount < o.maxCount);
        if (available.Length == 0) return;

        // 랜덤으로 장애물 선택
        ObstacleInfo target = available[Random.Range(0, available.Length)];

        // 랜덤 위치 계산
        float x = Random.Range(target.xRange.x, target.xRange.y);
        float z = Random.Range(target.zRange.x, target.zRange.y);
        Vector3 spawnPos = new Vector3(x, z);

        // 생성
        Instantiate(target.prefab, spawnPos, Quaternion.identity);
        target.currentCount++;
    }
}

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
    public ObstacleInfo hudle;
    public ObstacleInfo invisible;
    public ObstacleInfo star;

    private float timer;

    void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        ObstacleInfo[] list = { owl, rock, hudle, invisible, star };

        var available = System.Array.FindAll(list, o => o.currentCount < o.maxCount);
        if (available.Length == 0) return;

        ObstacleInfo target = available[Random.Range(0, available.Length)];

        float x = Random.Range(target.xRange.x, target.xRange.y);
        float z = Random.Range(target.zRange.x, target.zRange.y); // 충분히 넓게
        Vector3 spawnPos = new Vector3(x, target.y, z);

        Instantiate(target.prefab, spawnPos, Quaternion.identity);
        target.currentCount++;
    }
}
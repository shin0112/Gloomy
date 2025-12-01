using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [System.Serializable]
    public class  ObstacleInfo
    {
        public GameObject prefab;
        public int maxCount;
        public int currentCount;
    }

    public ObstacleInfo owl;
    public ObstacleInfo rock;
    public ObstacleInfo hurdle;
    public ObstacleInfo invisible;
    public ObstacleInfo star;

    public Transform[] spawnPoints;
    public float interval = 1.2f;

    float timer;

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
        ObstacleInfo[]list = {owl, rock, hurdle, invisible, star };
        ObstacleInfo target = null;

        var available = System.Array.FindAll(list, o => o.currentCount < o.maxCount);
        if (available.Length == 0)
            return;
        target = available[Random.Range(0, available.Length)];
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(target.prefab, point.position, point.rotation);
        target.currentCount++;
    }
}

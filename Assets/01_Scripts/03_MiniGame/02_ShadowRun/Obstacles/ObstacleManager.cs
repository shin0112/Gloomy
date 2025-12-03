using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [System.Serializable]
    public class ObstacleInfo
    {
        public GameObject prefab;
        public float y;           // 고정 높이
        public Vector2 xRange;    // X 랜덤 범위
        public float startZ;      // 시작 지점
        public float spacing;     // 최소 간격
    }

    public Transform root;
    public float mapLength = 1000f;

    public ObstacleInfo owl;
    public ObstacleInfo rock;
    public ObstacleInfo hurdle;
    //public ObstacleInfo invisible;
    public ObstacleInfo star;

    void Start()
    {
        SpawnObstacleLine(owl);
        SpawnObstacleLine(rock);
        SpawnObstacleLine(hurdle);
        //SpawnObstacleLine(invisible);
        SpawnStars(); // 별만 특별하게 처리
    }

    // 일반 장애물 라인 스폰
    void SpawnObstacleLine(ObstacleInfo info)
    {
        float z = info.startZ;

        while (z < mapLength)
        {
            float x = Random.Range(info.xRange.x, info.xRange.y);
            Vector3 pos = new Vector3(x, info.y, z);

            if (info == owl && z > 500f)
            {
                z += info.spacing + Random.Range(0f, 3f);
                continue; // 이번 스폰 건너뛰고 다음으로
            }

            GameObject obj = Instantiate(info.prefab, root);
            obj.transform.localPosition = pos;

            z += info.spacing + Random.Range(0f, 3f); // Z 간격 랜덤
        }
    }

    // 별만 3개, 시작/중간/끝
    void SpawnStars()
    {
        float[] zPositions = { 0f, mapLength / 2f, mapLength };

        foreach (float z in zPositions)
        {
            float x = Random.Range(star.xRange.x, star.xRange.y);
            Vector3 pos = new Vector3(x, star.y, z);

            GameObject obj = Instantiate(star.prefab, root);
            obj.transform.localPosition = pos;
        }
    }
}

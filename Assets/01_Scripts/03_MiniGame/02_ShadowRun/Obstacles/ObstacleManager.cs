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
    public ObstacleInfo invisible;
    public ObstacleInfo star;

    void Start()
    {
        SpawnObstacleLine(owl);
        SpawnObstacleLine(rock);
        SpawnObstacleLine(hurdle);
        SpawnObstacleLine(invisible);
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

            GameObject obj = Instantiate(info.prefab, root);
            obj.transform.localPosition = pos;

            z += info.spacing + Random.Range(0f, 3f); // Z 간격 랜덤
        }
    }

    // 별만 3개, 시작/중간/끝
    void SpawnStars()
    {
        float[] zPositions = { 15f, mapLength / 2f, mapLength }; //별의 생성지점, 시작 중간 , 끝지점
        //0f는 시작지점 값, maplength는 /2fm mapLength는 1000M (전체 맵 길이 나누기 2)
        
        foreach (float z in zPositions)
        {
            float x = Random.Range(star.xRange.x, star.xRange.y);
            Vector3 pos = new Vector3(x, star.y, z); 

            GameObject obj = Instantiate(star.prefab, root);
            obj.transform.localPosition = pos;
        }
    }
}

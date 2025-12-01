using UnityEngine;


/// <summary>
/// 시작 시 맵에 나무 스폰하기
/// </summary>
public class SpawnTree : MonoBehaviour
{
    [Header("나무 스폰")]
    [SerializeField] private GameObject _root;
    [SerializeField] private GameObject[] _trees;

    [Tooltip("체크가 해제되어 있을 경우 맵 전체에 나무가 만들어집니다")]
    [SerializeField] private bool _overrideTreeCount = false;
    [Tooltip("최대 나무 줄 개수")]
    [SerializeField] private int _treeRowCount;
    [SerializeField] private float _spacing = 2f;
    [SerializeField] private float _width = 4f;


    private void Reset()
    {
        _root = transform.FindChild<Transform>("Trees").gameObject;
    }

    private void Awake()
    {
        SpawnTrees();
    }

    private void SpawnTrees()
    {
        // root 좌표 가져오기
        float totalx = _root.transform.localScale.x;
        float z = _root.transform.localScale.z + 4f; // 

        if (!_overrideTreeCount)
        {
            _treeRowCount = (int)(Define.MapLength / Define.TreeLength) - 1;
        }

        // 나무 랜덤으로 하면 교체
        for (int i = 0; i < _treeRowCount; i++)
        {
            GameObject rightTree = Instantiate(_trees[0], _root.transform);
            rightTree.transform.localPosition = new Vector3(
                -(i * Define.TreeLength + _spacing * i),
                -0.5f,
                _width);

            GameObject leftTree = Instantiate(_trees[0], _root.transform);
            leftTree.transform.localPosition = new Vector3(
                -(i * Define.TreeLength + _spacing * i),
                -0.5f,
                -_width);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 그림자 추격에서 비네틱 효과를 보정하기 위한 이미지 효과
/// </summary>
public class CloseEffect : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private ShadowController _shadow;
    [SerializeField] private float _closeSpeed = 5f;

    #region 초기화
    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        _shadow = FindObjectOfType<ShadowController>();
    }
    #endregion

    private void Update()
    {
        ChangeColorAlpha();
    }

    /// <summary>
    /// 그림자 잡힘 여부에 따라 이미지의 알파값을 조정
    /// </summary>
    private void ChangeColorAlpha()
    {
        Color color = _image.color;

        float delta = Time.deltaTime * _closeSpeed;

        if (_shadow.HasCaughtTarget)
            color.a = Mathf.Min(color.a + delta, 1f);
        else
            color.a = Mathf.Max(color.a - delta, 0f);

        _image.color = color;
    }
}

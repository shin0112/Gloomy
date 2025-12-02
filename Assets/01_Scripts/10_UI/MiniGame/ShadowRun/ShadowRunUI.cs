using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 그림자 추격 시 전체 UI
/// </summary>
public class ShadowRunUI : MonoBehaviour
{
    #region 필드
    // 외부 컴포넌트
    [SerializeField] private ShadowController _shadow;

    // UI 컴포넌트
    [Header("비네틱 효과")]
    [SerializeField] private Image _closeEffect;
    [SerializeField] private float _closeSpeed = 5f;

    [Header("그림자 거리")]
    [SerializeField] private TextMeshProUGUI _distanceText;

    [Header("감정 조각")]
    [SerializeField] private Image _mindPiece;
    #endregion

    #region 초기화
    private void Reset()
    {
        _closeEffect = transform.FindChild<Image>("Image- CloseEffect");
        _distanceText = transform.FindChild<TextMeshProUGUI>("Text - Distance");
        _mindPiece = transform.FindChild<Image>("Image - MindPiece");
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
        Color color = _closeEffect.color;

        float delta = Time.deltaTime * _closeSpeed;

        if (_shadow.HasCaughtTarget)
            color.a = Mathf.Min(color.a + delta, 1f);
        else
            color.a = Mathf.Max(color.a - delta, 0f);

        _closeEffect.color = color;
    }
}

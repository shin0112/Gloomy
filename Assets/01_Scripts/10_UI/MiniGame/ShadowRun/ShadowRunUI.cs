using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [Header("테스트용")]
    [SerializeField] private Button _testReLoadButton;
    [SerializeField] private Button _testStartButton;
    [SerializeField] private Button _testInvisibleButton;
    #endregion

    #region Unity API
    private void Reset()
    {
        _closeEffect = transform.FindChild<Image>("Image- CloseEffect");
        _distanceText = transform.FindChild<TextMeshProUGUI>("Text - Distance");
        _mindPiece = transform.FindChild<Image>("Image - MindPiece");
        _testReLoadButton = transform.FindChild<Button>("Button - ReLoad");
        _testStartButton = transform.FindChild<Button>("Button - Start");
        _testInvisibleButton = transform.FindChild<Button>("Button - Invisible");
    }

    private void Start()
    {
        _shadow = FindObjectOfType<ShadowController>();
    }

    private void OnEnable()
    {
        _distanceText.text = "0.00M";
        _mindPiece.gameObject.SetActive(false);
        _testReLoadButton.onClick.AddListener(OnClickTestReloadButton);
        _testStartButton.onClick.AddListener(OnClickTestStartButton);
        _testInvisibleButton.onClick.AddListener(OnClickTestInvisibleButton);
    }

    private void Update()
    {
        ChangeColorAlpha();
        UpdateDistanceText();
    }

    private void OnDisable()
    {
        _testInvisibleButton.onClick.RemoveAllListeners();
    }
    #endregion

    #region 비네팅 연출
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
    #endregion

    #region 게임 정보 UI
    /// <summary>
    /// 그림자 거리 텍스트 변경하기
    /// </summary>
    private void UpdateDistanceText()
    {
        if (_shadow.HasCaughtTarget)
        {
            _distanceText.text = "0.00M";
            return;
        }

        _distanceText.text = $"{_shadow.Distance:0.00}M";
    }

    /// <summary>
    /// [public] 감정 조각 획득 시 이미지 활성화하기
    /// todo: 실제로 사용 시 이벤트로 사용 가능
    /// </summary>
    public void GetMindPiece()
    {
        _mindPiece.gameObject.SetActive(true);
    }
    #endregion

    #region 테스트
    private void OnClickTestReloadButton()
    {
        _testReLoadButton.onClick.RemoveListener(OnClickTestReloadButton);
        Scene curScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(curScene.name);
    }

    private void OnClickTestStartButton()
    {
        _testStartButton.onClick.RemoveListener(OnClickTestStartButton);
        _shadow.IsTest = false;
    }

    private void OnClickTestInvisibleButton()
    {
        var player = FindObjectOfType<PlayerController>();
        player.transform.position = new Vector3(0, 1, 495);
    }
    #endregion
}

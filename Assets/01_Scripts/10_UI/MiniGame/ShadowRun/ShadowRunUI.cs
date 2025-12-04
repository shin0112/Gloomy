using System.Collections;
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

    [Header("게임 연출")]
    [SerializeField] private GameObject _pressDashKeyObj;
    [SerializeField] private float _delayShowDashKeyTime = 1f;

    [Header("테스트용")]
    [SerializeField] private Button _testReLoadButton;
    [SerializeField] private Button _testStartButton;
    [SerializeField] private Button _testInvisibleButton;
    [SerializeField] private Button _testIGoalButton;

    // 코루틴
    private Coroutine _showDashKeyCoroutine;
    #endregion

    #region Unity API
    private void Reset()
    {
        _closeEffect = transform.FindChild<Image>("Image- CloseEffect");
        _distanceText = transform.FindChild<TextMeshProUGUI>("Text - Distance");
        _mindPiece = transform.FindChild<Image>("Image - MindPiece");
        _pressDashKeyObj = transform.FindChild<Image>("Image - PressDashKey").gameObject;

        // test
        _testReLoadButton = transform.FindChild<Button>("Button - ReLoad");
        _testStartButton = transform.FindChild<Button>("Button - Start");
        _testInvisibleButton = transform.FindChild<Button>("Button - Invisible");
        _testIGoalButton = transform.FindChild<Button>("Button - Goal");
    }

    private void Start()
    {
        _shadow = FindObjectOfType<ShadowController>();
        _shadow.OnCaughtTarget += OnCaughtTarget;
        _shadow.OnEscapeTarget += OnEscapeTarget;
    }

    private void OnEnable()
    {
        _distanceText.text = "0.00M";
        _mindPiece.gameObject.SetActive(false);

        // test
        _testReLoadButton.onClick.AddListener(OnClickTestReloadButton);
        _testStartButton.onClick.AddListener(OnClickTestStartButton);
        _testInvisibleButton.onClick.AddListener(OnClickTestInvisibleButton);
        _testIGoalButton.onClick.AddListener(OnClickTestGoalButton);
    }

    private void Update()
    {
        ChangeColorAlpha();
        UpdateDistanceText();
    }

    private void OnDisable()
    {
        _testInvisibleButton.onClick.RemoveAllListeners();
        _shadow.OnCaughtTarget -= OnCaughtTarget;
        _shadow.OnEscapeTarget -= OnEscapeTarget;
    }
    #endregion

    #region 게임 연출
    /// <summary>
    /// 그림자 잡힘 여부에 따라 이미지의 알파값을 조정
    /// </summary>
    private void ChangeColorAlpha()
    {
        Color color = _closeEffect.color;

        float delta = Time.deltaTime * _closeSpeed;

        if (_shadow.HasCaughtTarget)
        {
            color.a = Mathf.Min(color.a + delta, 1f);
        }
        else
        {
            color.a = Mathf.Max(color.a - delta, 0f);
        }

        _closeEffect.color = color;
    }

    /// <summary>
    /// 타겟이 잡혔을 때 동작하는 연출
    /// </summary>
    private void OnCaughtTarget()
    {
        Logger.Log("플레이어 잡힘");

        if (_showDashKeyCoroutine != null)
        {
            StopCoroutine(nameof(ShowPressDashKeyCoroutine));
            _showDashKeyCoroutine = null;
        }
        _showDashKeyCoroutine = StartCoroutine(nameof(ShowPressDashKeyCoroutine));
    }

    /// <summary>
    /// 타겟이 벗어났을 때 동작하는 연출
    /// </summary>
    private void OnEscapeTarget()
    {
        _pressDashKeyObj.SetActive(false);
    }

    /// <summary>
    /// 특정 시간 후 대시 키 이미지 보여주기
    /// </summary>
    private IEnumerator ShowPressDashKeyCoroutine()
    {
        yield return new WaitForSeconds(_delayShowDashKeyTime);

        _pressDashKeyObj.SetActive(true);
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

    private void OnClickTestGoalButton()
    {
        var player = FindObjectOfType<PlayerController>();
        player.transform.position = new Vector3(0, 1, 950);
    }
    #endregion
}

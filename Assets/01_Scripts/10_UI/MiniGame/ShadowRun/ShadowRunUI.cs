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
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _testShadowButtonText;
    private float _timer = 0f;
    private bool _onTimer = false;

    [SerializeField] private Button _testStartButton;
    [SerializeField] private Button _testShadowButton;
    [SerializeField] private Button _testTimerButton;
    [SerializeField] private Button _testInvisibleButton;
    [SerializeField] private Button _testIGoalButton;
    [SerializeField] private Button _testReLoadButton;

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
        _timerText = transform.FindChild<TextMeshProUGUI>("Text - Timer");
        _testShadowButtonText = transform.FindChild<TextMeshProUGUI>("Text (TMP) - Shadow");

        _testStartButton = transform.FindChild<Button>("Button - Start");
        _testShadowButton = transform.FindChild<Button>("Button - Shadow");
        _testTimerButton = transform.FindChild<Button>("Button - Timer");
        _testInvisibleButton = transform.FindChild<Button>("Button - Invisible");
        _testIGoalButton = transform.FindChild<Button>("Button - Goal");
        _testReLoadButton = transform.FindChild<Button>("Button - ReLoad");
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
        AddTestButtonListener();
    }

    private void Update()
    {
        ChangeColorAlpha();
        UpdateDistanceText();

        UpdateTimer();
        if (_onTimer)
        {
            _timer += Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        _shadow.OnCaughtTarget -= OnCaughtTarget;
        _shadow.OnEscapeTarget -= OnEscapeTarget;

        // test
        RemoveTestButtonListener();
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

    private void OnCaughtTarget()
    {
        _pressDashKeyObj.SetActive(true);
    }

    /// <summary>
    /// 타겟이 잡혔을 때 동작하는 연출 - 코루틴
    /// </summary>
    private void OnCaughtTargetCoroutine()
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
    /// <summary>
    /// 테스트 버튼 리스너 추가
    /// </summary>
    private void AddTestButtonListener()
    {
        _testStartButton.onClick.AddListener(OnClickTestStartButton);
        _testShadowButton.onClick.AddListener(OnClickTestShadowButton);
        _testTimerButton.onClick.AddListener(OnClickTestTimerButton);
        _testInvisibleButton.onClick.AddListener(OnClickTestInvisibleButton);
        _testIGoalButton.onClick.AddListener(OnClickTestGoalButton);
        _testReLoadButton.onClick.AddListener(OnClickTestReloadButton);
    }

    /// <summary>
    /// 테스트 버튼 리스너 해제
    /// </summary>
    private void RemoveTestButtonListener()
    {
        _testStartButton.onClick.RemoveAllListeners();
        _testShadowButton.onClick.RemoveAllListeners();
        _testTimerButton.onClick.RemoveAllListeners();
        _testInvisibleButton.onClick.RemoveAllListeners();
        _testIGoalButton.onClick.RemoveAllListeners();
    }

    private void UpdateTimer()
    {
        _timerText.text = _timer.ToString("0.00");
    }

    /// <summary>
    /// [test] 그림자 테스트 & 타이머 시작
    /// </summary>
    private void OnClickTestStartButton()
    {
        ToggleShadowTestMode();
        ResetTimer();
    }

    /// <summary>
    /// [test] 그림자 테스트 플래그 변경
    /// </summary>
    private void OnClickTestShadowButton()
    {
        ToggleShadowTestMode();
    }

    /// <summary>
    /// [test] 타이머 시작
    /// </summary>
    private void OnClickTestTimerButton()
    {
        ResetTimer();
    }

    /// <summary>
    /// [test] 투명 인간 스폰 위치로 순간 이동
    /// </summary>
    private void OnClickTestInvisibleButton()
    {
        var player = FindObjectOfType<PlayerController>();
        player.transform.position = new Vector3(0, 1, 495);
    }

    /// <summary>
    /// [test] 골 지점으로 순간 이동
    /// </summary>
    private void OnClickTestGoalButton()
    {
        var player = FindObjectOfType<PlayerController>();
        player.transform.position = new Vector3(0, 1, 950);
    }

    /// <summary>
    /// [test] 현재 씬 리로드
    /// </summary>
    private void OnClickTestReloadButton()
    {
        _testReLoadButton.onClick.RemoveListener(OnClickTestReloadButton);
        Scene curScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(curScene.name);
    }

    private void ResetTimer()
    {
        _timer = 0f;
        _onTimer = true;
    }

    private void ToggleShadowTestMode()
    {
        _shadow.IsTest = !_shadow.IsTest;
        _testShadowButtonText.text = $"그림자 움직임\n{(_shadow.IsTest ? "ON" : "OFF")}";
    }
    #endregion
}

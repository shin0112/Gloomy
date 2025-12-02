using UnityEngine;
using UnityEngine.UI;

public class CloseEffect : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private ShadowController _shadow;
    [SerializeField] private float _closeSpeed = 5f;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        _shadow = FindObjectOfType<ShadowController>();
    }

    private void Update()
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

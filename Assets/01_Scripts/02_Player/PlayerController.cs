using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 curtransformInput; //�̵���ũ��Ʈ © �� �ʼ�
    [SerializeField] private Transform cameraMoveObject; //ī�޶� �̵��� ����ϴ� ������Ʈ
    private Vector2 mouseDelta;//���콺 �̵��� �� �ʿ�
    private Rigidbody rb;//������ٵ�
    private float dir;
    [Header("Option")]
    [SerializeField] private float speed;//�̵��ӵ�
    [SerializeField] private float jumpPower;//�����Ŀ�
    [SerializeField] private float mouseSensesivity;//����
    [SerializeField] private float MaxRoationX;//X�� �ִ� ����
    [SerializeField] private float MinRoationX;//X�� �ּ� ����

    [SerializeField] private float curX;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Look();
    }

    public void Move()
    {
        Vector3 dir = transform.forward * curtransformInput.y + transform.right * curtransformInput.x;

        dir *= speed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }

    public void Look()
    {
        dir += mouseDelta.y * mouseSensesivity;
        dir = Mathf.Clamp(dir, MinRoationX, MaxRoationX);
        cameraMoveObject.localEulerAngles = new Vector3(-dir, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * mouseSensesivity, 0);
    }

    public void InputMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curtransformInput = context.ReadValue<Vector2>();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            curtransformInput = Vector2.zero;
        }
    }

    public void InputJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            rb.velocity = new Vector3(0, jumpPower, 0);
        }
    }

    public void InputLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
}

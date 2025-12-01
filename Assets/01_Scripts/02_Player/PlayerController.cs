using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 curtransformInput; //�̵���ũ��Ʈ © �� �ʼ�
    [SerializeField] private Transform cameraMoveObject; //ī�޶� �̵��� ����ϴ� ������Ʈ
    private Vector2 mouseDelta;//���콺 �̵��� �� �ʿ�
    private Rigidbody rb;//������ٵ�

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
        float mouseX = Input.GetAxis("mouse X") * mouseSensesivity;
        float mouseY = Input.GetAxis("mouse Y");

        transform.Rotate(Vector3.up * mouseX);
        transform.Rotate(Vector3.right * mouseY);
        transform.eulerAngles = new Vector3(mouseX, mouseY,0);
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

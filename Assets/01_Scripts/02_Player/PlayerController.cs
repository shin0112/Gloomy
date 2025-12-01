using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 curtransformInput; //이동스크립트 짤 때 필수
    [SerializeField] private Transform cameraMoveObject; //카메라 이동을 담당하는 오브젝트
    private Vector2 mouseDelta;//마우스 이동할 때 필요
    private Rigidbody rb;//리지드바디
    private float dir;
    [Header("Option")]
    [SerializeField] private float speed;//이동속도
    [SerializeField] private float jumpPower;//점프파워
    [SerializeField] private float mouseSensesivity;//감도
    [SerializeField] private float MaxRoationX;//X축 최대 각도
    [SerializeField] private float MinRoationX;//X축 최소 각도

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
        dir = Mathf.Clamp(dir, MinRoationX,MaxRoationX);
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

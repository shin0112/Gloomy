using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 curtransformInput; //방향 입력 값
    [SerializeField] private Transform cameraMoveObject; //카메라 이동 오브젝트
    private Vector2 mouseDelta;//마우스입력값
    private Rigidbody rb;//리지드바디
    private float dir;
    [Header("Option")]
    [SerializeField] private float speed;//이동스피드
    [SerializeField] private float jumpPower;//점프 파워
    [SerializeField] private float mouseSensesivity;//감도
    [SerializeField] private float MaxRoationX;//각도 최대값
    [SerializeField] private float MinRoationX;//각도 최소값

    private Vector3 moveDir;
    private bool isLock = true;
    private float runGameMoveTransformValue = 3f;
    [SerializeField] private float curX;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Look();
        Grabity();
        RunPlayerRule();
    }

    public void Move()
    {

       
        if (SceneManager.GetActiveScene().name == "ShadowRunScene")
        {
            //Vector3 runGameMoveX = Vector3.up;
            transform.Translate(transform.right *speed*Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (transform.position.z <= 3)
                {
                    transform.position += transform.right * runGameMoveTransformValue * Time.deltaTime;
                }
                if (transform.position.z >= 3)
                {
                    transform.position += transform.right * 0;
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {

                if (transform.position.z >= -3)
                {
                    transform.position += transform.right * -runGameMoveTransformValue * Time.deltaTime;
                }
                if (transform.position.z <= -3)
                {
                    transform.position += transform.right * 0;
                }

            }
        }
        else
        {
            moveDir = transform.forward * curtransformInput.y + transform.right * curtransformInput.x;
            moveDir *= speed;
            moveDir.y = rb.velocity.y;
            rb.velocity = moveDir;
        }
    }

    public void Look()
    {
        if (isLock == true)
        {
            dir += mouseDelta.y * mouseSensesivity;
            dir = Mathf.Clamp(dir, MinRoationX, MaxRoationX);
            cameraMoveObject.localEulerAngles = new Vector3(-dir, 0, 0);
            transform.eulerAngles += new Vector3(0, mouseDelta.x * mouseSensesivity, 0);
        }
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
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void InputLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void Grabity()
    {
        rb.AddForce(Vector3.down * 30, ForceMode.Acceleration);
    }

    public void RunPlayerRule()
    {
        if (SceneManager.GetActiveScene().name == "ShadowRunScene")
        {
            isLock = false;

        }
    }
}

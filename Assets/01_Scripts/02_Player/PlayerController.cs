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
    [SerializeField] private float maxRoationX;//각도 최대값
    [SerializeField] private float minRoationX;//각도 최소값

    [Header("RunGame")]
    [SerializeField] private float moveTime;
    private Vector3 moveDir;
    [SerializeField] private bool isAutoRun = true; 
    private bool isLock = true;
    private float runGameMoveTransformValue = 3f;
    private float[] index;

    
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
       
            if (transform.position.x < -3.0f)
            {
                transform.position = new Vector3(-3.0f, transform.position.y, transform.position.z);
            }
            else if (transform.position.x > 3.0f)
            {
                transform.position = new Vector3(3.0f, transform.position.y, transform.position.z);
            }
      
            moveDir = transform.forward * curtransformInput.y + transform.right * curtransformInput.x;
            moveDir *= speed;
            moveDir.y = rb.velocity.y;
            rb.velocity = moveDir;
        
    }

    public void Look()
    {
        if (isLock == true)
        {
            dir += mouseDelta.y * mouseSensesivity;
            dir = Mathf.Clamp(dir, minRoationX, maxRoationX);
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
        if (isAutoRun == true)
        {
            isLock = false;

        }
    }
    //public void RailIndex()
    //{
    //    index[0] = Mathf.Clamp(transform.position.x, -4, 1);

    //    for(int i = 0; )
    //    {

    //    }
    //}

}
//1번 2번 3번 레일 위치를 정한다
//1번 레일 position 값
//2번 레일 position 값
//3번 레일 position 값
//현제 위치가 1번레일일 경우
//현제 위치가 2번레일일 경우
//현제 위치가 3번레일일 경우
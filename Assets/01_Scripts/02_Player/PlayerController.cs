using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("playerMoveOption")]
    [SerializeField] private Vector2 curtransformInput; //방향 입력 값
    [SerializeField] private Transform cameraMoveObject; //카메라 이동 오브젝트
    private Vector2 mouseDelta;//마우스입력값
    private Rigidbody rb;//리지드바디
    [SerializeField] private GameObject rayObject;//ray로 점프제한 역할 //실질적 플레이어 이미지가 있는 오브젝트
    private float dir;//이동할 때 변수?
    [SerializeField] private Transform slidePivot;//슬라이더 할 때 기준점
    [SerializeField] LayerMask layerMask;
    [SerializeField] private Transform cameraTransfrom;//카메라
    [SerializeField] private CapsuleCollider capsuleCollider;
    private Vector3 moveDir;

    [Header("Option")]
    [SerializeField] private float speed;//이동스피드
    [SerializeField] private float jumpPower;//점프 파워
    [SerializeField] private float mouseSensesivity;//감도
    [SerializeField] private float maxRoationX;//각도 최대값
    [SerializeField] private float minRoationX;//각도 최소값

    [Header("RungameOption")]
    [SerializeField] private float DashPower; //대쉬 했을 때거리?
    [SerializeField] private float dashDuration = 3f; //대쉬 지속시간 기본 3초
    [SerializeField] private float dashCooldown = 3.5f;
    private float slidingSpeed = 500f;


    [Header("bool")]
    [SerializeField] private bool isOpenShadowScene = false;
    private bool isSliding = false;
    private bool isDash = false;

    [Header("Scripts")]
    public InteractableObject interactableObject;
    public IInteractable interactable;

    [Header("InputAction")]
    public InputActionReference interactAction;
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference slidingAction;
    public InputActionReference dashAction;
    public InputActionReference lookAction;
    RaycastHit hit;

    [Header("Obstacle")]
    [SerializeField] ShadowController shadowController;


    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        if (isOpenShadowScene == false)
        {
            cameraTransfrom.rotation = Quaternion.Euler(0, 0, 0);
            cameraTransfrom.position = new Vector3(0, 0, -7);
        }
        interactableObject = GetComponent<InteractableObject>();

    }

    void Update()
    {

        Look();
        if(isOpenShadowScene)
        {
            if (isSliding == true)
            {
                Sliding();
            }
            else if (isSliding == false)
            {
                NoSliding();
            }
        }
        
        IsGround();
        CharacterRay();
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.red);

    }

    private void FixedUpdate()
    {
        Move();
        if(isOpenShadowScene == true)
        {
            if (isDash)
            {
                Dash();
            }
        }
        
        //Gravity();

    }

    private void CatchMoveEnable()//inputaction 연결
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        slidingAction.action.Enable();
        lookAction.action.Enable();
        interactAction.action.Enable();
    }

    public void CatchMoveDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        slidingAction.action.Disable();
        interactAction.action.Disable();
        lookAction.action.Disable();
    }

    public void Move()//이동
    {
        if (isOpenShadowScene == true)
        {
            if (transform.position.x < -3.0f)
            {
                transform.position = new Vector3(-3.0f, transform.position.y, transform.position.z);
            }
            else if (transform.position.x > 3.0f)
            {
                transform.position = new Vector3(3.0f, transform.position.y, transform.position.z);
            }
        }
        moveDir = cameraTransfrom.forward * curtransformInput.y + cameraTransfrom.right * curtransformInput.x;
        moveDir *= speed;
        moveDir.y = rb.velocity.y;
        rb.velocity = moveDir;
    }

    public void Look()//마우스 이동으로 화면 돌리기
    {
        if (isOpenShadowScene == true)
        {
            return;
        }
        else
        {
            dir += mouseDelta.y * mouseSensesivity;
            dir = Mathf.Clamp(dir, minRoationX, maxRoationX);
            cameraMoveObject.localEulerAngles = new Vector3(-dir, 0, 0);
            transform.eulerAngles += new Vector3(0, mouseDelta.x * mouseSensesivity, 0);

            mouseDelta = Vector2.zero;
        }

    }

    public void Sliding()//슬라이딩
    {

        float playerRotate = Mathf.MoveTowardsAngle(slidePivot.eulerAngles.x, -90, slidingSpeed * Time.deltaTime);
        slidePivot.localEulerAngles = new Vector3(playerRotate, 0, 0);

    }

    public void NoSliding()//슬라이딩 안할 때
    {
        float playerRotate = Mathf.MoveTowardsAngle(slidePivot.eulerAngles.x, 0, slidingSpeed * Time.deltaTime);
        slidePivot.localEulerAngles = new Vector3(playerRotate, 0, 0);
    }

    public void Dash()//대쉬
    {

        StartCoroutine(DashTime());
        //isDash = false;
    }

    public void InputMove(InputAction.CallbackContext context)//움직임 입력
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

    public void InputJump(InputAction.CallbackContext context)//점프 입력
    {
        if (context.phase == InputActionPhase.Started && IsGround())
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void InputSliding(InputAction.CallbackContext context)//슬라이딩 입력
    {
        if (context.performed)
        {
            isSliding = true;
        }
        if (context.canceled)
        {
            isSliding = false;
        }
    }

    public void InputDash(InputAction.CallbackContext context)//대쉬 입력
    {
        if (context.started)
        {
            isDash = true;
        }
    }

    public void InputLook(InputAction.CallbackContext context)//카메라 입력
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void InputIinteract(InputAction.CallbackContext context)//interact 입력(E)
    {

        if (context.phase == InputActionPhase.Started)
        {
            interactable?.OnInteract();
        }
        else if (context.phase == InputActionPhase.Performed)
        {
            return;
        }

    }

    bool IsGround()//플레이어가 레이와 닿았는지 체크
    {
        Ray ray = new Ray(rayObject.transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, 0.5f, layerMask))
        {
            return true;
        }
        return false;
    }

    public void Gravity()
    {
        rb.AddForce(Vector3.down * 30, ForceMode.Acceleration);
    }

    public IEnumerator DashTime()//대쉬타임 그리고 쿨타임
    {
        Vector3 dash = transform.forward;
        dash *= DashPower;
        rb.velocity += dash;
        //capsuleCollider.enabled = false;

        yield return new WaitForSeconds(dashDuration);
        isDash = false;
        capsuleCollider.enabled = true;
        yield return new WaitForSeconds(dashCooldown);
        StopCoroutine(DashTime());
    }

    public void CharacterRay()//캐릭터로 레이쏘기
    {
        
        if (Physics.Raycast(rayObject.transform.position, transform.forward, out hit, 1f))
        {
            InteractableObject obj = hit.collider.gameObject.GetComponent<InteractableObject>();
            interactable = hit.collider.gameObject.GetComponent<IInteractable>();
        }
    }
    public void ShadowCollision()
    {
        if(shadowController.HasCaughtTarget == true)
        {
            CatchMoveDisable();
            isDash = true;
            dashCooldown = 0;
            if (Input.GetKey(KeyCode.E))
            {
                Dash();
                CatchMoveEnable();
            }
            
        }
    }    
}

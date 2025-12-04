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
    public bool isOpenShadowScene = false;
    private bool isSliding = false;
    public bool isDash = false;
    private bool isMove = true;
    private bool isJump = true;


    [Header("Scripts")]
    public InteractableObject interactableObject;
    public IInteractable interactable;
    public ShadowController shadowController;


    RaycastHit hit;


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
        if (SceneManager.GetActiveScene().name == "ShadowRunScene")
        {
            shadowController = FindObjectOfType<ShadowController>();
        }
       
        isDash = false;
        //isMove = true;
    }

    void Update()
    {

        Look();
        if (isOpenShadowScene)
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

        //IsGround();
        CharacterRay();
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.red);
        ShadowCollision();

    }

    private void FixedUpdate()
    {
        Move();
        if (isOpenShadowScene == true)
        {
            if (isDash == true)
            {
                Dash();
            }
        }
        //Gravity();
    }

    public void IsTrue()
    {
        isDash = true;
        isJump = true;
        isMove = true;
       
    }

    public void IsFalse()
    {
        isDash = false;
        isJump = false;
        isMove = false;
        isSliding = false;
    }

    public void Move()
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
        if (isMove == true)
        {
            moveDir = cameraTransfrom.forward * curtransformInput.y + cameraTransfrom.right * curtransformInput.x;
            moveDir *= speed;
            moveDir.y = rb.velocity.y;
            rb.velocity = moveDir;
        }

    }

    public void Look()
    {
        
        if(isOpenShadowScene == false)
        {
            dir += mouseDelta.y * mouseSensesivity;
            dir = Mathf.Clamp(dir, minRoationX, maxRoationX);
            cameraMoveObject.localEulerAngles = new Vector3(-dir, 0, 0);
            transform.eulerAngles += new Vector3(0, mouseDelta.x * mouseSensesivity, 0);

            mouseDelta = Vector2.zero;
        }
        else
        {
            return;
        }

    }

    public void Sliding()
    {
        float playerRotate = Mathf.MoveTowardsAngle(slidePivot.eulerAngles.x, -90, slidingSpeed * Time.deltaTime);
        slidePivot.localEulerAngles = new Vector3(playerRotate, 0, 0);
    }

    public void NoSliding()
    {
        float playerRotate = Mathf.MoveTowardsAngle(slidePivot.eulerAngles.x, 0, slidingSpeed * Time.deltaTime);
        slidePivot.localEulerAngles = new Vector3(playerRotate, 0, 0);
    }

    public void Dash()
    {

        StartCoroutine(DashTime());
        //isDash = false;
    }

    public void InputMove(InputAction.CallbackContext context)
    {
        if(isMove == true)
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
        
    }

    public void InputJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGround())
        {
            if (isJump == true)
            {
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }

        }
    }

    public void InputSliding(InputAction.CallbackContext context)
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

    public void InputDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isDash = true;
        }
    }

    public void InputLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void InputIinteract(InputAction.CallbackContext context)
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

    bool IsGround()
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

    public IEnumerator DashTime()
    {
        Vector3 dash = transform.forward;
        dash *= DashPower;
        rb.velocity += dash;
        //capsuleCollider.enabled = false;

        yield return new WaitForSeconds(dashDuration);
        isDash = false;
        //capsuleCollider.enabled = true;
        yield return new WaitForSeconds(dashCooldown);
    }

    public void CharacterRay()
    {

        if (Physics.Raycast(rayObject.transform.position, transform.forward, out hit, 5f))
        {
            InteractableObject obj = hit.collider.gameObject.GetComponent<InteractableObject>();
            interactable = hit.collider.gameObject.GetComponent<IInteractable>();
        }


    }

    public void ShadowCollision()
    {
        if (shadowController == null) return;
        if (shadowController.HasCaughtTarget == true)
        {
            dashCooldown = 0;
            IsFalse();
            if (Input.GetKey(KeyCode.F))
            {
                IsTrue();
                Dash();
            }

        }

    }
}

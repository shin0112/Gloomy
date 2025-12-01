using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 curtransform; //현제 위치
    private string moveAxisName = "Vertical";
    private Rigidbody rb;

    [Header("Option")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
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
        Vector3 dir = transform.forward * curtransform.y + transform.right * curtransform.x;
        
        dir *= speed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }

    public void InputMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curtransform = context.ReadValue<Vector2>();
        }
        if(context.phase == InputActionPhase.Canceled)
        {
            curtransform = Vector2.zero;
        }
    }
}

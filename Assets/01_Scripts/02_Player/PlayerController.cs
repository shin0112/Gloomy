using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 curtransform; //현제 위치
    private Rigidbody rb;

    [Header("Option")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    void Start()
    {
        
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertinal");
        float xSpeed = xInput * speed * Time.deltaTime;
        float zSpeed = zInput * speed * Time.deltaTime;

        Vector3 move = new Vector3(xInput,0,zInput);
    }
}

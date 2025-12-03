using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlMove : MonoBehaviour
{
    public float minX = -4.5f;
    public float maxX = 4.5f;
    public float speed = 2f;

    private float direction = 1f;

    void Update()
    {
        //이동
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        //방향 반전
        if (transform.position.x > maxX)
            direction = -1f;
        else if (transform.position.x < minX)
            direction = 1f;
    }
}

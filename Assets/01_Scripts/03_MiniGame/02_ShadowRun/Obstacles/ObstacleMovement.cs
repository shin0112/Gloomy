using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    private Vector3 startPos;
    public float maxDistance = 30f;
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
        if (Vector3.Distance(transform.position, startPos) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
}

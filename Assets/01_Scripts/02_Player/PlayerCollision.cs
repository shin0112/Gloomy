using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private LayerMask layerMask;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)//대쉬시 장애물 파괴
    {
        if (controller.isDash == true)
        {
            if ((layerMask & (1 << other.gameObject.layer)) != 0)
            {
                Debug.Log("Destroy : " + other.name);
                Destroy(other.gameObject);
            }
        }
    }
}

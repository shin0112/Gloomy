using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticle : MonoBehaviour
{
    [SerializeField] RectTransform target;
    ParticleSystem particle;
    Camera cam;

    [SerializeField] float zOffset = 0.1f;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 dir = Vector3.Normalize(cam.transform.position - target.position);
        particle.transform.position = target.transform.position + (dir * zOffset);

    }


    public void SetTargetRectTransform(RectTransform _target)
    {
        target = _target;
    }
}
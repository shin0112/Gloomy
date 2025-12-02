using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticle : MonoBehaviour
{
    [SerializeField] RectTransform target;
     ParticleSystem particle;
    Camera cam; 

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(cam, target.position);
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 1f));

        particle.transform.position = worldPos;
    }


    public void SetTargetRectTransform(RectTransform _target)
    {
        target = _target;
    }
}

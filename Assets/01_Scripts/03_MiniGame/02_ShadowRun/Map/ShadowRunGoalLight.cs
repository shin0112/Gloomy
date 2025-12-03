using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowRunGoalLight : MonoBehaviour
{
    [Header("1000미터 기준 기본 사이즈")]
    [SerializeField] private float defaultViewScale = 2;

    [Header("도착 시 목표 사이즈")]
    [SerializeField] private float finalScale = 10f;

    [Header("빛이 커지기 시작하는 거리")]
    [SerializeField] private float stopCorrectDistance = 100;

    [Header("거리 타겟")]
    // todo : 후에 Player 를 찾을 수 있게 추가 필요
    [SerializeField] Transform target;

    private void Update()
    {
        float dis = Vector3.Distance(target.position, transform.position);

        CalcScale(dis);
    }

    void CalcScale(float dis)
    {
        if (dis > stopCorrectDistance)
        {
            this.transform.localScale = Vector3.one * ((dis / Define.MapLength) * defaultViewScale);
        }
        else
        {
            if (dis <= 0) return;
            float boundary = (stopCorrectDistance / Define.MapLength) * defaultViewScale;
            float calcBalance = boundary * stopCorrectDistance;
            this.transform.localScale = Vector3.one * (calcBalance / dis);
        }
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (target == null) return;
        float dis = Vector3.Distance(target.position, transform.position);
        CalcScale(dis);
    }
#endif

}
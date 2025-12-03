using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceTextTest : MonoBehaviour
{
    private TextMeshProUGUI text;
    [Header("거리 볼 오브젝트들")]
    [SerializeField] Transform target1;
    [SerializeField] Transform target2;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(target2.transform.position, target1.position);
        text.text = distance.ToString("F2");
    }
}

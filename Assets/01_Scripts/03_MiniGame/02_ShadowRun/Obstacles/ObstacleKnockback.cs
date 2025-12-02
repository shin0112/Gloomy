using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleKnockback : MonoBehaviour
{
    public float distance = 0.33f;
    public float duration = 0.2f;

    Vector3 start, end;
    float t;
    bool active;

    public void DoKnockback(Vector3 dir)
    {
        start = transform.position;
        end = start + dir.normalized * distance;
        t = 0;
        active = true;
    }
    private void Update()
    {
        if (!active)
            return;
        t += Time.deltaTime / duration;
        transform.position=Vector3.Lerp(start, end, t);

        if (t >= 1) active = false;
    }
}

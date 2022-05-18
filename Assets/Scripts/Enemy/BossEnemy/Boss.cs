using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header ("Missile Info")]
    public GameObject missile;
    public Transform missileR;
    public Transform missileL;

    // 플레이어 움직임 예측
    Vector3 lookVec;
    Vector3 tauntVec;

    // 플레이어 바라보는 플래그
    bool isLook;

    void Start()
    {
        isLook = true;
    }

    void Update()
    {
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 4f;
            transform.LookAt(target.position + lookVec);
        }
    }
}

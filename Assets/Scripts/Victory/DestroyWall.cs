using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    public Boss boss;

    private void Update()
    {
        if (boss.curHp <= 0)
            Destroy(gameObject);
    }
}

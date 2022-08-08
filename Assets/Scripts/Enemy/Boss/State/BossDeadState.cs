using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadState : MonoBehaviour, BossState
{
    Boss owner;

    public void ExcuteEnter()
    {
        StopAllCoroutines();
    }

    public void ExcuteExit()
    {
        
    }

    public void Init(Boss owner)
    {
        this.owner = owner;
    }
}

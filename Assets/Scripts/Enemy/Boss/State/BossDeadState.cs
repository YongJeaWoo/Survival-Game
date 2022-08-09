using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadState : MonoBehaviour, IBossState
{
    Boss owner;

    public void ExcuteEnter()
    {
        StopAllCoroutines();
    }

    public void ExcuteUpdate()
    {
        
    }

    public void ExcuteExit()
    {
        
    }

    public void Init(Boss owner)
    {
        this.owner = owner;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossState
{
    Boss owner;

    int actionTime = 0;

    public void ExcuteEnter()
    {
        actionTime++;

        LookTarget();
    }

    public void ExcuteExit()
    {
        if (actionTime > 4)
        {
            owner.ChangeState(Boss.EBossState.ATTACK);
            actionTime = 0;
        }
    }

    public void Init(Boss owner)
    {
        this.owner = owner;
    }

    void LookTarget()
    {
        if (owner.isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            owner.lookVec = new Vector3(h, 0, v) * 3f;
            owner.transform.LookAt(owner.target.position + owner.lookVec);
        }

        else
        {
            owner.nav.SetDestination(owner.tauntVec);
        }
    }
}

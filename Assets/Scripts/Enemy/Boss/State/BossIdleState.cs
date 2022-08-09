using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : IBossState
{
    Boss owner;

    float actionTime = 0;

    public void ExcuteEnter()
    {

    }

    public void ExcuteUpdate()
    {
        LookTarget();

        actionTime += Time.deltaTime;

        if (actionTime > 4)
        {
            owner.ChangeState(Boss.EBossState.ATTACK);
            Debug.Log("Change to Attack");
        }

        Debug.Log("IDLE");
    }

    public void ExcuteExit()
    {
        actionTime = 0;
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

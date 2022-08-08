using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : MonoBehaviour, BossAttack
{
    Boss owner;

    public void AttackEnter()
    {
        // SwitchAttack();
    }

    public void AttackExit()
    {
        owner.ChangeState(Boss.EBossState.IDLE);

        if (owner.isDead)
        {
            owner.ChangeState(Boss.EBossState.DEAD);
        }
    }

    public void Init(Boss owner)
    {
        this.owner = owner;
    }

    void SwitchAttack()
    {
        owner.ChangeAttack(Boss.EBossAttack.MISSILE);
        owner.ChangeAttack(Boss.EBossAttack.ROCK);
        owner.ChangeAttack(Boss.EBossAttack.TAUNT);
    }
}

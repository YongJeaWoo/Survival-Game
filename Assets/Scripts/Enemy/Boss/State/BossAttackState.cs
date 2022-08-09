using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : MonoBehaviour, IBossState
{
    public enum EBossAttack
    {
        NONE,
        MISSILE,
        ROCK,
        TAUNT,
        COUNT,
    }

    Boss owner;

    float attackTime = 0;

    public void Init(Boss owner)
    {
        this.owner = owner;
    }

    public void ExcuteEnter()
    {

    }

    public void ExcuteUpdate()
    {
        SwitchAttack();

        if (owner.isDead)
        {
            owner.ChangeState(Boss.EBossState.DEAD);
        }
    }

    public void ExcuteExit()
    {

    }

    void SwitchAttack()
    {
        attackTime += Time.deltaTime;

        if (attackTime > 3)
        {
            Debug.Log("1");
            StopAllCoroutines();
            attackTime = 0;
            EBossAttack randomAction = (EBossAttack)Random.Range(0, (float)EBossAttack.COUNT);

            switch (randomAction)
            {
                case EBossAttack.NONE:
                    owner.anim.SetTrigger("doBigShot");
                    break;
                case EBossAttack.MISSILE:
                    {
                        StartCoroutine(Missile());
                    }
                    break;
                case EBossAttack.ROCK:
                    {
                        StartCoroutine(Rock());
                    }
                    break;
                case EBossAttack.TAUNT:
                    {
                        StartCoroutine(Taunt());
                    }
                    break;
            }
            owner.ChangeState(Boss.EBossState.IDLE);
        }
    }

    IEnumerator Missile()
    {
        owner.anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.4f);
        GameObject instantMissileR = Instantiate(owner.missile, owner.missileR.position, owner.missileR.rotation);
        BossMissile bossMissileR = instantMissileR.GetComponent<BossMissile>();
        bossMissileR.target = owner.target;

        yield return new WaitForSeconds(0.5f);
        GameObject instantMissileL = Instantiate(owner.missile, owner.missileL.position, owner.missileL.rotation);
        BossMissile bossMissileL = instantMissileL.GetComponent<BossMissile>();
        bossMissileL.target = owner.target;

        yield return new WaitForSeconds(3f);
    }

    IEnumerator Rock()
    {
        owner.isLook = false;
        owner.anim.SetTrigger("doBigShot");

        Instantiate(owner.bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);

        owner.isLook = true;
    }

    IEnumerator Taunt()
    {
        Collider[] rayTaunt = Physics.OverlapSphere(transform.position, 40f, LayerMask.GetMask("Player"));

        owner.tauntVec = owner.target.position + owner.lookVec;

        if (rayTaunt.Length > 0)
        {
            owner.isLook = false;
            owner.nav.isStopped = false;
            // boxCollider.enabled = false;
            owner.anim.SetTrigger("doTaunt");

            yield return new WaitForSeconds(1.5f);
            owner.attackArea.enabled = true;
            // boxCollider.enabled = true;

            yield return new WaitForSeconds(0.5f);
            owner.attackArea.enabled = false;

            yield return new WaitForSeconds(3f);
            owner.isLook = true;
            owner.nav.isStopped = true;
        }
    }
}

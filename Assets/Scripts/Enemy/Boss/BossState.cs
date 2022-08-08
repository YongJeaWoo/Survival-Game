using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossState : Enemy
{
    public enum State
    {
        IDLE,
        THINK,
        ATTACK,
        DEAD,
        END
    }

    [Header("Missile Info")]
    public GameObject missile;
    public Transform missileR;
    public Transform missileL;

    // 플레이어 움직임 예측
    Vector3 lookVec;
    Vector3 tauntVec;

    bool isLook;

    State state = State.IDLE;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        nav = GetComponent<NavMeshAgent>();
        mesh = GetComponentsInChildren<MeshRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        nav.isStopped = true;
        StateCheck();
    }

    private void Update()
    {
        LookTarget();
    }

    // 상태 체크
    private void StateCheck()
    {
        switch (state)
        {
            case State.IDLE:
                // 바라보고 있는 상태
            case State.THINK:
                // 패턴 생각
                StartCoroutine(Think());
                break;
            case State.ATTACK:
                // 공격 패턴 실행 중
                AttackReady();
                break;
            case State.DEAD:
                if (isDead)
                {
                    StopAllCoroutines();
                    return;
                }
                break;
        }
    }

    private void LookTarget()
    {
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            lookVec = new Vector3(h, 0, v) * 3f;
            transform.LookAt(target.position + lookVec);
        }
        else
            nav.SetDestination(tauntVec);
    }

    private void AttackReady()
    {
        int attackTarget = Random.Range(0, 2);
        switch (attackTarget)
        {
            case 0:
                MissileAttack();
                break;
            case 1:
                RockAttack();
                break;
            case 2:
                StartCoroutine(TauntAttack());
                break;
        }
    }

    private void MissileAttack()
    {
        float instantMissile = 0f;
        instantMissile += 0.1f;
        anim.SetTrigger("doShot");

        if (instantMissile == 0.5f)
        {
            GameObject instantMissileR = Instantiate(missile, missileR.position, missileR.rotation);
            BossMissile bossMissileR = instantMissileR.GetComponent<BossMissile>();
            bossMissileR.target = target;
        }

        if (instantMissile == 0.7f)
        {
            GameObject instantMissileL = Instantiate(missile, missileL.position, missileL.rotation);
            BossMissile bossMissileL = instantMissileL.GetComponent<BossMissile>();
            bossMissileL.target = target;
        }

        StateCheck();
    }

    private void RockAttack()
    {
        anim.SetTrigger("doBigShot");
        Instantiate(bullet, transform.position, transform.rotation);
        StateCheck();
    }

    IEnumerator TauntAttack()
    {
        Collider[] rayTaunt = Physics.OverlapSphere(transform.position, 40f, LayerMask.GetMask("Player"));

        tauntVec = target.position + lookVec;

        if (rayTaunt.Length > 0)
        {
            isLook = false;
            nav.isStopped = false;
            anim.SetTrigger("doTaunt");

            yield return new WaitForSeconds(1.5f);
            attackArea.enabled = true;

            yield return new WaitForSeconds(0.5f);
            attackArea.enabled = false;

            yield return new WaitForSeconds(3f);
            isLook = true;
            nav.isStopped = true;
            StartCoroutine(Think());
        }
        else
        {
            StartCoroutine(Think());
            yield return null;
        }
    }

    IEnumerator Think()
    {
        int randomAction = Random.Range(0, 3);

        switch (randomAction)
        {
            case 0:
                state = State.IDLE;
                break;
            case 1:
                state = State.THINK;
                break;
            case 2:
                state = State.ATTACK;
                break;
        }

        yield return new WaitForSeconds(5f);
    }  
}


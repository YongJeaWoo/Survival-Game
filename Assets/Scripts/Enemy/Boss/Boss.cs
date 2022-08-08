using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public enum EBossState
    {
        IDLE,
        ATTACK,
        DEAD,
        END
    }

    public enum EBossAttack
    {
        NONE,
        MISSILE,
        ROCK,
        TAUNT,
        END
    }
    
    BossState curBossState;
    BossAttack curBossAttack;

    List<BossState> bossStates = new List<BossState>();
    List<BossAttack> bossAttack = new List<BossAttack>();

    [Header ("Missile Info")]
    public GameObject missile;
    public Transform missileR;
    public Transform missileL;

    // 플레이어 움직임 예측
    public Vector3 lookVec;
    public Vector3 tauntVec;

    // 플레이어 바라보는 플래그
    public bool isLook;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mesh = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        bossStates.Add(new BossIdleState());
        bossStates.Add(new BossDeadState());
        bossAttack.Add(new BossAttackState());

        foreach (BossState state in bossStates)
            state?.Init(this);
    }

    private void Start()
    {
        nav.isStopped = true;
        curBossState = bossStates[(int)EBossState.IDLE];
        curBossAttack = bossAttack[(int)EBossAttack.NONE];
        // StartCoroutine(Think());
    }

    public void ChangeState(EBossState nextState)
    {
        curBossState.ExcuteExit();
        curBossState = bossStates[(int)nextState];
        curBossState.ExcuteEnter();
    }

    public void ChangeAttack(EBossAttack nextAttack)
    {
        curBossAttack.AttackExit();
        curBossAttack = bossAttack[(int)nextAttack];
        curBossAttack.AttackEnter();
    }

    void Update()
    {
        curBossState.ExcuteEnter();
        curBossAttack.AttackEnter();
    }

    /*
    IEnumerator Think()
    {
        yield return new WaitForSeconds(1f);

        int randomAct = Random.Range(0, 4);
        switch (randomAct)
        {
            case 0:
            // 기본상태
            case 1:
                // 미사일 공격
                StartCoroutine(Missile());
                break;
            case 2:
                // 돌 공격
                StartCoroutine(Rock());
                break;
            case 3:
                // 점프
                StartCoroutine(Taunt());
                break;
        }
    }

    IEnumerator Missile()
    {
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.4f);
        GameObject instantMissileR = Instantiate(missile, missileR.position, missileR.rotation);
        BossMissile bossMissileR = instantMissileR.GetComponent<BossMissile>();
        bossMissileR.target = target;

        yield return new WaitForSeconds(0.5f);
        GameObject instantMissileL = Instantiate(missile, missileL.position, missileL.rotation);
        BossMissile bossMissileL = instantMissileL.GetComponent<BossMissile>();
        bossMissileL.target = target;

        yield return new WaitForSeconds(3f);
        // StartCoroutine(Think());
    }

    IEnumerator Rock()
    {
        isLook = false;
        anim.SetTrigger("doBigShot");

        Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);

        isLook = true;
        // StartCoroutine(Think());
    }

    IEnumerator Taunt()
    {
        Collider[] rayTaunt = Physics.OverlapSphere(transform.position, 40f, LayerMask.GetMask("Player"));

        tauntVec = target.position + lookVec;

        if (rayTaunt.Length > 0)
        {
            isLook = false;
            nav.isStopped = false;
            // boxCollider.enabled = false;
            anim.SetTrigger("doTaunt");

            yield return new WaitForSeconds(1.5f);
            attackArea.enabled = true;
            // boxCollider.enabled = true;

            yield return new WaitForSeconds(0.5f);
            attackArea.enabled = false;

            yield return new WaitForSeconds(3f);
            isLook = true;
            nav.isStopped = true;
            // StartCoroutine(Think());
        }
        else
        {
            // StartCoroutine(Think());
            yield return null;
        }
    }
    */

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 20f);
    }
}

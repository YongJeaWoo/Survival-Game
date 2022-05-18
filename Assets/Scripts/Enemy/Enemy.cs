using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type
    {
        A,
        B,
        C,
    };

    public Type enemyType;


    public int maxHp;
    public int curHp;

    public bool isChase;
    public bool isAttack;

    public BoxCollider enemyArea;
    public GameObject bullet;

    public Transform target;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    NavMeshAgent nav;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        Invoke("ChaseStart", 2f);
    }

    private void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }

    private void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }

    void Targeting()
    {
        float targetRadius = 0f;
        float targetRange = 0f;

        switch (enemyType)
        {
            case Type.A:
                targetRadius = 0.8f;
                targetRange = 1f;
                break;
            case Type.B:
                targetRadius = 0.5f;
                targetRange = 4f;
                break;
            case Type.C:
                targetRadius = 0.3f;
                targetRange = 20f;
                break;
        }



        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if(rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);

        switch (enemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.78f);
                enemyArea.enabled = true;

                yield return new WaitForSeconds(1.5f);
                enemyArea.enabled = false;

                yield return new WaitForSeconds(1f);

                break;
            case Type.B:
                yield return new WaitForSeconds(0.5f);

                rigid.AddForce(transform.forward * 20f, ForceMode.Impulse);
                enemyArea.enabled = true;

                yield return new WaitForSeconds(0.45f);
                rigid.velocity = Vector3.zero;
                enemyArea.enabled = false;

                yield return new WaitForSeconds(3f);
                break;
            case Type C:
                yield return new WaitForSeconds(0.5f);

                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = transform.forward * 20f;
                    
                yield return new WaitForSeconds(2f);
                break;

        }

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Melee"))
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHp -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(Ondamage(reactVec, false));
        }
        else if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHp -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(Ondamage(reactVec, false));
        }
    }

    IEnumerator Ondamage(Vector3 reactVec, bool grenadeReact)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (curHp > 0)
        {
            mat.color = Color.white;
        }
        else
        {
            mat.color = Color.gray;
            gameObject.layer = 21;

            // 넉백
            if (grenadeReact)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;

                rigid.freezeRotation = false;
                rigid.AddForce(reactVec * Random.Range(30, 50), ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);

            }
            else
            {
                reactVec = -reactVec.normalized;
                reactVec += Vector3.up;
                rigid.AddForce(reactVec * 8f, ForceMode.Impulse);
            }

            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");
            Destroy(gameObject, 4f);
        }
    }

    public void HitByGrenade(Vector3 explosion)
    {
        curHp -= 100;
        Vector3 reactVec = transform.position - explosion;
        StartCoroutine(Ondamage(reactVec, true));
    }
}

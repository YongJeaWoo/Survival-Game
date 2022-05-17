using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHp;
    public int curHp;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponent<MeshRenderer>().material;
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
                rigid.AddForce(reactVec * Random.Range(1, 5), ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);

            }
            else
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rigid.AddForce(reactVec * 10f, ForceMode.Impulse);
            }


            Destroy(gameObject, 2f);
        }
    }

    public void HitByGrenade(Vector3 explosion)
    {
        curHp -= 100;
        Vector3 reactVec = transform.position - explosion;
        StartCoroutine(Ondamage(reactVec, true));
    }
}

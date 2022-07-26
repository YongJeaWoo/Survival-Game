using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type
    {
        Melee,
        Range,
    }

    [Header("Pooling Obj")]
    // public GameObject prefabObj;

    List<GameObject> popList = new List<GameObject>();

    ObjectPooling pooling = new ObjectPooling();

    int poolingCount = 30;

    private void Awake()
    {
        pooling.OnRePooling += PoolingObj;
        pooling.OnRePooling?.Invoke();
    }

    void PoolingObj()
    {
        for (int i = 0; i < poolingCount; ++i)
        {
            GameObject instantiateBullet = Instantiate(bullet, transform.position, transform.rotation);
            pooling.Push(instantiateBullet);

            // Rigidbody bulletRigid = instantiateBullet.GetComponent<Rigidbody>();
            // bulletRigid.velocity = bulletPos.forward * 50f;

            //GameObject instantiateCase = Instantiate(bulletCase, transform.position, transform.rotation);
            //pooling.Push(instantiateCase);

            //Rigidbody bulletCaseRigid = instantiateCase.GetComponent<Rigidbody>();
            //Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -1) + Vector3.up * Random.Range(2, 4);

            //bulletCaseRigid.AddForce(caseVec, ForceMode.Impulse);
            //gunSound.Play();
            //bulletCaseRigid.AddTorque(Vector3.up * 20f, ForceMode.Impulse);
        }
    }

    public void CreateObj()
    {
        GameObject obj = pooling.Pop(bulletPos.position);
        popList.Add(obj);

        Rigidbody bulletRigid = obj.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50f;

        obj.SetActive(true);
    }

    public void RemoveObj(GameObject obj)
    {
        pooling.Push(obj);
        popList.Remove(obj);
    }

    public void RemoveAllObj()
    {
        int cnt = popList.Count;
        for (int i = 0; i < cnt; ++i)
        {
            RemoveObj(popList[0]);
        }
    }


    public Type type;

    [Header("Sound Info")]
    public AudioSource gunSound;
    public AudioSource emptySound;

    [Header ("Weapon Type")]
    public int damage;
    public float rate;

    public BoxCollider meleeArea;
    public TrailRenderer trailRenderer;

    [Header ("Weapon Info")]
    public Transform bulletPos;
    public GameObject bullet;

    public Transform bulletCasePos;
    public GameObject bulletCase;

    public int maxAmmo;
    public int curAmmo;


    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine(Swing());
            StartCoroutine(Swing());
        }

        else if (type == Type.Range && curAmmo > 0)
        {
            curAmmo--;
            StartCoroutine(Shot());
        }

        else if (type == Type.Range && curAmmo == 0)
        {
            StartCoroutine(Empty());
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = true;
        trailRenderer.enabled = true;

        yield return new WaitForSeconds(0.7f);
        meleeArea.enabled = false;
    }

    IEnumerator Shot()
    {
        CreateObj();

        //GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        //Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();

        //bulletRigid.velocity = bulletPos.forward * 50f;

        yield return null;

        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();

        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -1) + Vector3.up * Random.Range(2, 4);

        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        gunSound.Play();
        caseRigid.AddTorque(Vector3.up * 20f, ForceMode.Impulse);
    }

    IEnumerator Empty()
    {
        emptySound.Play();
        yield break;
    }
}

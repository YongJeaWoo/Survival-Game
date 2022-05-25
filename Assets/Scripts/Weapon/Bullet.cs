using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        // 탄피 제거
        if (collision.gameObject.CompareTag("BulletCheck"))
            Destroy(gameObject,2f);
        else if (collision.gameObject.CompareTag("Ground"))
            Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BulletCheck")
            Destroy(gameObject);
        else if (other.gameObject.CompareTag("Objects"))
            Destroy(gameObject);
        else if (other.gameObject.CompareTag("Ground"))
            Destroy(gameObject, 1f);
        else if (other.gameObject.CompareTag("Enemy"))
            Destroy(gameObject);
    }
}

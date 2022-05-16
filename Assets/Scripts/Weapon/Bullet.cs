using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Objects")
            Destroy(gameObject);
        else if (collision.gameObject.tag == "Ground")
            Destroy(gameObject, 1f);
        else if (collision.gameObject.tag == "BulletCheck")
            Destroy(gameObject);
        //if (collision.gameObject.CompareTag("Enemy"))
        //    Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Objects")
            Destroy(gameObject);
        else if (other.gameObject.tag == "Ground")
            Destroy(gameObject, 1f);
        else if (other.gameObject.tag == "BulletCheck")
            Destroy(gameObject);
        //if (collision.gameObject.CompareTag("Enemy"))
        //    Destroy(gameObject);
    }
}

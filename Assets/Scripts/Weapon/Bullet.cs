using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            Destroy(gameObject, 1f);
        else if (collision.gameObject.tag == "Wall")
            Destroy(gameObject);
        else if (collision.gameObject.tag == "Objects")
            Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public int damage;

    public event UnityAction<GameObject> onReleaseBulletEvent;

    private void OnCollisionEnter(Collision collision)
    {
        // 탄피 제거
        if (collision.gameObject.CompareTag("BulletCheck"))
        {
            gameObject.SetActive(false);
            onReleaseBulletEvent?.Invoke(gameObject);
        }
        else if (collision.gameObject.CompareTag("Tile"))
        {
            gameObject.SetActive(false);
            onReleaseBulletEvent?.Invoke(gameObject);
        }
        else if (collision.gameObject.CompareTag("Grass"))
        {
            gameObject.SetActive(false);
            onReleaseBulletEvent?.Invoke(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BulletCheck"))
        {
            gameObject.SetActive(false);
            onReleaseBulletEvent?.Invoke(gameObject);
        }
        else if (other.gameObject.CompareTag("Objects"))
        {
            gameObject.SetActive(false);
            onReleaseBulletEvent?.Invoke(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            onReleaseBulletEvent?.Invoke(gameObject);
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            gameObject.SetActive(false);
            onReleaseBulletEvent?.Invoke(gameObject);
        }
    }
}

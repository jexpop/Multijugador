using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnCollisionEnter(Collision other)
    {
        GameObject collision = other.gameObject;
        var otherHealth = collision.GetComponent<Health>();

        if (otherHealth != null)
        {
            otherHealth.GetDamage(1);
        }

        Destroy(gameObject);
    }

}

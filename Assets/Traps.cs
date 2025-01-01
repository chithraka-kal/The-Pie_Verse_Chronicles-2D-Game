using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    [SerializeField] private int damage = 10; // Default damage dealt by the trap

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){

        // Check if the object can be damaged
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.Hit(damage, Vector2.zero); // Apply damage with no knockback
        }

        }
    }
}

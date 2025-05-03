using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damageAmount = 1;
    public Vector2 knockback = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Attack.OnTriggerEnter2D -> Collision with: " + collision.name);

        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            Debug.Log("Found Damageable on: " + collision.name);

            Knight knight = collision.GetComponent<Knight>();
            if (knight != null)
            {
                Debug.Log("Knight component found on: " + collision.name);
                Debug.Log("Knight gameObject.activeSelf: " + knight.gameObject.activeSelf);
                Debug.Log("Knight.enabled: " + knight.enabled);
            }

            damageable.Hit(damageAmount, knockback);
        }
        else
        {
            Debug.Log("No Damageable found on: " + collision.name);
        }
    }
}

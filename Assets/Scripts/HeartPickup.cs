using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HeartPickup : MonoBehaviour
{
    public int heartHealthAmount = 100; // One heart = 100 HP
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);

    AudioSource pickupSource;

    private void Awake()
    {
        pickupSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable)
        {
            // Increase max health and heal to full new max
            damageable.MaxHealth += heartHealthAmount;
            damageable.Health += heartHealthAmount;

            // Play sound and destroy pickup
            if (pickupSource)
            {
                AudioSource.PlayClipAtPoint(pickupSource.clip, transform.position, pickupSource.volume);
            }

            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}

using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioSource audioSource;        // Drag your AudioSource here
    public bool playOnlyOnce = false;
    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && audioSource != null)
        {
            if (playOnlyOnce && hasPlayed) return;

            audioSource.Play();
            hasPlayed = true;
        }
    }
}


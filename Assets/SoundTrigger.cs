using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioSource audioSource;       // Drag your AudioSource here
    public bool playOnlyOnce = false;
    public bool loopWhileInside = false;

    private bool hasPlayed = false;

    private void Awake()
    {
        if (loopWhileInside && audioSource != null)
        {
            audioSource.loop = true;      // Force loop mode on
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && audioSource != null)
        {
            if (loopWhileInside)
            {
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
            else
            {
                if (playOnlyOnce && hasPlayed) return;

                audioSource.Play();
                hasPlayed = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && audioSource != null && loopWhileInside)
        {
            audioSource.Stop();
        }
    }
}

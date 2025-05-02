using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 1f;
    public float resetDelay = 2f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource fallingSound;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isFalling = false;

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FallAndReset());
        }
    }

    private IEnumerator FallAndReset()
    {
        isFalling = true;

        yield return new WaitForSeconds(fallDelay);

        if (fallingSound != null)
            fallingSound.Play();

        rb.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(resetDelay);

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        isFalling = false;
    }
}

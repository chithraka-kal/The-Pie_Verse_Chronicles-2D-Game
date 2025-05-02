using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActive = false;
    private static Transform currentCheckpoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive && other.CompareTag("Player"))
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                currentCheckpoint = this.transform;
                respawn.SetRespawnPoint(currentCheckpoint);
                Debug.Log($"[CHECKPOINT] Activated: {gameObject.name}");
                isActive = true;
            }
        }
    }


    public float floatingSpeed = 1f;
    public float floatingHeight = 0.5f;
    private Vector3 startPosition;
    private void Start()
    {
        startPosition = transform.localPosition;
    }
    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatingSpeed) * floatingHeight;
        transform.localPosition = startPosition + new Vector3(0, yOffset, 0);
    }
}

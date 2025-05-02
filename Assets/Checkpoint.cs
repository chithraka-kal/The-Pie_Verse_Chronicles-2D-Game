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
}

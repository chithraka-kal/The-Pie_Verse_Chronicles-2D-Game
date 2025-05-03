using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    public Transform respawnPoint;              // Checkpoint position
    public GameObject gameOverPanel;            // Game Over UI Panel (not used here, set via PlayerPrefs)

    private Damageable damageable;              // Reference to Damageable component
    private Animator animator;                  // Reference to Animator

    private int maxHeartHP = 100;               // Max HP per heart
    public int totalHearts => damageable.MaxHealth / maxHeartHP; // Total heart containers

    void Start()
    {
        damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();

        // Ensure the game over panel is hidden at start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Fix for zero health at scene start
        if (damageable.Health <= 0)
        {
            damageable.Health = totalHearts * maxHeartHP;
            Debug.LogWarning("[PlayerRespawn] Health was 0 on start. Resetting to full.");
        }

        // Assign custom hit logic handler
        damageable.customHitLogic = CustomHitLogic;
    }

    private bool CustomHitLogic(int damage, Vector2 knockback)
    {
        if (!damageable.IsAlive) return false;

        int previousHealth = damageable.Health;
        int newHealth = Mathf.Max(previousHealth - damage, 0);

        int previousHearts = Mathf.CeilToInt(previousHealth / (float)maxHeartHP);
        int newHearts = Mathf.CeilToInt(newHealth / (float)maxHeartHP);

        bool lostHeart = newHearts < previousHearts;

        damageable.Health = newHealth;

        Debug.Log($"[HIT] Took {damage} damage. HP: {previousHealth} -> {newHealth} | Hearts: {previousHearts} -> {newHearts} | LostHeart: {lostHeart}");

        animator.SetTrigger(AnimationStrings.hitTrigger);
        damageable.LockVelocity = true;
        damageable.damageableHit?.Invoke(damage, knockback);
        CharacterEvents.characterDamaged.Invoke(gameObject, damage);

        if (lostHeart)
        {
            if (newHearts > 0)
            {
                Debug.Log($"[HEART LOST] {newHearts} hearts remaining. Respawning...");
                StartCoroutine(RespawnAfterDelay(1.5f));
            }
            else
            {
                Debug.Log("[HEART LOST] 0 hearts remaining.");
                StartCoroutine(TriggerGameOver());
            }
        }

        return true;
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        transform.position = respawnPoint.position;

        int remainingHearts = Mathf.CeilToInt(damageable.Health / (float)maxHeartHP);
        damageable.Health = remainingHearts * maxHeartHP;

        Debug.Log($"[RESPAWNED] Moved to checkpoint. HP reset to {damageable.Health}");
    }

    private IEnumerator TriggerGameOver()
    {
        Debug.Log("[PlayerRespawn] TriggerGameOver() CALLED!");

        yield return new WaitForSeconds(2f);

        // Set flag and ensure it’s saved before loading scene
        PlayerPrefs.SetInt("ShowGameOver", 1);
        PlayerPrefs.Save();
        yield return new WaitForEndOfFrame(); // Ensures flag is committed

        SceneManager.LoadScene("MainMenu");
    }

    public void SetRespawnPoint(Transform point)
    {
        respawnPoint = point;
        Debug.Log($"[RESPAWN POINT SET] New point: {point.name}");
    }
}

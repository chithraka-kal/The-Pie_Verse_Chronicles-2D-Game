using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerRespawn : MonoBehaviour
{
    public Transform respawnPoint;
    public GameObject gameOverPanel; // Assign in Inspector

    private Damageable damageable;
    private Animator animator;

    private int maxHeartHP = 100;
    public int totalHearts => damageable.MaxHealth / maxHeartHP;


    void Start()
    {
        damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        damageable.customHitLogic = CustomHitLogic;
    }

    private bool CustomHitLogic(int damage, Vector2 knockback)
    {
        if (!damageable.IsAlive) return false;

        int previousHealth = damageable.Health;
        int newHealth = Mathf.Max(previousHealth - damage, 0);

        // Correct heart calculation using ceiling to avoid premature loss
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
    yield return new WaitForSeconds(2f);

    // Set a flag to show Game Over panel in Main Menu
    PlayerPrefs.SetInt("ShowGameOver", 1);
    PlayerPrefs.Save();

    // Load Main Menu scene
    SceneManager.LoadScene("MainMenu");
}

    public void SetRespawnPoint(Transform point)
{
    respawnPoint = point;
    Debug.Log($"[RESPAWN POINT SET] New point: {point.name}");
}


    
}
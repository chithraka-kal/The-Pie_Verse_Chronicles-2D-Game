using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro; // For TMP_Text

public class PlayerRespawn : MonoBehaviour
{
    [Header("Checkpoint & Game Over")]
    public Transform respawnPoint;              // Checkpoint position
    public GameObject gameOverPanel;            // Game Over UI Panel (handled via PlayerPrefs)

    [Header("Messages")]
    public TMP_Text respawnMessageText;         // “Respawning…” message
    public TMP_Text savingMessageText;          // “Saving…” message

    private Damageable damageable;              // Reference to Damageable component
    private Animator animator;                  // Reference to Animator

    private int maxHeartHP = 100;               // HP per heart
    public int totalHearts => damageable.MaxHealth / maxHeartHP; // How many hearts you have

    void Start()
    {
        damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();

        // Hide both UI messages at start
        if (respawnMessageText != null) respawnMessageText.gameObject.SetActive(false);
        if (savingMessageText != null) savingMessageText.gameObject.SetActive(false);

        // Hide game over panel at start
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // If health somehow starts at zero, reset it
        if (damageable.Health <= 0)
        {
            damageable.Health = totalHearts * maxHeartHP;
            Debug.LogWarning("[PlayerRespawn] Health was 0 on start. Resetting to full.");
        }

        // Hook into custom hit logic
        damageable.customHitLogic = CustomHitLogic;
    }

    // Custom damage handling (heart loss, respawn, game over)
    private bool CustomHitLogic(int damage, Vector2 knockback)
    {
        if (!damageable.IsAlive) return false;

        int prevHP = damageable.Health;
        int newHP  = Mathf.Max(prevHP - damage, 0);

        int prevHearts = Mathf.CeilToInt(prevHP / (float)maxHeartHP);
        int newHearts  = Mathf.CeilToInt(newHP / (float)maxHeartHP);

        bool lostHeart = newHearts < prevHearts;
        damageable.Health = newHP;

        Debug.Log($"[HIT] HP: {prevHP}->{newHP} | Hearts: {prevHearts}->{newHearts} | LostHeart:{lostHeart}");

        // Trigger hit feedback
        animator.SetTrigger(AnimationStrings.hitTrigger);
        damageable.LockVelocity = true;
        damageable.damageableHit?.Invoke(damage, knockback);
        CharacterEvents.characterDamaged.Invoke(gameObject, damage);

        // On heart loss: respawn or game over
        if (lostHeart)
        {
            if (newHearts > 0)
                StartCoroutine(RespawnAfterDelay(1.5f));
            else
                StartCoroutine(TriggerGameOver());
        }

        return true;
    }

    // Show respawn message, wait, then teleport & restore HP
    private IEnumerator RespawnAfterDelay(float delay)
    {
        // Display “Respawning…” text
        if (respawnMessageText != null)
        {
            respawnMessageText.text = "Respawning...";
            respawnMessageText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(delay);

        // Teleport
        transform.position = respawnPoint.position;

        // Restore full HP for remaining hearts
        int remainingHearts = Mathf.CeilToInt(damageable.Health / (float)maxHeartHP);
        damageable.Health = remainingHearts * maxHeartHP;

        Debug.Log($"[RESPAWNED] Checkpoint reached. HP reset to {damageable.Health}");

        // Hide that message after 2s
        if (respawnMessageText != null)
            StartCoroutine(HideAfter(respawnMessageText, 2f));
    }

    // Game over flow: set flag, save, then load MainMenu
    private IEnumerator TriggerGameOver()
    {
        Debug.Log("[PlayerRespawn] TriggerGameOver() CALLED!");

        yield return new WaitForSeconds(2f);

        PlayerPrefs.SetInt("ShowGameOver", 1);
        PlayerPrefs.Save();
        yield return new WaitForEndOfFrame();

        SceneManager.LoadScene("MainMenu");
    }

    // Call this whenever you want to update the respawn point
    public void SetRespawnPoint(Transform point)
    {
        respawnPoint = point;
        Debug.Log($"[RESPAWN POINT SET] {point.name}");

        // Show “Saving…” message when checkpoint is updated
        if (savingMessageText != null)
        {
            savingMessageText.text = "Saving...";
            savingMessageText.gameObject.SetActive(true);
            StartCoroutine(HideAfter(savingMessageText, 2f));
        }
    }

    // Generic hide-after-X-seconds coroutine for any TMP_Text
    private IEnumerator HideAfter(TMP_Text textObj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        textObj.gameObject.SetActive(false);
    }
}

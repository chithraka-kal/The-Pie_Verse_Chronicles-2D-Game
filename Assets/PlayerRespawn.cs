using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    public Transform respawnPoint;
    private Damageable damageable;
    private Animator animator;

    private int maxHeartHP = 100;

    public int totalHearts => damageable.MaxHealth / maxHeartHP;

    void Start()
    {
        damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();

        damageable.customHitLogic = CustomHitLogic;
    }

    private bool CustomHitLogic(int damage, Vector2 knockback)
    {
        if (!damageable.IsAlive) return false;

        int currentHealth = damageable.Health;
        int newHealth = Mathf.Max(currentHealth - damage, 0);

        int oldHeart = currentHealth / maxHeartHP;
        int newHeart = newHealth / maxHeartHP;

        bool lostHeart = newHeart < oldHeart;

        damageable.Health = newHealth;

        // Handle knockback + feedback
        animator.SetTrigger(AnimationStrings.hitTrigger);
        damageable.LockVelocity = true;
        damageable.damageableHit?.Invoke(damage, knockback);
        CharacterEvents.characterDamaged.Invoke(gameObject, damage);

        if (lostHeart)
        {
            if (damageable.Health > 0)
            {
                RespawnPlayer();
            }
            else
            {
                GameOver();
            }
        }

        return true;
    }

    private void RespawnPlayer()
    {
        transform.position = respawnPoint.position;

        // Subtract 1 heart and restore remaining heart's HP
        int remainingHearts = Mathf.Max((damageable.Health / maxHeartHP), 1);
        damageable.Health = remainingHearts * maxHeartHP;
    }

    private void GameOver()
    {
        Debug.Log("Game Over. Restarting...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

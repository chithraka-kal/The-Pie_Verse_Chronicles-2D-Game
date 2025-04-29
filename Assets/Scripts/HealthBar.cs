using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("UI References")]
    public Slider healthSlider;
    public TMP_Text healthBarText;

    

    [Header("Heart UI")]
    public GameObject heartContainer;     // Parent with Horizontal Layout Group
    public GameObject heartPrefab;        // Prefab with Image component

    public float heartSpacing = 100f;
    private List<GameObject> hearts = new List<GameObject>();

    private const int healthPerHeart = 100;
    private Damageable playerDamageable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found in scene.");
            return;
        }

        playerDamageable = player.GetComponent<Damageable>();
    }

    private void Start()
    {
        InitializeHearts(playerDamageable.MaxHealth);
        UpdateHealthBar(playerDamageable.Health, playerDamageable.MaxHealth);
    }

    private void OnEnable()
    {
        if (playerDamageable != null)
            playerDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        if (playerDamageable != null)
            playerDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

private void InitializeHearts(int maxHealth)
{
    int heartCount = Mathf.CeilToInt(maxHealth / (float)healthPerHeart);

    //float spacing = 40f; // Space between hearts (adjust for your icon size)

    while (hearts.Count < heartCount)
    {
        GameObject newHeart = Instantiate(heartPrefab, heartContainer.transform);
        
        RectTransform rect = newHeart.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(hearts.Count * heartSpacing, 0); // Manual position

        hearts.Add(newHeart);
    }

    // Optional: Remove extra hearts if maxHealth decreases
    while (hearts.Count > heartCount)
    {
        Destroy(hearts[hearts.Count - 1]);
        hearts.RemoveAt(hearts.Count - 1);
    }
}



    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        UpdateHealthBar(newHealth, maxHealth);
    }

private void UpdateHealthBar(int currentHealth, int maxHealth)
{
    // Clamp to valid range
    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

    int totalHearts = Mathf.CeilToInt(maxHealth / (float)healthPerHeart);

    // Determine how much HP is in the current (last visible) heart
    int currentHeartIndex = Mathf.Clamp((currentHealth - 1) / healthPerHeart, 0, totalHearts - 1);
    int currentHeartHP = currentHealth - (currentHeartIndex * healthPerHeart);
    currentHeartHP = Mathf.Clamp(currentHeartHP, 0, healthPerHeart); // Just in case

    // Update health slider and text to show current heart's HP
    healthSlider.value = (float)currentHeartHP / healthPerHeart;
    healthBarText.text = "HP " + currentHeartHP + " / " + healthPerHeart;

    // Add hearts if needed
    if (hearts.Count < totalHearts)
    {
        InitializeHearts(maxHealth);
    }

    // Show/hide hearts based on health
    for (int i = 0; i < hearts.Count; i++)
    {
        int heartStartHP = i * healthPerHeart;
        bool isHeartVisible = currentHealth > heartStartHP;
        hearts[i].SetActive(isHeartVisible);
    }
}


}

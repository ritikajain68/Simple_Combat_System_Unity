using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("Character Health Settings")]
    public bool isAlive = true;
    public float health;
    public float maxHealth = 100f;

    [Header("Health Bar UI")]
    public Slider healthSlider;
    public Image fillImage;
    public Vector3 offset = new Vector3(0, 2f, 0);
    private Camera mainCamera;

    public event Action<float> OnHealthChanged;

    private void Start()
    {
        mainCamera = Camera.main;        
        health = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }
    }

    void Update()
    {
        if (healthSlider != null)
        {
            healthSlider.transform.position = transform.position + offset;
            healthSlider.transform.LookAt(mainCamera.transform);

            // float normalizedHealth = Mathf.Clamp01(health / maxHealth);
            // healthSlider.value = health;

            // if (fillImage != null)
            // {
            //     fillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, normalizedHealth);
            // }
        }
    }

    public void TakeDamage(float amount, PlayerSetup attacker)
    {
        Debug.Log($"{gameObject.name} took {amount} damage from {attacker?.name}");

        if (!CheckIfPlayerAlive()) return;

        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthBarUI();
        OnHealthChanged?.Invoke(health);

        if (health <= 0f)
        {
            // Get the correct setup reference — should be PlayerSetup
            PlayerSetup setup = GetComponent<PlayerSetup>();
            if (setup != null)
            {
                setup.isAlive = false;

                if (attacker?.playerStatsData != null)
                {
                    attacker.playerStatsData.killCount++;
                    Debug.Log($"{attacker.playerStatsData.playerName} now has {attacker.playerStatsData.killCount} kills.");
                }

                attacker?.FindNewTarget();
            }
            else
            {
                Debug.LogWarning("PlayerSetup component not found on this GameObject.");
            }

            gameObject.SetActive(false);
            if (transform.parent != null)
            {
                transform.parent.gameObject.SetActive(false);
            }

            PlayerSpawner.Instance.CheckBattleState();
        }
    }


    public void Heal(float amount)
    {
        if (!CheckIfPlayerAlive()) return;

        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthBarUI();
        OnHealthChanged?.Invoke(health);
    }

    public void ResetHealth()
    {
        health = maxHealth;
        UpdateHealthBarUI();
        OnHealthChanged?.Invoke(health);
    }

    public bool CheckIfPlayerAlive()
    {
        isAlive = health > 0f;
        return isAlive;
    }

    private void UpdateHealthBarUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = health;

            // float normalized = Mathf.Clamp01(health / maxHealth);
            // if (fillImage != null)
            //     fillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, normalized);
        }
    }
    private void HandleDeath()
    {
        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        OnHealthChanged = null;
    }
}
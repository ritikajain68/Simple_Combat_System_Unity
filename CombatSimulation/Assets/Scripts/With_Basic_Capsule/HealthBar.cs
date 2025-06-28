using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBar : MonoBehaviour
{
    [Header("Character Health Settings")]
    public bool isAlive = true;
    public float health;
    public float maxHealth = 100f;

    [Header("Health Bar UI")]
    public Slider healthBarSlider;
    public Vector3 offset = new Vector3(0, 2f, 0);
    private Camera mainCamera;
    public event Action<float> OnHealthChanged;    

    void Start()
    {
        health = maxHealth;
        mainCamera = Camera.main;

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = health;
        }
    }

    void Update()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.transform.position = transform.position + offset;
            healthBarSlider.transform.LookAt(mainCamera.transform);
        }
    }

    public void TakeDamage(float amount, CharacterSetup attacker)
    {
        if (!CheckIfPlayerAlive()) return;

        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthBarUI();
        OnHealthChanged?.Invoke(health);

        if (health <= 0)
        {
            CharacterSetup setup = GetComponent<CharacterSetup>();
            if (setup != null)
            {
                setup.isAlive = false;
                if (attacker != null && attacker.characterStatsData != null)
                {
                    attacker.characterStatsData.KillCount++;
                }
                attacker?.FindNewTarget();
            }

            gameObject.SetActive(false);
            Spawner.Instance.CheckBattleState();
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
        if (healthBarSlider != null)
        {
            healthBarSlider.value = health;
        }
    }

    private void HandleDeath()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        OnHealthChanged = null;
    }
}

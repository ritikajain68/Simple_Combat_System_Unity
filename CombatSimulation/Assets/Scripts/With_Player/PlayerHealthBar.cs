using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("References")]
    public Slider healthSlider;
    public PlayerSetup player;
    public Image fillImage;

    [Header("Health Colors")]
    public Color fullHealthColor = Color.green;
    public Color lowHealthColor = Color.red;

    private void Start()
    {
        if (player == null)
            player = GetComponentInParent<PlayerSetup>();

        if (healthSlider != null)
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = 1f;

            if (fillImage == null)
                fillImage = healthSlider.fillRect.GetComponent<Image>();

            // Make sure health is correctly initialized
            float normalizedHealth = Mathf.Clamp01(player.health / player.maxHealth);
            healthSlider.value = normalizedHealth;

            fillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, normalizedHealth);
        }
        Debug.Log($"Health at Start: {player.health}/{player.maxHealth}");
    }
    
    private void Update()
    {
        if (player == null || !player.isAlive || healthSlider == null) return;

        float normalizedHealth = Mathf.Clamp01(player.health / player.maxHealth);
        healthSlider.value = normalizedHealth;

        if (fillImage != null)
            fillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, normalizedHealth);

        // Rotate to face player forward direction
        Vector3 forward = player.transform.forward;
        forward.y = 0f;
        if (forward != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(forward);


        Debug.Log($"Health: {player.health} / {player.maxHealth}, Normalized: {normalizedHealth}");
    }
}

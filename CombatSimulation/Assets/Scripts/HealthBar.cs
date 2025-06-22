using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fill;
    private CharacterSetup character;

    void Start()
    {
        character = GetComponentInParent<CharacterSetup>();
    }

    void Update()
    {
        if (character)
            fill.fillAmount = character.health / 100f;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image fill;
    private PlayerSetup player;

    void Start()
    {
        player = GetComponentInParent<PlayerSetup>();
    }

    void Update()
    {
        if (player)
            fill.fillAmount = player.health / 100f;
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI winnerText;
    public GameObject winnerCanvas;

    void Awake()
    {
        Instance = this;
        winnerText.text = "";
        winnerCanvas.SetActive(false);
    }

    public void ShowWinner(string name)
    {
        winnerCanvas.SetActive(true);
        winnerText.text = $"{name} Wins!";
    }
}

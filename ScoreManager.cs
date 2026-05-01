using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public Transform player;
    public TMP_Text scoreText;

    
    private double currentScore;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

      void UpdateUI()
    {
        if (scoreText == null) return;
        scoreText.text = currentScore.ToString();
    }

    public static void AddPlatformScore(double amount = 1.0)
    {
        Instance.currentScore += (int)amount;
        Instance.UpdateUI();
    }

    public void AddBreakBonus(int amount = 2)
    {
        currentScore += amount;
        UpdateUI();
    }

    public void AddPowerupScore(int amount = 1)
    {
        currentScore += amount;
        UpdateUI();
    }
}
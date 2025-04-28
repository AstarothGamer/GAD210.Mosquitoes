using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class MosquitoHealth : MonoBehaviour
{
    [Header("Health Bar")]
    public float maxHealth = 100;
    public float currentHP;
    public float chipSpeed = 2f;
    public float secondsToEmptyHealth = 60;
    public float secondsToFullHealth = 30;
    public Image frontHealthBar;
    public Image backHealthBar;
    private float uiHP;

    [Header("Exp Bar")]
    public Image ExpBar;
    private float uiExp;

    public Abilities player;
    public Stats stats;
    public float changeFactor = 6f;
    public GameObject deathPanel;

    void Start()
    {
        currentHP = maxHealth;
        uiExp = stats.currentExp;
    }

    void Update()
    {
        UpdateHP();
        UpdateExp();

        if (currentHP <= 0)
        {
            GameOver();
        }
    }

    void UpdateHP()
    {
        if(player.isBiting & currentHP < maxHealth)
        {
            currentHP += 100 / secondsToFullHealth * Time.deltaTime;
        }
        else if(currentHP > 0 & !player.isBiting)
        {
            currentHP -= 100 / secondsToEmptyHealth * Time.deltaTime;  
        }
        uiHP = Mathf.Lerp(uiHP, currentHP, Time.deltaTime * changeFactor);
        frontHealthBar.fillAmount = uiHP / 100;
    }

    void UpdateExp()
    {
        uiExp = Mathf.Lerp(uiExp, stats.currentExp, Time.deltaTime * changeFactor);
        ExpBar.fillAmount = uiExp / stats.expToNextLevel;
    }

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Game Over!");
        deathPanel.SetActive(true);
        Time.timeScale = 0f; // Stop the game
    }
}

using System.Collections;
using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private TMP_Text levelUpNotificationText;
    [SerializeField] private TMP_Text statsNotificationText;  
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    public void AddExperience(int exp)
    {
        currentExp += exp;
        Debug.Log("Gained exp: " + exp + ". Current exp: " + currentExp);
        UpdateStatsUI();

        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    void Start()
    {
        UpdateStatsUI();   
    }

    void LevelUp()
    {
        level++;
        currentExp -= expToNextLevel;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.2f);

        Debug.Log("Level up " + level + ".");
        if (levelUpNotificationText != null)
        {
            levelUpNotificationText.text = "Congrats! You leveled up " + level + ".";
            StartCoroutine(ClearNots());
        }
        UpdateStatsUI();
    }

    void UpdateStatsUI()
    {
        if (statsNotificationText != null)
        {
            statsNotificationText.text = "Level: " + level + "\nExp: " + currentExp + " / " + expToNextLevel;
        }
    }

    IEnumerator ClearNots()
    {
        yield return new WaitForSeconds(5f);
        levelUpNotificationText.text = "";
    }
}

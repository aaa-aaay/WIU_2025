using UnityEngine;
using UnityEngine.UI;

public class StatBars : MonoBehaviour
{
    public Slider healthSlider;
    public Slider manaSlider;
    public PlayerStats playerStats;

    private int lastHealth;
    private int lastMana;

    void Start()
    {
        healthSlider.maxValue = playerStats.MaxHealth;
        manaSlider.maxValue = playerStats.MaxMana;

        UpdateStats();
    }

    void Update()
    {
        if (playerStats.Health != lastHealth || playerStats.Mana != lastMana)
        {
            UpdateStats();
        }
    }

    public void UpdateStats()
    {
        lastHealth = playerStats.Health;
        lastMana = playerStats.Mana;
        healthSlider.value = lastHealth;
        manaSlider.value = lastMana;
    }
}

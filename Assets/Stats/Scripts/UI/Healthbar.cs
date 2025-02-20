using UnityEngine;
using UnityEngine.UI;

public class StatBars : MonoBehaviour
{
    public Slider healthSlider;
    public Slider manaSlider;
    public PlayerStats playerStats;

    void Start()
    {
        healthSlider.maxValue = playerStats.MaxHealth;
        healthSlider.value = playerStats.Health; 
        manaSlider.maxValue = playerStats.MaxMana;
        healthSlider.value = playerStats.Mana;
    }

    public void UpdateStats()
    {
        healthSlider.value = playerStats.Health;
        healthSlider.value = playerStats.Mana;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private List<StatEntry> startStats = new List<StatEntry>(); // set stats in the inspector
    private Dictionary<SkillSO.UpgradeType, int> stats = new Dictionary<SkillSO.UpgradeType, int>(); // store the stats in a dictionary

    PlayerSkillManager playerSkillManager;
    private bool unlockedHeal = false;
    private bool unlockedBarrier = false;
    private int healAmt;
    private int barrierAmt;

    [Header ("Cooldowns")]
    // cd timer
    [SerializeField] private float healCd = 30f;
    [SerializeField] private float barrierCd = 30f;

    [SerializeField] private Image healCdImage; 
    [SerializeField] private Image barrierCdImage; 

    private float healCdRemaining = 0f;
    private float barrierCdRemaining = 0f;

    public int Strength => stats[SkillSO.UpgradeType.Strength];
    public int Speed => stats[SkillSO.UpgradeType.Speed];
    public int Defence { get; private set; }
    public int Health { get; private set; }
    public int Mana { get; private set; }
    public int Gold { get; private set; }
    public int MaxHealth => stats[SkillSO.UpgradeType.MaxHealth];
    public int MaxMana => stats[SkillSO.UpgradeType.MaxMana];

    public event Action<int> OnMoneyAmtChanged;

    private void Awake()
    {
        playerSkillManager = GetComponent<PlayerSkillManager>();

        // initialize variables with values set in inspector
        foreach (var entry in startStats)
        {
            stats[entry.statType] = entry.value;
        }
        Health = MaxHealth;
        Mana = MaxMana;


    }
    private void Start()
    {
        Gold = 70; //tempory starting gold for now
        OnMoneyAmtChanged?.Invoke(Gold); //send the init gold to ui
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) { TakeDamage(10); } // for testing
        if (Input.GetKeyDown(KeyCode.I)) { BarrierSkill(); }
        if (Input.GetKeyDown(KeyCode.O)) { HealSkill(); }

        if (healCdRemaining > 0)
        {
            healCdRemaining -= Time.deltaTime;
            healCdImage.fillAmount = healCdRemaining / healCd; // % of cd left
        }
        if (barrierCdRemaining > 0)
        {
            barrierCdRemaining -= Time.deltaTime;
            barrierCdImage.fillAmount = barrierCdRemaining / barrierCd; // % of cd left
        }
        if (Input.GetKeyDown(KeyCode.P)) { TakeDamage(10); }
        if (Input.GetKeyDown(KeyCode.L)) { UseMana(10); }
    }

    public void UpgradeStat(SkillSO type)
    {
        if (type.upgradeType == SkillSO.UpgradeType.Heal)
        {
            unlockedHeal = true;
            healCdImage.fillAmount = healCdRemaining / healCd;
            healAmt = Mathf.RoundToInt((Health / 100) * type.statChange);
        }
        else if (type.upgradeType == SkillSO.UpgradeType.Barrier)
        {
            unlockedBarrier = true;
            barrierCdImage.fillAmount = barrierCdRemaining / barrierCd;
            barrierAmt = Mathf.RoundToInt((Defence / 100f) * type.statChange);
        }
        else if (stats.ContainsKey(type.upgradeType))
        {
            stats[type.upgradeType] += type.statChange;
            if (type.upgradeType == SkillSO.UpgradeType.MaxHealth) // increase max health
                Health += type.statChange; // increase current health
        }
    }

    public void TakeDamage(int amt)
    {
        int damage = Mathf.Max(amt - Defence, 0); // ensure no negative dmg
        Health = Mathf.Max(Health - damage, 0); // prevents negative health
    }

    public void HealPotion(int amt)
    {
        Health += amt;
        Health = Mathf.Min(Health, MaxHealth);
    }

    public void ManaPotion(int amt)
    {
        Mana += amt;
        Mana = Mathf.Min(Mana, MaxMana);

    }
    public bool UseMana(int amt)
    {
        int manaCost = Mathf.Max(amt, 0); // ensure no negative
        if (Mana - manaCost < 0) { return false; }
        else Mana = Mathf.Max(Mana - manaCost, 0);
        return true;

    }

    public void UseGold(int amt)
    {
        if (Gold >= amt)
        {
            Gold -= amt;
            OnMoneyAmtChanged.Invoke(Gold); //Update UI
}
        else
        {
            Debug.Log("Player only has " + Gold);
        }
    }

    public void HealSkill()
    {
        if (unlockedHeal)
        {
            if (healCdRemaining <= 0)
            {
                Health += healAmt;
                Health = Mathf.Min(Health, MaxHealth);
                healCdRemaining = healCd; // reset cd

                Debug.Log("Heal skill used");
            }
        }
        else
        {
            Debug.Log("Heal has not been unlocked yet");
        }
    }

    public void BarrierSkill()
    {
        if (unlockedBarrier)
        {
            if (barrierCdRemaining <= 0)
            {
                StartCoroutine(BarrierEffect()); // gain barrier for x duration
                barrierCdRemaining = barrierCd; // reset cd
            }
        }
        else
        {
            Debug.Log("Barrier has not been unlocked yet");
        }
    }

    private IEnumerator BarrierEffect()
    {
        int originalDefence = Defence; // store def
        Defence += barrierAmt; // barrier

        yield return new WaitForSeconds(3f); 

        Defence = originalDefence; // reset def
    }
    public bool HasEnoughGold(int requiredGold)
    {
        return (Gold >= requiredGold);
    }

    public void UseGoldForSkills(SkillSO type)
    {
        Gold -= type.goldRequired;
    }
}

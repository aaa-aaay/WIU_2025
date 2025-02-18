using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private List<StatEntry> startStats = new List<StatEntry>(); // set stats in the inspector
    private Dictionary<SkillSO.UpgradeType, int> stats = new Dictionary<SkillSO.UpgradeType, int>();
    private bool unlockedHeal = false;
    private bool unlockedBarrier = false;
    private int healAmt;
    private int barrierAmt;

    public int Strength => stats[SkillSO.UpgradeType.Strength];
    public int Speed => stats[SkillSO.UpgradeType.Speed];
    public int Defence { get; private set; }
    public int Health { get; private set; }
    public int Mana => stats[SkillSO.UpgradeType.Mana];
    public int Gold => stats[SkillSO.UpgradeType.Gold];
    public int MaxHealth => stats[SkillSO.UpgradeType.MaxHealth];
    public bool UnlockedHeal => unlockedHeal;
    public bool UnlockedBarrier => unlockedBarrier;

    private void Start()
    {
        // initialize variables with values set in inspector
        foreach (var entry in startStats)
        {
            stats[entry.statType] = entry.value;
        }
        Health = MaxHealth;
    }

    public void UpgradeStat(SkillSO type)
    {
        if (type.upgradeType == SkillSO.UpgradeType.Heal)
        {
            unlockedHeal = true;
            healAmt = Mathf.RoundToInt((Health / 100) * type.statChange);
        }
        else if (type.upgradeType == SkillSO.UpgradeType.Barrier)
        {
            unlockedBarrier = true;
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

    public void Heal()
    {
        Health += healAmt;
    }

    public void Barrier()
    {
        StartCoroutine(BarrierEffect()); // gain barrier for x duration
    }

    private IEnumerator BarrierEffect()
    {
        int originalDefence = Defence; // store def
        Defence += barrierAmt; // barrier

        yield return new WaitForSeconds(3f); 

        Defence = originalDefence; // reset def
    }

    //private int GetStat(SkillSO.UpgradeType statType)
    //{
    //    return stats.ContainsKey(statType) ? stats[statType] : 0;
    //}
}

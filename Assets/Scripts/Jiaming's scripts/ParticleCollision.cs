using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    public int damage = 10; // Damage dealt by the rocks

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("collides with smth??");
        if (other.gameObject.GetComponent<EnemyHealth>())
        {
            TakeDamage(damage);
        }
    }

    private void TakeDamage(int damageAmount)
    {
        // Implement your enemy's health system here
        Debug.Log($"Enemy took {damageAmount} damage from falling rocks!");
        // Example: Decrease health, trigger animations, or destroy the enemy
    }
}

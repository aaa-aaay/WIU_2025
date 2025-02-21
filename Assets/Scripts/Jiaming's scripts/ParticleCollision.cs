using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    public int damage = 40; // Damage dealt by the rocks

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("collides with smth??");
        Debug.Log(other.gameObject.name);
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}

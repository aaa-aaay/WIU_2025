using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFeedback : MonoBehaviour
{

    [SerializeField] private float lifetime = 10f;

    private PlayerStats playerStats;

    void Start()
    {
        Destroy(gameObject, lifetime);
        GameObject obj = GameObject.Find("player");
        playerStats = obj.GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Trigger damage taken by player
            Debug.Log("BOOOM");
            Destroy(gameObject);

            playerStats.TakeDamage(25);
        }
    }
}
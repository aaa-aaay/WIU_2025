using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFeedback : MonoBehaviour
{

    [SerializeField] private float lifetime = 10f; 

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Trigger damage taken by player
            Debug.Log("BOOOM");
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackHandler : MonoBehaviour
{
    [SerializeField] private Transform playerPos;   
    [SerializeField] private GameObject arrowPrefab;     
    [SerializeField] private Transform shootPoint;       
    [SerializeField] private float arrowSpeed = 10f;      


    void ShootArrow()
    {
        // Calculate the direction using Vector3
        Vector3 direction = (playerPos.position - shootPoint.position).normalized;

        // Instantiate the arrow with a rotation that faces the direction of travel
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.LookRotation(direction));

        // Get the Rigidbody component and apply velocity
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * arrowSpeed;
        }
    }
}

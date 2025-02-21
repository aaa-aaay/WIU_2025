using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] SphereCollider[] colliders;

    private PlayerStats playerStats;

    void Start()
    {
        GameObject obj = GameObject.Find("player");
        playerStats = obj.GetComponent<PlayerStats>();
    }
    public void EnableCollider(int index)
    {
        SphereCollider collider = colliders[index];
        collider.enabled = true;

        LayerMask layer = LayerMask.GetMask("Player");

        Collider[] hitcolliders = Physics.OverlapSphere(collider.transform.position, collider.radius, layer);
        for (int i = 0; i < hitcolliders.Length; i++)
        {
            collider.enabled = false;
            //Perform damage on other objects
            Debug.Log("BOMBA");
            playerStats.TakeDamage(25);

        }
    }
}

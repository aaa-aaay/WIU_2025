using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] SphereCollider[] swordCollider;

    public void EnableCollider(int index)
    {
        SphereCollider collider = swordCollider[index];
        collider.enabled = true;

        LayerMask layer = LayerMask.GetMask("Player");

        Collider[] hitcolliders = Physics.OverlapSphere(collider.transform.position, collider.radius, layer);
        for (int i = 0; i < hitcolliders.Length; i++)
        {
            collider.enabled = false;
            //Perform damage on other objects
            Debug.Log("BOMBA");
        }
    }
}

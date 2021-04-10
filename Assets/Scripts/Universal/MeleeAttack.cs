using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {
    public LayerMask targetableLayer;
    public SphereCollider attackCollider;
    public float attackDamage;

    private void OnTriggerEnter(Collider other) {
        if (1 << other.gameObject.layer == targetableLayer.value) { // Layers use bitshifting for performance reasons
            // Apply Damage
            Debug.Log(targetableLayer.ToString() + "Hit!");
            other.gameObject.GetComponent<StatusManager>().ApplyDamage(attackDamage);
        }
    }
}
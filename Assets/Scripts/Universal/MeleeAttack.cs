using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {
    public LayerMask targetableLayer;
    public Collider attackCollider;
    public float attackDamage;

    private void OnTriggerEnter(Collider other) {
        // Layers use bitshifting for performance reasons (currently, there seems to be issues with multiple layers)
        if (1 << other.gameObject.layer == targetableLayer.value) {
            // Apply Damage
            Debug.Log(targetableLayer.ToString() + "Hit!");
            other.transform.root.GetComponent<StatusManager>().ApplyDamage(attackDamage);
        }
    }
}
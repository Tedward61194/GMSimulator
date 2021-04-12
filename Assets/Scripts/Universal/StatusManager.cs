using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should this all be handled server side?
public class StatusManager : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(float damage) {
        currentHealth -= damage;

        if (currentHealth <= 0) {
            Die();
        }
    }
    void Die() {
        Debug.Log(gameObject.name + " died");
    }
}
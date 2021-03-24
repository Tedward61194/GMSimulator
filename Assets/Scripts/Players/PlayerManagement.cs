using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    void Start() {
        currentHealth = maxHealth;
        HideThirdPersonBody();
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

    void HideThirdPersonBody() {
        if (transform.Find("Body") != null) {
            GameObject body = transform.Find("Body").gameObject;
            var bodyParts = GetComponentsInChildren<Transform>();
            foreach (Transform bodyPart in bodyParts) {
                bodyPart.gameObject.layer = LayerMask.NameToLayer("ThirdPersonVisible");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour, ICharacterController {

    public PaladinAnimationStateController animationController;

    void Start() {
        // Initialize this script as controller in StatusManager
        GetComponentInParent<StatusManager>().controllerScript = this.GetComponent<PlayerManagement>();

        animationController = GetComponentInParent<PaladinAnimationStateController>();
        HideThirdPersonBody();
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

    public void Die() {
        animationController.Die();
    }
}

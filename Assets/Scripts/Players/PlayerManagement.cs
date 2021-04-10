using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    void Start() {
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
}

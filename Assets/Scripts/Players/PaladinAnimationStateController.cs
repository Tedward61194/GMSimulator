using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PaladinAnimationStateController : MonoBehaviour {
    Animator animator;

    void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetIsRunning(bool value) {
        animator.SetBool("isRunning", value);
    }

    public void AttackOne() {
        GetComponentInParent<NetworkAnimator>().SetTrigger("attackOneTrigger");
    }
}

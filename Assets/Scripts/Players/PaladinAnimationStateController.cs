using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PaladinAnimationStateController : MonoBehaviour {
    NetworkPlayer networkPlayer;
    NetworkAnimator networkAnimator;
    Animator animator;

    public void Start() {
        networkPlayer = GetComponent<NetworkPlayer>();
        networkAnimator = GetComponent<NetworkAnimator>();
        animator = GetComponent<Animator>();
    }

    public void SetIsRunning(bool value) {
        animator.SetBool("isRunning", value);
    }

    public void AttackOne() {
        networkAnimator.SetTrigger("attackOneTrigger");
    }

    public void Die() {
        networkAnimator.SetTrigger("dieTrigger");
    }
}

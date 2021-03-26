using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PaladinAnimationStateController : MonoBehaviour {
    [SerializeField] GameObject bodyToAnimate;

    NetworkPlayer networkPlayer;
    NetworkAnimator networkAnimator;
    Animator animator;

    public void Init() {
        networkPlayer = GetComponentInParent<NetworkPlayer>();
        networkAnimator = GetComponent<NetworkAnimator>();
        animator = networkAnimator.animator;
    }

    public void SetIsRunning(bool value) {
        animator.SetBool("isRunning", value);
    }

    public void AttackOne() {
        networkAnimator.SetTrigger("attackOneTrigger");
    }
}

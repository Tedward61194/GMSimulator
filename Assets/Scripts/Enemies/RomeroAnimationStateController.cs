using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RomeroAnimationStateController : NetworkBehaviour {
    Animator animator;
    private NetworkAnimator networkAnimator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        networkAnimator = GetComponent<NetworkAnimator>();
    }

    public void IsWalking() {
        animator.SetBool("isWalking", true);
    }

    public void IsIdle() {
        animator.SetBool("isWalking", false);
    }

    public void Attack() {
        networkAnimator.SetTrigger("isAttacking");
    }
}

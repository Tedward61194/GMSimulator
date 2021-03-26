using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PaladinAnimationStateController : MonoBehaviour {
    //[SerializeField] GameObject bodyToAnimate;

    NetworkPlayer networkPlayer;
    NetworkAnimator networkAnimator;
    Animator animator;
    //RuntimeAnimatorController runtimeAnimatorController;

    public void Start() {
        networkPlayer = GetComponent<NetworkPlayer>();
        networkAnimator = GetComponent<NetworkAnimator>();
        animator = GetComponent<Animator>();
        //runtimeAnimatorController = GetComponent<RuntimeAnimatorController>();
        //networkPlayer.GetComponent<Animator>().runtimeAnimatorController = runtimeAnimatorController;
        
    }

    public void SetIsRunning(bool value) {
        animator.SetBool("isRunning", value);
    }

    public void AttackOne() {
        networkAnimator.SetTrigger("attackOneTrigger");
    }
}

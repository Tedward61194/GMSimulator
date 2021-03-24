using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class EnemyController1 : NetworkBehaviour
{
    public float attackCooldown;
    public bool attackReady = true;
    public LayerMask playerLayer;
    public Transform attackPoint;
    public float attackRange;
    public float attackDamage;

    [SerializeField] Transform destination;

    NavMeshAgent navMeshAgent;
    RomeroAnimationStateController animationController;
    NetworkIdentity networkIdentity;

    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        animationController = GetComponent<RomeroAnimationStateController>();
        networkIdentity = GetComponentInParent<NetworkIdentity>();
        if (navMeshAgent == null) {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        }
        else {
            if (networkIdentity.isServer) {
                FindDestination();
                SetDestination();
            }
        }
    }

    void Update() {
        if (networkIdentity.isServer) {
            // Do I need to move
            if (!navMeshAgent.pathPending) {
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) {
                        // Done Moveing
                        animationController.IsIdle();
                        if (attackReady) {
                            Attack();
                        }
                    }
                }
            }
        }
    }

    public void FindDestination() {
        // Pick a random player to target
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject target = players[Random.Range(0, players.Length)];
        destination = target.transform;
    }

    private void SetDestination() {
        if (destination != null) {
            Vector3 targetVector = destination.transform.position;
            navMeshAgent.SetDestination(targetVector);
            animationController.IsWalking();
        }
    }
    
    private void Attack() {
        // Play attack animation
        animationController.Attack();
        attackReady = false;
        StartCoroutine("AttackCooldown");

        // Detect players in range
        Collider[] playersHit = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

        // Apply Damage
        foreach (Collider player in playersHit) {
            player.GetComponent<PlayerManagement>().ApplyDamage(attackDamage);
        }
    }

    public IEnumerator AttackCooldown() {
        yield return new WaitForSeconds(attackCooldown);
        attackReady = true;
    }
}

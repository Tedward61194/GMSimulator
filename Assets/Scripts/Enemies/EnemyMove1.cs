using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove1 : MonoBehaviour
{
    [SerializeField] Transform destination;

    NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        if (navMeshAgent == null) {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        }
        else {
            FindDestination();
            SetDestination();
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
        }
    }
    
}

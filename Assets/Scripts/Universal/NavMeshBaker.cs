using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshBaker : MonoBehaviour
{
    [SerializeField] NavMeshSurface[] navMeshSurfaces;

    void Awake()
    {
        BuildAllNavMeshes();
    }

    public void BuildAllNavMeshes() {
        foreach (NavMeshSurface navMesh in navMeshSurfaces) {
            navMesh.BuildNavMesh();
        }
    }
}

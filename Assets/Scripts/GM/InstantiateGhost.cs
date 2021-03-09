using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateGhost : MonoBehaviour {
    public void InstantiateObject(GameObject ghost) {
        Instantiate(ghost);
    }
}

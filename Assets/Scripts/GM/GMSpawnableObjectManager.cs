using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GMSpawnableObjectManager : MonoBehaviour {

    public Dictionary<GameObject, GameObject> GhostCorporealKvP = new Dictionary<GameObject, GameObject>();

    // Unity dosn't seem to let me setup KvPs in inspector, so this'll have to do
    [SerializeField] List<GameObject> ghosts;
    [SerializeField] List<GameObject> corporealObject;

    bool ghostSpawnedFlag;
    int activeGhostIndex;

    public void Start() {
        // Initialize GhostCorporealKvP
        for (int i = 0; i < ghosts.Count; i++) {
            GhostCorporealKvP.Add(ghosts[i], corporealObject[i]);
        }
    }

    public void Update() {
        if (ghostSpawnedFlag) {
            if (Input.GetMouseButtonDown(0)) {
                GetComponentInParent<NetworkPlayer>().CmdSpawnCorporealObject(activeGhostIndex);
            }
            if (Input.GetKey(KeyCode.Escape)) {
                ghostSpawnedFlag = false;
            }
        }
    }

    public void SpawnGhost(GameObject ghost) {
        activeGhostIndex = ghosts.IndexOf(ghost);
        GetComponentInParent<NetworkPlayer>().CmdSpawnGhost(activeGhostIndex);
        ghostSpawnedFlag = true;
        
    }

}

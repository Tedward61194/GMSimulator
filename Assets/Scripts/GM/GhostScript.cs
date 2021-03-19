using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GhostScript : NetworkBehaviour {
    public LayerMask GMPlaceableLayer;

    Vector3 hitPos;
    RaycastHit hit;

    void Start()
    {
        if (hasAuthority) {
            UpdateMousePosition(connectionToClient);
        }
    }

    void Update()
    {
        if (hasAuthority) {
            UpdateMousePosition(connectionToClient);
            CmdMove();

            if (Input.GetKey(KeyCode.Escape)) {
                CmdDestroyGhost();
            }
        }
    }

    public void UpdateMousePosition(NetworkConnection sender) {
        Ray ray = GameObject.FindGameObjectWithTag("GMCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~GMPlaceableLayer)) {
            CmdRegisterHitPosition(hit.point.ToString());
        }
    }

    [Command]
    public void CmdRegisterHitPosition(string hitPosString) {
        hitPos = TedsUtilities.ToVector3(hitPosString);
    }

    [Command]
    public void CmdMove() {
        transform.position = hitPos;
    }

    [Command]
    public void CmdDestroyGhost() {
        NetworkServer.Destroy(this.gameObject);
    }
}

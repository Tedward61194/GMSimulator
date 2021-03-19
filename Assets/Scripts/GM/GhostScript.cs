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
        hitPos = ToVector3(hitPosString);
    }

    [Command]
    public void CmdMove() {
        transform.position = hitPos;
    }

    [Command]
    public void CmdDestroyGhost() {
        NetworkServer.Destroy(this.gameObject);
    }

    // I should eventually move this to some sort of static custom utilities class
    private static Vector3 ToVector3(string inString) {

        string outString;
        Vector3 result;
        var splitString = new string[3];

        // Trim extranious parenthesis

        outString = inString.Substring(1, inString.Length - 2);

        // Split delimted values into an array

        splitString = outString.Split(","[0]);

        // Build new Vector3 from array elements

        result.x = float.Parse(splitString[0]);
        result.y = float.Parse(splitString[1]);
        result.z = float.Parse(splitString[2]);

        return result;

    }
}

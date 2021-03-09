using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerLaser : NetworkBehaviour
{
    public Transform laserTransform;
    public LineRenderer line;
    [SerializeField] float range;
    [SerializeField] float flashDuration;

    void Start()
    {
        
    }

    void Update()
    {
        if (isLocalPlayer && Input.GetMouseButtonDown(0)) {
            CmdShoot();
        }
    }

    [Command]
    public void CmdShoot() {
        Ray ray = new Ray(laserTransform.position, laserTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range)) {
            RpcDrawLaser(laserTransform.position, hit.point);
        } else {
            RpcDrawLaser(laserTransform.position, laserTransform.position + laserTransform.forward * range);
        }
    }

    [ClientRpc]
    void RpcDrawLaser(Vector3 start, Vector3 end) {
        StartCoroutine(LaserFlash(start, end));
    }

    IEnumerator LaserFlash(Vector3 start, Vector3 end) {
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        yield return new WaitForSeconds(flashDuration);
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);
    }
}

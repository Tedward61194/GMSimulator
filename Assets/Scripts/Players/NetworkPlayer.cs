using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.Events;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar]
    public int ActivePlayerIndex; // What child of Network Player is active

    [SerializeField] List<GameObject> characters;

    private GameObject NetworkManager;
    private Animator animator;


    public struct NetworkPlayerNotification : NetworkMessage {
        public string childName;
        public string connectionGuid;
    }

    public override void OnStartClient() {
        if (isLocalPlayer) {
            // Spawn in as character select
            transform.Find(characters[0].name).gameObject.SetActive(true);
            transform.Find(characters[1].name).gameObject.SetActive(false);
            transform.Find(characters[2].name).gameObject.SetActive(false);
            NetworkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        }
    }

    #region Character Select
    public void Select(GameObject choice) {
        int choiceIndex = characters.IndexOf(choice);

        if (isServer) {
            // The server should track player choice
            ActivePlayerIndex = choiceIndex;
            // If the player is the server, tell all clients to spawn them
            RpcTestSelect(choiceIndex);
        } else {
            // Commmand server to spawn them on all other clients
            CmdTestSelect(choiceIndex);
        }

        if (isLocalPlayer) {
            // Get active character of players who already spawned 
            ActivateOtherPlayers();
            // Spawn the local player
            LocalSelect(characters.IndexOf(choice));

        }
    }

    private void LocalSelect(int choice) {
        // Handle character select locally
        transform.Find(characters[0].name).gameObject.SetActive(false);
        transform.Find(characters[choice].name).gameObject.SetActive(true);
    }

    [ClientRpc]
    private void RpcTestSelect(int choice) {
        // Spawn other clients who joined
        if (isLocalPlayer)
            return;
        LocalSelect(choice);
    }

    [Command]
    private void CmdTestSelect(int choice) {
        // Apply to all other clients
        // Set active player so for new player's reference
        ActivePlayerIndex = choice;
        // Spawn the player on the server
        LocalSelect(choice);
        // Tell all other clients to spawn the player
        RpcTestSelect(choice);
    }

    // Activate players who are already in the game
    private void ActivateOtherPlayers() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("NetworkPlayer").Where(p => p.GetInstanceID() != gameObject.GetInstanceID()).ToArray();
        foreach (GameObject playerTest in players) {
            // Activate correct child
            int playerActiveIndex = playerTest.GetComponent<NetworkPlayer>().ActivePlayerIndex;
            playerTest.transform.Find(characters[0].name).gameObject.SetActive(false);
            playerTest.transform.Find(characters[playerActiveIndex].name).gameObject.SetActive(true);
        }
    }
    #endregion

    [Command]
    public void CmdSpawnGhost(int ghostId) {
        var GhostCorporealKvP = GetComponentInChildren<GMSpawnableObjectManager>().GhostCorporealKvP;
        NetworkServer.Spawn(Instantiate(GhostCorporealKvP.ElementAt(ghostId).Key), gameObject);
    }

    [Command]
    public void CmdSpawnCorporealObject(int ghostId) {
        var GhostCorporealKvP = GetComponentInChildren<GMSpawnableObjectManager>().GhostCorporealKvP;
        var targetGhost = GameObject.FindGameObjectsWithTag("Ghost").Last();
        Vector3 targetPos = targetGhost.transform.position;
        Quaternion targetRot = targetGhost.transform.rotation;
        GameObject corporealObject = Instantiate(GhostCorporealKvP.ElementAt(ghostId).Value, targetPos, targetRot);
        NetworkServer.Spawn(corporealObject);
        BuildNavMesh(corporealObject);
    }

    [Command]
    public void CmdSpawnWall(int wallId, string middleSt, string wallEndPosSt) {
        Vector3 middle = TedsUtilities.ToVector3(middleSt);
        Vector3 wallEndPos = TedsUtilities.ToVector3(wallEndPosSt);
        GameObject activeWall = GetComponentInChildren<GMSpawnableObjectManager>().Walls[wallId];
        GameObject newWall = Instantiate(activeWall, middle, Quaternion.identity);
        NetworkServer.Spawn(newWall);
        newWall.transform.LookAt(wallEndPos); //setRotation
        BuildNavMesh(newWall);
    }

    [Command]
    public void CmdGMDelete(string guid) {
        GameObject target = GameObject.Find(guid);
        NetworkServer.Destroy(target);
        BuildNavMesh(target);
    }

    private void BuildNavMesh(GameObject newObject) {
        if (newObject.GetComponent(typeof(NavMeshObstacle)) != null) {
            NetworkManager.GetComponent<NavMeshBaker>().BuildAllNavMeshes();
        }
    }

    [Command]
    public void CmdSetIsRunning(bool flag) {//This should probably be more modular just trying to get animation it to work using mirror
        GetComponentInChildren<PaladinAnimationStateController>().SetIsRunning(flag);
    }

    [Command]
    public void CmdAttackOne() { //This should probably be more modular just trying to get animation it to work using mirror
        GetComponentInChildren<PaladinAnimationStateController>().AttackOne();
    }
}

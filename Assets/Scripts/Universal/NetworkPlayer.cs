using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar]
    public int ActivePlayerIndex;

    [SerializeField] List<GameObject> characters;

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
        }
    }

    public void Select(GameObject choice) {
        // Handle character select buttons locally
        transform.Find(characters[0].name).gameObject.SetActive(false);
        transform.Find(characters[characters.IndexOf(choice)].name).gameObject.SetActive(true);

        // Send character select to server
        CmdSelect(characters.IndexOf(choice));
        // Get active character of players who already spawned in
        ActivateOtherPlayers();
        // Set active player so for new player's reference
        ActivePlayerIndex = characters.IndexOf(choice);  
    }

    [Command]
    private void CmdSelect(int choice) {
        transform.Find(characters[0].name).gameObject.SetActive(false);
        transform.Find(characters[choice].name).gameObject.SetActive(true);
    }

    private void ActivateOtherPlayers() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player").Where(p => p.GetInstanceID() != gameObject.GetInstanceID()).ToArray();
        foreach (GameObject player in players) {
            int playerActiveIndex = player.GetComponent<NetworkPlayer>().ActivePlayerIndex;
            player.transform.Find(characters[playerActiveIndex].name).gameObject.SetActive(true);
            player.transform.Find(characters[0].name).gameObject.SetActive(false);
        }
    }
}

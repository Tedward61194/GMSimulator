using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System.Linq;
using System.Reflection;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] GameObject characterSelectDisplay;
    [SerializeField] List<GameObject> characters;

    //GameObject parentNetworkPlayer;

    [Command]
    public override void OnStartClient() {
        if (isLocalPlayer) {
            transform.Find(characters[0].name).gameObject.SetActive(true);
            transform.Find(characters[1].name).gameObject.SetActive(false);
            transform.Find(characters[2].name).gameObject.SetActive(false);

            transform.Find(characters[0].name).GetComponent<CharacterSelect>().characters = characters;
        }
        //parentNetworkPlayer = transform.parent.gameObject;
    }
    /*public void Select(GameObject choice) {
        //CmdOnSelect(choice.name);
        CmdTestSelect(choice.name);
    }*/

    /*[Command(ignoreAuthority = true)]
    public void CmdTestSelect(string choice, NetworkConnectionToClient sender = null) {
        List<string> characterNames = characters.Select(c => c.name).ToList();
        int index = characterNames.IndexOf(choice);
        GameObject playerInstance = Instantiate(characters[index]);
        switch (index) {
            case 0:
                playerInstance.GetComponent<GMCameraController>().connectionToClientId = transform.gameObject.GetComponent<NetworkIdentity>().connectionToClient.connectionId;
                break;
            case 1:
                //playerInstance.GetComponent<PlayerController>().connectionToClientId = transform.gameObject.GetComponent<NetworkIdentity>().connectionToClient.connectionId;
                break;
        }

        NetworkServer.Spawn(playerInstance, sender);
        characterSelectDisplay.SetActive(false);
    }*/


    /*

    [Command(ignoreAuthority = true)] // Normally commands are only allowed to be called by clients who own a script, this line (and the param who tells us who's calling) get around that
    public void CmdOnSelect(string choice, NetworkConnectionToClient sender = null) {
        //var test = isLocalPlayer;//parentNetworkPlayer;//networkObject.GetComponent<NetworkIdentity>().isLocalPlayer;
        List<string> characterNames = characters.Select(c => c.name).ToList();
        int index = characterNames.IndexOf(choice);
        //if (isLocalPlayer) {
            //var test = connectionToClient;
       // }
        GameObject playerInstance = Instantiate(characters[index]);
        switch (index) {
            case 0:
                playerInstance.GetComponent<GMCameraController>().isLocalPlayerOverride = isLocalPlayer;
                break;
            case 1:
                playerInstance.GetComponent<PlayerController>().isLocalPlayerOverride = isLocalPlayer;
                break;
        }

        //CopyNetworkData(transform.gameObject.GetComponent<NetworkIdentity>(), ref playerInstance);
        //NetworkIdentity test = transform.gameObject.GetComponent<NetworkIdentity>();
        //playerInstance.AddComponent<NetworkIdentity>(transform.gameObject.GetComponent<NetworkIdentity>());// = transform.gameObject.GetComponent<NetworkIdentity>();
        NetworkServer.Spawn(playerInstance, sender);
        //PlayerPrefs.SetInt("playerId", index);
        characterSelectDisplay.SetActive(false);
    }*/





    /*private void CopyNetworkData (Component original, ref GameObject target) {
        System.Type type = original.GetType();
        Component targetComponent = target.transform.gameObject.GetComponent<NetworkIdentity>();
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |BindingFlags.FlattenHierarchy); // By default we only get public

        System.Type targetType = targetComponent.GetType();

        PropertyInfo tedsTest1 = type.GetProperty("isLocalPlayer");
        PropertyInfo tedsTest2 = targetType.GetProperty("isLocalPlayer");

        //var field = type.GetField("isLocalPlayer", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
        //var field2 = type.GetFields();

        //var test3 = tedsTest1.GetValue();
        //var test4 = tedsTest2.GetValue();
        //tedsTest2.SetValue(tedsTest2, true));

        foreach (FieldInfo field in fields) {
            //if (!field.FieldType.Name.Contains("NetworkBehaviour[]")) {
                field.SetValue(targetComponent, field.GetValue(original));
 //           }
        }
        //var test1 = .SetValue
        //copy.SetValue()
        var test = targetComponent;
        return;
    }*/
}

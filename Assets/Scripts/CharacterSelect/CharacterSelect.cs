using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterSelect : NetworkBehaviour
{
    [HideInInspector]
    public List<GameObject> characters;

    public void Select(GameObject choice) {
        int index = characters.IndexOf(choice);
    }
    
    [Command]
    public void CmdSelect(int choice) {

    }
}

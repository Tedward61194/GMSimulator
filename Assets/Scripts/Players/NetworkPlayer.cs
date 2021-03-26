using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.Events;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar]
    public int ActivePlayerIndex;

    [SerializeField] List<GameObject> characters;

    private GameObject NetworkManager;
    private GameObject bodyToAnimate;
    private Animator animator;
    private MonoBehaviour animationController;

    //public delegate void TestEvent();
    //public static event TestEvent OnTestEvent;
    //private UnityAction testAction;

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

            // Delegate test
            //PlayerMovement.OnAttackOneEvent += DelegateTest;
        }
    }

    //void DelegateTest() {
    //var test = 1;
    //}

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
        // Set Network Animator
        animator = transform.Find(characters[characters.IndexOf(choice)].name).gameObject.GetComponentInChildren<Animator>();
        GetComponent<NetworkAnimator>().animator = animator;//transform.Find(characters[characters.IndexOf(choice)].name).gameObject.GetComponentInChildren<Animator>();

        // TODO: Make this more robust
        if (characters.IndexOf(choice) == 2) {
            GetComponent<PaladinAnimationStateController>().Init();
            CmdInitTest();
            // Players with animations must have "Body"
            //bodyToAnimate = transform.Find("Body").gameObject;
            // animationController = transform.Find(characters[characters.IndexOf(choice)].name).gameObject.GetComponent<PlayerMovement>().animationStateController;
        }
    }

    [Command]
    void CmdInitTest() {
        GetComponent<PaladinAnimationStateController>().Init();
    }

    [Command]
    private void CmdSelect(int choice) {
        transform.Find(characters[0].name).gameObject.SetActive(false);
        transform.Find(characters[choice].name).gameObject.SetActive(true);
        GetComponent<NetworkAnimator>().animator = transform.Find(characters[choice].name).gameObject.GetComponentInChildren<Animator>();
    }

    private void ActivateOtherPlayers() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("NetworkPlayer").Where(p => p.GetInstanceID() != gameObject.GetInstanceID()).ToArray();
        foreach (GameObject player in players) {
            // Activate correct child
            int playerActiveIndex = player.GetComponent<NetworkPlayer>().ActivePlayerIndex;
            player.transform.Find(characters[playerActiveIndex].name).gameObject.SetActive(true);
            player.transform.Find(characters[0].name).gameObject.SetActive(false);

            // Set Network Animator
            player.GetComponent<NetworkAnimator>().animator = player.transform.Find(characters[playerActiveIndex].name).gameObject.GetComponentInChildren<Animator>();
        }
    }

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
        GetComponent<PaladinAnimationStateController>().SetIsRunning(flag);
    }

    [Command]
    public void CmdAttackOne() { //This should probably be more modular just trying to get animation it to work using mirror
        //var testTransform = transform.Find("Player1");
        //var testGameObject = testTransform.gameObject;
        //var testPlayerMovement = testGameObject.GetComponent<PlayerMovement>();
        GetComponent<PaladinAnimationStateController>().AttackOne();
//        testPlayerMovement.AttackOne();
    }

    //[Command]
    public void CmdSetAnimationDependancies(string bodyName, string controllerName) {
        //bodyToAnimate = transform.Find(bodyName).gameObject;
        var test = transform.Find("Body");
        var test2 = test.gameObject;
        animationController = bodyToAnimate.GetComponent("controllerName") as MonoBehaviour;
        //PlayerMovement.OnAttackOneEvent += AttackOneEvent;
        //        GetComponentInChildren<PaladinAnimationStateController>().Invoke("AttackOne");
    }

    [Command]
    public void CmdCallAnimationOnServer(string animationInformation) {
        var test = JsonUtility.FromJson<AnimationInformation>(animationInformation);
        //       Transform bodyToAnimate = transform.Find(test.
        //testAction += TestMethod;
    }

    ////public void TestMethod() {
        //var test = 1;
    //}
}

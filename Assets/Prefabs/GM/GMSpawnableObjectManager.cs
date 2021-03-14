using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GMSpawnableObjectManager : MonoBehaviour {

    public Dictionary<GameObject, GameObject> GhostCorporealKvP = new Dictionary<GameObject, GameObject>();

    public GameObject polePrefab; // Make these lists like ghosts and figure out better way for hight offset
    public GameObject fencePrefab;
    public float fenceHeightOffset; // Set to half height of fence

    // Unity dosn't seem to let me setup KvPs in inspector, so this'll have to do
    [SerializeField] List<GameObject> ghosts;
    [SerializeField] List<GameObject> corporealObject;

    GMCameraController cameraController;
    bool ghostActiveFlag;
    bool placingFenceFlag; // Wall creation mode entered but waiting on player input
    bool creatingFence; // Activly dragging wall
    int activeGhostIndex;
    GameObject activeWall;
    GameObject activePole;
    GameObject lastPole;

    public void Start() {
        // Initialize GhostCorporealKvP
        for (int i = 0; i < ghosts.Count; i++) {
            GhostCorporealKvP.Add(ghosts[i], corporealObject[i]);
        }

        cameraController = GetComponent<GMCameraController>();
    }

    public void Update() {
        if (ghostActiveFlag) {
            if (Input.GetMouseButtonDown(0)) {
                GetComponentInParent<NetworkPlayer>().CmdSpawnCorporealObject(activeGhostIndex);
            }
            if (Input.GetKey(KeyCode.Escape)) {
                ghostActiveFlag = false;
            }
        }

        if (placingFenceFlag) {    
            if (Input.GetMouseButtonDown(0)) {
                StartFence(activeWall, activePole);
            } else if(Input.GetMouseButtonUp(0)) {
                FinishFence();
            } else {
                if (creatingFence) {
                    UpdateFence();
                }
            }
            if (Input.GetKey(KeyCode.Escape)) {
                placingFenceFlag = false;
            }
        }
    }

    public void SpawnGhost(GameObject ghost) {
        activeGhostIndex = ghosts.IndexOf(ghost);
        GetComponentInParent<NetworkPlayer>().CmdSpawnGhost(activeGhostIndex);
        ghostActiveFlag = true;
        
    }

    public void CreateFence(GameObject wallParent) {
        activeWall = wallParent.transform.Find("MainSection").gameObject;
        activePole = wallParent.transform.Find("Pole").gameObject;
        placingFenceFlag = true;
    }

    public void StartFence(GameObject wall, GameObject pole) {
        Vector3 startPos = cameraController.GetMousePos();
        startPos = cameraController.SnapPosition(startPos);
        GameObject startPole = Instantiate(pole, startPos, Quaternion.identity);
        startPole.transform.position = new Vector3(startPos.x, startPos.y, startPos.z);
        lastPole = startPole;
        creatingFence = true;
    }

    public void FinishFence() {
        //creatingFence = false;
    }
    public void UpdateFence() {
        Vector3 current = cameraController.GetMousePos();
        current = cameraController.SnapPosition(current);
        if (!current.Equals(lastPole.transform.position)) {
            createFenceSegment(current);
        }
    }
    void createFenceSegment(Vector3 current) {
        GameObject newPole = Instantiate(activePole, current, Quaternion.identity);
        Vector3 middle = Vector3.Lerp(newPole.transform.position, lastPole.transform.position, 0.5f);
        GameObject newFence = Instantiate(activeWall, middle, Quaternion.identity);
        newFence.transform.LookAt(lastPole.transform); // Set rotation
        lastPole = newPole;
    }

}

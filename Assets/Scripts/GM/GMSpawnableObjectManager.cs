using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GMSpawnableObjectManager : MonoBehaviour {

    public Dictionary<GameObject, GameObject> GhostCorporealKvP = new Dictionary<GameObject, GameObject>();
    public List<GameObject> Walls = new List<GameObject>();

    // Unity dosn't seem to let me setup KvPs in inspector, so this'll have to do
    [SerializeField] List<GameObject> ghosts;
    [SerializeField] List<GameObject> corporealObject;
    [SerializeField] List<GameObject> walls;

    GMCameraController cameraController;
    bool ghostActiveFlag;
    bool placingWallFlag; // Wall creation mode entered but waiting on player input
    bool creatingWall; // Activly dragging wall
    int activeGhostIndex;
    int activeWallIndex;
    GameObject activeWall;
    Vector3 wallStartPos;
    Vector3 wallEndPos;

    public void Start() {
        // Initialize GhostCorporealKvP
        for (int i = 0; i < ghosts.Count; i++) {
            GhostCorporealKvP.Add(ghosts[i], corporealObject[i]);
        }
        for (int i = 0; i < walls.Count; i++) {
            Walls.Add(walls[i]);
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

        if (placingWallFlag) {    
            if (Input.GetMouseButtonDown(0)) {
                StartWall(activeWall);
            } else if(Input.GetMouseButtonUp(0)) {
                FinishWall();
            } else {
                if (creatingWall) {
                    UpdateWall();
                }
            }
            if (Input.GetKey(KeyCode.Escape)) {
                placingWallFlag = false;
            }
        }
    }

    public void SpawnGhost(GameObject ghost) {
        activeGhostIndex = ghosts.IndexOf(ghost);
        GetComponentInParent<NetworkPlayer>().CmdSpawnGhost(activeGhostIndex);
        ghostActiveFlag = true;
        
    }

    public void CreateWall(GameObject wallParent) {
        activeWall = wallParent;
        placingWallFlag = true;
    }

    public void StartWall(GameObject wall) {
        wallStartPos = cameraController.GetMousePos();
        wallStartPos = cameraController.SnapPosition(wallStartPos);
        wallEndPos = wallStartPos;
        creatingWall = true;
    }

    public void FinishWall() {
        creatingWall = false;
    }
    public void UpdateWall() {
        Vector3 currentPos = cameraController.GetMousePos();
        currentPos = cameraController.SnapPosition(currentPos);
        if (!currentPos.Equals(wallEndPos)) {
            createWallSegment(currentPos);
        }
    }
    void createWallSegment(Vector3 currentPos) {
        wallEndPos = currentPos;
        Vector3 middle = Vector3.Lerp(wallStartPos, wallEndPos, 0.5f);
        GetComponentInParent<NetworkPlayer>().CmdSpawnWall(Walls.IndexOf(activeWall), middle.ToString(), wallEndPos.ToString());
        //GameObject newWall = Instantiate(activeWall, middle, Quaternion.identity);
        //newWall.transform.LookAt(wallEndPos); //setRotation
        wallStartPos = wallEndPos;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] int maxPlayerId;
    [SerializeField] GameObject gmCamera;
    [SerializeField] GameObject playerOneCamera;

    AudioListener gmCameraAudioListener;
    AudioListener playerOneCameraAudioListener;

    void Start()
    {
        gmCameraAudioListener = gmCamera.GetComponent<AudioListener>();
        playerOneCameraAudioListener = playerOneCamera.GetComponent<AudioListener>();

        changeCamera(PlayerPrefs.GetInt("playerCamera"));
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            getCamera();
        }
    }

    void getCamera() {
        int cameraId = PlayerPrefs.GetInt("playerCamera");
        cameraId++;
        changeCamera(cameraId);
    }

    void changeCamera(int cameraId) {
        if (cameraId > maxPlayerId) {
            cameraId = 0;
        }

        PlayerPrefs.SetInt("playerCamera", cameraId);

        switch(cameraId) {
            case 0:
                gmCamera.SetActive(true);
                gmCameraAudioListener.enabled = true;
                playerOneCamera.SetActive(false);
                playerOneCameraAudioListener.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case 1:
                gmCamera.SetActive(false);
                gmCameraAudioListener.enabled = false;
                playerOneCamera.SetActive(true);
                playerOneCameraAudioListener.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            default:
                Debug.LogError("Invalid CameraId in CameraManager.changeCamera");
                break;
        }
    }
}

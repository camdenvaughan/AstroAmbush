using System;
using System.Collections;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UIShipHandler : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform shipTrans;
    [SerializeField] private Transform cameraTrans;
    [SerializeField] private Transform planetTrans;
    [SerializeField] private float pauseZoom;
    [SerializeField] private float settingsZoom;
    [SerializeField] private float controlsZoom;
    [SerializeField] private float volumeZoom;
    [SerializeField] private float volumeYHeight;
    [SerializeField] private float nameZoom;
    [SerializeField] private float nameYHeight;
    [SerializeField] private float nameXOffset;
    [SerializeField] private float planetZoom;
    [SerializeField] private float planetXOffset;
    [SerializeField] private Vector3 pauseShipRotation;
    [SerializeField] private Vector3 controlsShipRotation;
    [SerializeField] private Vector3 volumeShipRotation;
    [SerializeField] private Vector3 nameShipRotation;

    [SerializeField] private ParticleSystem[] rightBoosters = new ParticleSystem[2];
    [SerializeField] private ParticleSystem[] leftBoosters = new ParticleSystem[2];

    [SerializeField] private float mainBoosterMin;
    [SerializeField] private float secondaryBoosterMin;
    
    private ParticleSystem.MainModule rightMain;
    private ParticleSystem.MainModule rightSecondary;
    private ParticleSystem.MainModule leftMain;
    private ParticleSystem.MainModule leftSecondary;


    private Vector3 shipPos;
    private Quaternion shipRot;

    private Vector3 cameraPos;
    private CinemachineBrain cameraBrain;

    private bool onVolumeScreen = false;

    private IEnumerator moveAndRotateCoroutine;
    private delegate void EndMoveFunction();

    private void Start()
    {
        rightMain = rightBoosters[0].main;
        rightSecondary = rightBoosters[1].main;
        leftMain = leftBoosters[0].main;
        leftSecondary = leftBoosters[1].main;
    }

    private void Update()
    {
        if (!onVolumeScreen) return;

        leftMain.startLifetime = PlayerPrefs.GetFloat("musicVolume") + mainBoosterMin;
        leftSecondary.startLifetime = (PlayerPrefs.GetFloat("musicVolume") + secondaryBoosterMin);
            
        rightMain.startLifetime = PlayerPrefs.GetFloat("effectsVolume") + mainBoosterMin;
        rightSecondary.startLifetime = (PlayerPrefs.GetFloat("effectsVolume") + secondaryBoosterMin);
    }

    public void OpenPauseScreen()
    {
        shipPos = shipTrans.position;
        shipRot = shipTrans.rotation;        
        cameraBrain = Camera.main.GetComponent<CinemachineBrain>();
        cameraBrain.enabled = false;

        cameraPos = cameraTrans.position;
        SetBoosters(false);
    }

    public void ClosePauseScreen(GameObject gameScreen)
    {
        cameraPos = new Vector3(shipPos.x, shipPos.y, cameraPos.z);
        if (moveAndRotateCoroutine != null)
            StopCoroutine(moveAndRotateCoroutine);
        moveAndRotateCoroutine = MoveCameraAndShip(cameraTrans, cameraTrans.position, cameraPos, shipTrans, shipTrans.rotation,
            shipRot, () =>
            {
                cameraBrain.enabled = true;
                SetBoosters(true);
                gameScreen.SetActive(true);
            });
        StartCoroutine(moveAndRotateCoroutine);
    }
    
    public void OpenSettings()
    {
        shipPos = shipTrans.position;
        shipRot = shipTrans.rotation;
        cameraPos = cameraTrans.position;
        shipTrans.gameObject.GetComponent<MenuShip>().enabled = false;
        shipTrans.gameObject.GetComponent<Animator>().SetFloat("rotation", 0);
        SetBoosters(false);
    }

    public void CloseSettings(GameObject menuScreen)
    {
        if (moveAndRotateCoroutine != null)
            StopCoroutine(moveAndRotateCoroutine);
        moveAndRotateCoroutine = MoveCameraAndShip(cameraTrans, cameraTrans.position, cameraPos, cameraTrans, 
            cameraTrans.rotation, quaternion.identity, () =>
            {
                shipTrans.gameObject.GetComponent<MenuShip>().enabled = true;
                SetBoosters(true);
                menuScreen.SetActive(true);
            });
        StartCoroutine(moveAndRotateCoroutine);
    }

    public void OpenLeaderBoard(GameObject leaderBoardScreen)
    {
        cameraPos = cameraTrans.position;

        planetTrans.gameObject.GetComponent<Planet>().resolution = 200;
        shipTrans.gameObject.GetComponent<MenuShip>().enabled = false;

        Vector3 planetPos = new Vector3(planetTrans.position.x + planetXOffset, planetTrans.position.y, planetZoom);
        
        if (moveAndRotateCoroutine != null)
            StopCoroutine(moveAndRotateCoroutine);
        moveAndRotateCoroutine = MoveCameraAndShip(cameraTrans, cameraTrans.position, planetPos, cameraTrans,
            cameraTrans.rotation, quaternion.identity, () =>
            {
                Vector3 shipOrigin = new Vector3(0f, 0f, shipTrans.position.z);
                shipTrans.position = shipOrigin;
                leaderBoardScreen.SetActive(true);
            });
        StartCoroutine(moveAndRotateCoroutine);
    }

    public void CloseLeaderBoard(GameObject menuScreen)
    {
        if (moveAndRotateCoroutine != null)
            StopCoroutine(moveAndRotateCoroutine);
        moveAndRotateCoroutine = MoveCameraAndShip(cameraTrans, cameraTrans.position, cameraPos, cameraTrans,
            cameraTrans.rotation, quaternion.identity,
            () =>
            {
                shipTrans.gameObject.GetComponent<MenuShip>().enabled = true;
                menuScreen.SetActive(true);
                planetTrans.gameObject.GetComponent<Planet>().resolution = 75;
            });
        
        StartCoroutine(moveAndRotateCoroutine);
    }
    
    public void MoveCameraToPauseLoc(GameObject pauseScreen)
    {
        SetBoosters(false);
        Vector3 pauseLoc = new Vector3(shipTrans.position.x, shipTrans.position.y, pauseZoom);
        
        if (moveAndRotateCoroutine != null)
            StopCoroutine(moveAndRotateCoroutine);
        moveAndRotateCoroutine = MoveCameraAndShip(cameraTrans, cameraTrans.localPosition, pauseLoc, shipTrans,
            shipTrans.rotation, Quaternion.Euler(pauseShipRotation), () => { pauseScreen.SetActive(true); });
        StartCoroutine(moveAndRotateCoroutine);
    }
    public void MoveCameraToSettingsLoc(GameObject settingsScreen)
    {
        onVolumeScreen = false;
        SetBoosters(false);
        Vector3 settingsLoc = new Vector3(shipTrans.position.x, shipTrans.position.y, settingsZoom);
        
        if (moveAndRotateCoroutine != null)
            StopCoroutine(moveAndRotateCoroutine);
        moveAndRotateCoroutine = MoveCameraAndShip(cameraTrans, cameraTrans.localPosition, settingsLoc, shipTrans, shipTrans.rotation,
            Quaternion.identity, () => { settingsScreen.SetActive(true); });
        StartCoroutine(moveAndRotateCoroutine);
    }

    public void MoveCameraToControlsLoc(GameObject controlsScreen, Button mouseControls, Button keyboardControls)
    {
        Vector3 controlsLoc = new Vector3(shipTrans.position.x, shipTrans.position.y, controlsZoom);
        
        if (moveAndRotateCoroutine != null)
            StopCoroutine(moveAndRotateCoroutine);
        moveAndRotateCoroutine = MoveCameraAndShip(cameraTrans, cameraTrans.localPosition, controlsLoc,  shipTrans, shipTrans.rotation,
            Quaternion.Euler(controlsShipRotation), () =>
            {
                controlsScreen.SetActive(true);
                if (PlayerPrefs.GetInt("controlLayout", 0) == 0)
                {
                    mouseControls.GetComponent<Animator>().SetBool("isSelected", true);
                    keyboardControls.GetComponent<Animator>().SetBool("isSelected", false);
                }
                else
                {
                    mouseControls.GetComponent<Animator>().SetBool("isSelected", false);
                    keyboardControls.GetComponent<Animator>().SetBool("isSelected", true);
                }
            });
        StartCoroutine(moveAndRotateCoroutine);
    }

    public void MoveCameraToChangeNameLoc(GameObject changeNameScreen)
    {
        Vector3 nameLoc = new Vector3(shipTrans.position.x + nameXOffset, shipTrans.position.y + nameYHeight, nameZoom);
        
        if (moveAndRotateCoroutine != null)
            StopCoroutine(moveAndRotateCoroutine);
        moveAndRotateCoroutine = MoveCameraAndShip(cameraTrans, cameraTrans.localPosition, nameLoc, shipTrans,
            shipTrans.rotation, Quaternion.Euler(nameShipRotation),
            () =>
            {
                changeNameScreen.SetActive(true);
                InputField inputField = shipTrans.GetComponentInChildren<InputField>();
                inputField.text = "";
                inputField.ActivateInputField();
            });
        StartCoroutine(moveAndRotateCoroutine);
    }

    public void MoveCameraToVolumeLoc(GameObject volumeScreen)
    {
        onVolumeScreen = true;
        Vector3 volumeLoc = new Vector3(shipTrans.position.x, shipTrans.position.y + volumeYHeight, volumeZoom);
        
        if (moveAndRotateCoroutine != null)
            StopCoroutine(moveAndRotateCoroutine);
        moveAndRotateCoroutine = MoveCameraAndShip(cameraTrans, cameraTrans.localPosition, volumeLoc, shipTrans, shipTrans.rotation,
            Quaternion.Euler(volumeShipRotation), () =>
            {
                SetBoosters(true);
                volumeScreen.SetActive(true);
            });
        StartCoroutine(moveAndRotateCoroutine);
    }

    private void SetBoosters(bool isActive)
    {
        rightBoosters[0].gameObject.SetActive(isActive);
        leftBoosters[0].gameObject.SetActive(isActive);
    }
    
    private IEnumerator MoveCameraAndShip (Transform moveTransform, Vector3 startPos, Vector3 endPos, Transform rotationTransform, Quaternion startRot, Quaternion endRot, EndMoveFunction function = null)
    {
        if (GameManager.current != null)
            GameManager.ToggleInput();
        float i = 0;
        float rate = 1/rotationSpeed;
        while (i < 1)         
        {
            i += Time.deltaTime * rate;
            moveTransform.localPosition = Vector3.Lerp(startPos, endPos, i);
            rotationTransform.rotation = Quaternion.Slerp (startRot, endRot, i);
            yield return 0;
        }
        function?.Invoke();
        if (GameManager.current != null)
            GameManager.ToggleInput();
        yield return 0;
    }
}

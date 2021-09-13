using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIShipHandler : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform shipTrans;
    [SerializeField] private Transform cameraTrans;
    [SerializeField] private float settingsZoom;
    [SerializeField] private float controlsZoom;
    [SerializeField] private float volumeZoom;
    [SerializeField] private float volumeYHeight;
    [SerializeField] private Vector3 controlsShipRotation;
    [SerializeField] private Vector3 volumeShipRotation;

    


    private bool onVolumeScreen = false;

    [SerializeField] private ParticleSystem[] rightBoosters = new ParticleSystem[2];
    [SerializeField] private ParticleSystem[] leftBoosters = new ParticleSystem[2];

    [SerializeField] private float mainBoosterMin;
    [SerializeField] private float secondaryBoosterMin;

    private ParticleSystem.MainModule rightMain;
    private ParticleSystem.MainModule rightSecondary;
    private ParticleSystem.MainModule leftMain;
    private ParticleSystem.MainModule leftSecondary;


    private Vector3 menuShipPos;
    private Quaternion menuShipRot;

    private Vector3 cameraPos;

    private void Update()
    {
        if (onVolumeScreen)
        {
            rightMain = rightBoosters[0].main;
            rightSecondary = rightBoosters[1].main;
            leftMain = leftBoosters[0].main;
            leftSecondary = leftBoosters[1].main;
            
            leftMain.startLifetime = PlayerPrefs.GetFloat("musicVolume") + mainBoosterMin;
            leftSecondary.startLifetime = (PlayerPrefs.GetFloat("musicVolume") + secondaryBoosterMin);
            
            rightMain.startLifetime = PlayerPrefs.GetFloat("effectsVolume") + mainBoosterMin;
            rightSecondary.startLifetime = (PlayerPrefs.GetFloat("effectsVolume") + secondaryBoosterMin);
        }
    }

    public void OpenSettings()
    {
        menuShipPos = shipTrans.position;
        menuShipRot = shipTrans.rotation;
        cameraPos = cameraTrans.position;
        shipTrans.gameObject.GetComponent<MenuShip>().enabled = false;
        SetBoostersScene(false);
    }
    public void MoveCameraToSettingsLoc(GameObject settingsScreen)
    { 
        onVolumeScreen = false;
        SetBoostersScene(false);
        Vector3 settingsLoc = new Vector3(shipTrans.position.x, shipTrans.position.y, settingsZoom);
        StartCoroutine(MoveCameraAndShip(cameraTrans, cameraTrans.localPosition, settingsLoc, shipTrans, shipTrans.rotation,
            Quaternion.identity, settingsScreen));
    }

    public void MoveCameraToControlsLoc(GameObject controlsScreen)
    {
        onVolumeScreen = false;
        Vector3 controlsLoc = new Vector3(shipTrans.position.x, shipTrans.position.y, controlsZoom);
        StartCoroutine(MoveCameraAndShip(cameraTrans, cameraTrans.localPosition, controlsLoc,  shipTrans, shipTrans.rotation,
            Quaternion.Euler(controlsShipRotation), controlsScreen));
    }

    public void MoveCameraToVolumeLoc(GameObject volumeScreen)
    {
        onVolumeScreen = true;
        Vector3 volumeLoc = new Vector3(shipTrans.position.x, shipTrans.position.y + volumeYHeight, volumeZoom);
        StartCoroutine(MoveCameraAndShip(cameraTrans, cameraTrans.localPosition, volumeLoc, shipTrans, shipTrans.rotation,
            Quaternion.Euler(volumeShipRotation), volumeScreen, true));
    }

    public void SetBoostersScene(bool turnOn)
    {

        rightBoosters[0].gameObject.SetActive(turnOn);
        leftBoosters[0].gameObject.SetActive(turnOn);
        
    }

    public void CloseSettings(GameObject menuScreen)
    {
        onVolumeScreen = false;
        StartCoroutine(MoveCameraAndShip(shipTrans, shipTrans.position, menuShipPos, shipTrans, shipTrans.rotation,
            menuShipRot, null, true));
        StartCoroutine(MoveCameraAndShip(cameraTrans, cameraTrans.position, cameraPos, cameraTrans, cameraTrans.rotation,
            quaternion.identity, menuScreen, true));
        shipTrans.gameObject.GetComponent<MenuShip>().enabled = true;
        SetBoostersScene(true);

    }

    public IEnumerator MoveCameraAndShip (Transform moveTransform, Vector3 startPos, Vector3 endPos, Transform rotationTransform, Quaternion startRot, Quaternion endRot, GameObject UI = null, bool turnOnBoosters = false, bool showUI = true)
    {
        float i = 0;
        float rate = 1/rotationSpeed;
        while (i < 1)         {
            i += Time.deltaTime * rate;
            moveTransform.localPosition = Vector3.Lerp(startPos, endPos, i);
            rotationTransform.rotation = Quaternion.Slerp (startRot, endRot, i);
            yield return 0;
        }
        if(UI != null)
            UI.SetActive(showUI);
        SetBoostersScene(turnOnBoosters);
        yield return 0;
    }
}

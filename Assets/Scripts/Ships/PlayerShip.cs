using System;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [SerializeField] private float radius;
    private ShipInputController activeController;
    private KeyboardInputController keyboardController;
    private MouseInputController mouseController;
    private ShipMovementController movement;
    private Animator anim;
    [SerializeField] private Transform[] guns = new Transform[2];
    private bool shootFromLeft;
    
    
    private void Start()
    {
        mouseController = gameObject.AddComponent<MouseInputController>();
        keyboardController = gameObject.AddComponent<KeyboardInputController>();
        
        SetControls();

        movement = GetComponent<ShipMovementController>();
        anim = GetComponent<Animator>();

        GameObject.FindObjectOfType<UINavigator>().PauseStateChanged += OnPauseStateChanged;
    }

    private void Update()
    {
        if (GameManager.GetState() == GameManager.GameState.Active)
        {
            movement.Move(activeController.horizontal, activeController.vertical, activeController.rotate);
            
            if (!activeController.fire) return;
            
            int gunToggle = shootFromLeft ? 0 : 1;
            GameObject obj = ObjectPooler.GetBullet();
            obj.transform.SetPositionAndRotation(guns[gunToggle].position, guns[gunToggle].rotation);
            obj.SetActive(true);
            shootFromLeft = !shootFromLeft;
            return;
        }
        
        if (GameManager.GetState() == GameManager.GameState.WaitingForInput)
            if (activeController.fire)
                GameManager.SetGameToActive();
        
        anim.SetFloat("rotation", 0);
    }

    void SetControls()
    {
        if (PlayerPrefs.GetInt("controlLayout", 0) == 0)
            activeController = mouseController;
        else
            activeController = keyboardController;
    }
    
    private void OnPauseStateChanged(object source, EventArgs e)
    {
        SetControls();
        GameManager.PauseGame();
    }
    
    
}

using System;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{

    [Header("Ship Properties")]
    [SerializeField]
    private float health = 100.0f;
    
    private ShipInputController inputController;
    private ShipMovementController movement;
    private Animator anim;
    [SerializeField] private Transform[] guns = new Transform[2];
    private bool shootFromLeft;
    private void Start()
    {
        inputController = GetComponent<PlayerInputController>();
        movement = GetComponent<ShipMovementController>();
    }

    private void Update()
    {
        if (GameManager.GameIsActive())
            movement.Move();
        else
            anim.SetFloat("rotation", 0);
        
        if(inputController.fire)
        {
            if (!GameManager.GameIsActive())
                GameManager.StartGame();
            int gunToggle = shootFromLeft ? 0 : 1;
            GameObject obj = ObjectPooler.GetBullet();
            obj.transform.SetPositionAndRotation(guns[gunToggle].position, guns[gunToggle].rotation);
            obj.SetActive(true);
            shootFromLeft = !shootFromLeft;
        }
    }
}

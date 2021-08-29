using System;
using UnityEngine;

public class Ship_Player : MonoBehaviour
{

    [Header("Ship Properties")]
    [SerializeField]
    private float health = 100.0f;
    
    private ShipInputController inputController;
    [SerializeField] private Transform[] guns = new Transform[2];
    private bool shootFromLeft;
    private void Start()
    {
        inputController = GetComponent<PlayerInputController>();
    }

    private void Update()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        // End Game
    }
}

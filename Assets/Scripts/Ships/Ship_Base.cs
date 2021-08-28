using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Base : MonoBehaviour
{
    [Header("Ship Properties")]
    [SerializeField]
    protected float health = 100.0f;
    
    protected Rigidbody rb;
    protected ShipInputController inputController;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

}

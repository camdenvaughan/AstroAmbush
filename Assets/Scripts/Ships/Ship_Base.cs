using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Base : MonoBehaviour
{
    [Header("Ship Properties")]
    [SerializeField]
    protected float health = 100.0f;
    [Header("Movement")]
    [SerializeField]
    protected float moveForce;
    
    protected Rigidbody rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    protected void MoveTo(Vector3 position)
    {
        Vector3 currentPos = transform.position;
        Vector3 dir = (currentPos - position).normalized;
        
        if (Vector3.Distance(currentPos, position) > 2.0f)
            rb.AddForce(-dir * moveForce);
        
    }

    protected void RotateTowards(Vector3 position)
    {
        Vector3 currentPos = transform.position;
        Vector3 dir = (currentPos - position).normalized;
        
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }

}

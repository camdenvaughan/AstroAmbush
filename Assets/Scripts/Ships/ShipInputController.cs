﻿using UnityEngine;

public class ShipInputController : MonoBehaviour
{

    [HideInInspector]
    public float horizontal;
    [HideInInspector]
    public float vertical;

    [HideInInspector] 
    public bool fire;
	
    void OnDrawGizmos() {
        var directionVector = new Vector3(horizontal, 0, vertical);
        var size = Mathf.Min(1, directionVector.magnitude);

        if (size >= float.Epsilon) {
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(this.transform.position, 4);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(this.transform.position, size * 4);
        }

        // Draw each component of the input
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + Vector3.right * horizontal * 4);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(this.transform.position, this.transform.position + Vector3.up * vertical * 4);
        // Show the sum of both components
        Gizmos.color = Color.white;
        Gizmos.DrawLine(this.transform.position, this.transform.position + directionVector * 4);
    }
}

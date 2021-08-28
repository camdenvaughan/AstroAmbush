
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    

    private Vector3 position;
    private void OnEnable()
    {
        Invoke("Disable", 2f);
    }


    private void Update()
    {
        transform.Translate(transform.up * bulletSpeed * Time.deltaTime);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}

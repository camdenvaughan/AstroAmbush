using UnityEngine;


public class Rotator : MonoBehaviour
{
    private float rotateSpeed;

    private void Start()
    {
        rotateSpeed = Random.Range(10f, 20f);
    }

    void Update()
    {        
        if (GameManager.GameIsActive())
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }
}

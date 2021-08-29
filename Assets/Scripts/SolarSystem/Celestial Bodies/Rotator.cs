using UnityEngine;


public class Rotator : MonoBehaviour
{
    private float rotateSpeed;

    private void Start()
    {
        rotateSpeed = Random.Range(2f, 7f);
    }

    void Update()
    {        
        if (GameManager.GameIsActive())
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }
}

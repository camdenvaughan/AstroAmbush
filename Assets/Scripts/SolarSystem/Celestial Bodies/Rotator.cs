using UnityEngine;


public class Rotator : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Vector3 axis;

    private void Start()
    {
        if (rotateSpeed == 0) 
            rotateSpeed = Random.Range(10f, 20f);
    }

    void Update()
    {
        transform.Rotate(axis, rotateSpeed * Time.deltaTime);
    }
}

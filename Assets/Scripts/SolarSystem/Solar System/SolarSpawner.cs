using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class SolarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject solarSystem;

    [SerializeField] Transform shipPos;
    [SerializeField] private Vector3[] startingPositions = new Vector3[4];

    private List<Transform> solarTransforms = new List<Transform>();

    private void Start()
    {
        for (int i = 0; i < startingPositions.Length; i++)
        {
            solarTransforms.Add(Instantiate(solarSystem, transform).transform);
            solarTransforms[i].position = startingPositions[i];
        }
    }

    private void Update()
    {
        CheckSolarPositions();
    }

    void CheckSolarPositions()
    {
        for (int i = 0; i < solarTransforms.Count; i++)
        {
            float distance = Vector3.Distance(solarTransforms[i].position, shipPos.position);
            if (distance > 300f)
            {
                int rnd = Random.Range(0, startingPositions.Length);
                solarTransforms[i].position = shipPos.position + startingPositions[rnd];
            }
        }
    }

    void DestroySystem(Transform systemTrans)
    {
        Destroy(systemTrans.gameObject);
    }
}

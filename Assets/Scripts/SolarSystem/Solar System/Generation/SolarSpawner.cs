using System.Collections.Generic;
using UnityEngine;


public class SolarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject solarSystem;

    [SerializeField] Transform shipPos;
    [SerializeField] private Vector3[] startingPositions = new Vector3[4];

    private List<SolarSystem> solarTransforms = new List<SolarSystem>();

    private void Start()
    {
        for (int i = 0; i < startingPositions.Length; i++)
        {
            solarTransforms.Add(Instantiate(solarSystem, transform).GetComponent<SolarSystem>());
            solarTransforms[i].transform.position = startingPositions[i];
        }
    }

    private void Update()
    {
        if (GameManager.GetState() == GameManager.GameState.Active)
            CheckSolarPositions();
    }

    void CheckSolarPositions()
    {
        for (int i = 0; i < solarTransforms.Count; i++)
        {
            float distance = Vector3.Distance(solarTransforms[i].transform.position, shipPos.position);
            if (distance > 300f)
            {
                int rnd = Random.Range(0, startingPositions.Length);
                solarTransforms[i].transform.position = shipPos.position + startingPositions[rnd];
                solarTransforms[i].ReplacePlanets();
            }
        }
    }
}

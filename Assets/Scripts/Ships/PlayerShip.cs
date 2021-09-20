using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerShip : ShipBase
{
    private KeyboardInputController keyboardController;
    private MouseInputController mouseController;
    
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject shipMesh;
    [SerializeField] private float alienSpawnTimer;
    
    
    private float startTime;

    protected override void SetDependencies()
    {
        base.SetDependencies();
        mouseController = gameObject.AddComponent<MouseInputController>();
        keyboardController = gameObject.AddComponent<KeyboardInputController>();
        SetControls();
        uiNav.PauseStateChanged += OnPauseStateChanged;
        startTime = Time.time;
    }

    protected override void HandleActions()
    {
        if (GameManager.GetState() == GameManager.GameState.Active)
        {
            Move();
            SpawnAliens();
            Shoot();
            return;
        }

        anim.SetFloat("rotation", 0);
        if (GameManager.GetState() == GameManager.GameState.WaitingForInput)
            if (activeController.fire)
            {
                GameManager.SetGameToActive();
            }
    }

    void SpawnAliens()
    {
        if (Time.time - startTime > alienSpawnTimer)
        {
            GameObject alienShip = ObjectPooler.GetAlienShip();
            int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            alienShip.transform.position = spawnPoints[randomSpawnPoint].transform.position;
            alienShip.SetActive(true);
            startTime = Time.time;
        }
    }

    void SetControls()
    {
        if (PlayerPrefs.GetInt("controlLayout", 0) == 0)
            activeController = mouseController;
        else
            activeController = keyboardController;
    }
    
    private void OnPauseStateChanged(object source, EventArgs e)
    {
        SetControls();
        GameManager.PauseGame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Bullet"))
        {
            GameObject obj = ObjectPooler.GetExplosionObj();
            obj.transform.position = transform.position;
            obj.SetActive(true);
            GameManager.EndGame();
            // play explosion
            shipMesh.SetActive(false);
            ParticleSystem[] particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem system in particleSystems)
            {
                system.Stop();
            }
        }
    }
}

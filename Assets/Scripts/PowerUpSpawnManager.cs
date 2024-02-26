using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _powerUps;

    private float _spawnTime = 10f;
    private bool _stopSpawning = false;

    private void Start()
    {
        StartCoroutine(SpawnPowerupRoutine());
    }

    private IEnumerator SpawnPowerupRoutine()
    {
        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, _powerUps.Length);

            Instantiate(_powerUps[randomPowerUp], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(_spawnTime);
        }
    }

    public void OnPlaneDestroyed()
    {
        _stopSpawning = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Fields
    #region IsSpawning Bools
    private bool _isSpawningEnemies = true;
    private bool _isSpawningPowerups = true;
    #endregion

    #region Spawn Container
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _powerupContainer;
    #endregion

    #region Prefabs
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerupPrefabs;
    #endregion
    #endregion

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyCoroutine());
        StartCoroutine(SpawnPowerupCoroutine());
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_isSpawningEnemies)
        {
            var randomX = Random.Range(-8f, 8f);
            Instantiate(_enemyPrefab, new Vector3(randomX, 7f, 0), Quaternion.identity, _enemyContainer.transform);
            yield return new WaitForSeconds(5);
        }
    }

    private IEnumerator SpawnPowerupCoroutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_isSpawningPowerups)
        {
            var spawnCooldown = Random.Range(3, 8);
            yield return new WaitForSeconds(spawnCooldown);

            var randomX = Random.Range(-8f, 8f);
            var randomPowerupIndex = Random.Range(0, _powerupPrefabs.Length);
            Instantiate(_powerupPrefabs[randomPowerupIndex], new Vector3(randomX, 7f, 0), Quaternion.identity, _powerupContainer.transform);
        }
    }

    public void OnPlayerDeath()
    {
        _isSpawningEnemies = false;
        _isSpawningPowerups = false;
    }
}

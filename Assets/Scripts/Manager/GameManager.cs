using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool IsAsteroidDestroyed { get; private set; }

    private bool _isGameOver = false;

    #region Manager References
    private SpawnManager _spawnManager;
    #endregion

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("SpawnManager is null");
    }

    void Update()
    {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void OnPlayerDeath()
    {
        _isGameOver = true;
    }

    /// <summary>
    /// Called when the asteroid is destroyed to
    /// start the game.
    /// </summary>
    public void StartGame()
    {
        _spawnManager.StartSpawning();
        IsAsteroidDestroyed = true;
    }
}

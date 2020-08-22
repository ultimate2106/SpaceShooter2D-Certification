using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private float _rotationSpeed = 20f;

    #region Manager References
    private AudioManager _audioManager;
    private SpawnManager _spawnManager;
    #endregion

    #region Prefabs
    [SerializeField]
    private GameObject _explosionPrefab;
    #endregion
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("SpawnManager ist null");

        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
            Debug.LogError("AudioManager is null");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back * _rotationSpeed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag.Equals("Laser"))
        {
            Destroy(other.gameObject);
            StartGame();
        }
    }

    private void StartGame()
    {
        GameObject explosion =  Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _audioManager.PlayExplosionSound();
        Destroy(explosion, 3.0f);
        _spawnManager.StartSpawning();
        Destroy(gameObject);
    }
}

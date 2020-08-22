﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private float _speed = 5;
    [SerializeField]
    private float _raisedSpeed = 10;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private float _canFire = -1f;

    #region UI related
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private GameObject _damageLeft;
    [SerializeField]
    private GameObject _damageRight;
    #endregion

    #region Manager References
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private GameManager _gameManager;
    private AudioManager _audioManager;
    #endregion

    #region Prefabs
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    #endregion

    #region PowerUp Controls
    private bool _isTripleShotActive = false;
    private bool _isSpeedUpActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shield;
    #endregion
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("SpawnManager is null");

        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (_uiManager == null)
            Debug.LogError("UIManager is null");

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
            Debug.LogError("GameManager is null");

        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
            Debug.LogError("AudioManager is null");
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Shoot();
        }
    }

    private void CalculateMovement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        var direction = new Vector3(horizontalInput, verticalInput, 0);
        if (_isSpeedUpActive)
        {
            transform.Translate(direction * _raisedSpeed * Time.deltaTime);
        } else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.y <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    private void Shoot()
    {
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        } else
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 0.8f, 0), Quaternion.identity);
        }

        _audioManager.PlayLaserSound();
    }

    public void AddScore(int pointsToAdd)
    {
        _score += pointsToAdd;
        _uiManager.UpdateScore(_score);
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _shield.SetActive(false);
            _isShieldActive = false;
        } else
        {
            --_lives;
            _uiManager.UpdateLives(_lives);
        }

        if (_lives == 2)
        {
            _damageLeft.SetActive(true);
        } else if (_lives == 1)
        {
            _damageRight.SetActive(true);
        }

        if(_lives < 1)
        {
            if (_spawnManager != null)
                _spawnManager.OnPlayerDeath();

            if (_uiManager != null)
                _uiManager.OnPlayerDeath();

            if (_gameManager != null)
                _gameManager.OnPlayerDeath();

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("EnemyLaser"))
        {
            Destroy(other.gameObject);
            Damage();
        }
    }

    #region Powerup Methods
    #region Triple Shot
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotCoroutine());
    }

    private IEnumerator TripleShotCoroutine()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }
    #endregion

    #region Speed
    public void SpeedUp()
    {
        _isSpeedUpActive = true;
        StartCoroutine(SpeedUpCoroutine());
    }

    private IEnumerator SpeedUpCoroutine()
    {
        yield return new WaitForSeconds(5);
        _isSpeedUpActive = false;
    }
    #endregion

    #region Shield
    public void ShieldActive()
    {
        _shield.SetActive(true);
        _isShieldActive = true;
    }
    #endregion
    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Fields
    #region Speed
    [SerializeField]
    private float _speed = 5;
    [SerializeField]
    private float _speedMultiplier = 2f;
    #endregion

    #region Shooting
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private int _maxAmmo = 15;
    #endregion

    #region UI related
    [SerializeField]
    private int _maxLives = 3;
    private int _lives;

    [SerializeField]
    private int _score = 0;

    private int _ammo;

    #region Damaged Engines
    private GameObject _damageLeft;
    private GameObject _damageRight;
    #endregion

    #region Shield
    private GameObject _shield;
    private Shield _shieldControl;
    #endregion
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
    #endregion
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        #region Get Manager References
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
        #endregion

        #region UI related
        #region Shield References
        _shield = gameObject.transform.Find("Shield").gameObject;
        if(_shield == null)
            Debug.LogError("Shield is null");

        _shieldControl = _shield.GetComponent<Shield>();
        if (_shieldControl == null)
            Debug.LogError("ShieldControl is null");
        #endregion

        #region Damaged Engines References
        _damageLeft = gameObject.transform.Find("DamageLeft").gameObject;
        if (_shield == null)
            Debug.LogError("DamageLeft is null");

        _damageRight = gameObject.transform.Find("DamageRight").gameObject;
        if (_damageRight == null)
            Debug.LogError("DamageRight is null");
        #endregion

        #region Init Text Visuals
        _ammo = _maxAmmo;
        _lives = _maxLives;
        _uiManager.UpdateScore(_score);
        _uiManager.UpdateAmmo(_ammo);
        _uiManager.UpdateLives(_lives);
        #endregion
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammo > 0)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed *= _speedMultiplier;
        } else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed /= _speedMultiplier;
        }
    }

    private void CalculateMovement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        var direction = new Vector3(horizontalInput, verticalInput, 0);
        if (_isSpeedUpActive)
        {
            transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);
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

        if (_gameManager.IsAsteroidDestroyed)
        {
            --_ammo;
            _uiManager.UpdateAmmo(_ammo);
        }
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
            _isShieldActive =  _shieldControl.DamageShield();
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

    private void OnTriggerEnter2D(Collider2D other)
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

    #region Ammo
    public void AddAmmo(int ammo)
    {
        if (ammo > _maxAmmo)
        {
            ammo = _maxAmmo - _ammo;
        }
        _ammo += ammo;
    }

    public int GetMaxAmmo()
    {
        return _maxAmmo;
    }
    #endregion

    #region Live
    public void AddLives(int lives)
    {
        if (lives > _maxLives)
        {
            lives = _maxLives - _lives;
        }
        _lives += lives;

        _uiManager.UpdateLives(_lives);
    }
    #endregion
    #endregion
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private float _speed = 4;

    [SerializeField]
    private float _minFirerate = 3.0f;
    [SerializeField]
    private float _maxFirerate = 7.0f;

    private bool _isDestroyed = false;

    #region Prefabs
    [SerializeField]
    private GameObject _laserPrefab;
    #endregion

    #region References
    private Player _player;
    private Animator _animator;

    #region Manager References
    private AudioManager _audioManager;
    #endregion
    #endregion
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region Get References
        _player = GameObject.FindWithTag("Player").transform.GetComponent<Player>();
        if (_player == null)
            Debug.LogError("Player is null");

        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
            Debug.LogError("AudioManager is null");

        _animator = GetComponent<Animator>();
        if (_animator == null)
            Debug.LogError("Enemy Animator is null");
        #endregion

        StartCoroutine(RandomShootCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5f)
        {
            var newX = Random.Range(-8f, 8f);
            transform.position = new Vector3(newX, 7f, 0);
        }
    }

    private IEnumerator RandomShootCoroutine()
    {
        while (!_isDestroyed)
        {
            float secondsBeforeShoot = Random.Range(_minFirerate, _maxFirerate);
            yield return new WaitForSeconds(secondsBeforeShoot);

            if (!_isDestroyed)
            {
                Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y - 1.5f, 0), Quaternion.Euler(0, 0, 180));
                _audioManager.PlayLaserSound();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            DestroyEnemy();
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            Destroy(gameObject, 2.8f);
        }

        if (other.tag.Equals("Laser"))
        {
            DestroyEnemy();
            Destroy(other.gameObject);
            Destroy(gameObject, 2.8f);
        }
    }

    private void DestroyEnemy()
    {
        _isDestroyed = true;

        _animator.SetTrigger("OnEnemyDestroy");

        Destroy(GetComponent<BoxCollider2D>());

        _speed = 0;
    }
}

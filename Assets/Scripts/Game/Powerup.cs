﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    enum PowerupType
    {
        TripleShot,
        MultiDirection,
        Shield,
        Speed,
        Ammo,
        Live
    }

    #region Fields
    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private PowerupType _powerupType = PowerupType.TripleShot;

    #region Manager References
    private AudioManager _audioManager;
    #endregion
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
            Debug.LogError("AudioManager is null");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupType)
                {
                    case PowerupType.TripleShot:
                        player.TripleShotActive();
                        break;
                    case PowerupType.Speed:
                        player.SpeedUp();
                        break;
                    case PowerupType.Shield:
                        player.ShieldActive();
                        break;
                    case PowerupType.Ammo:
                        player.AddAmmo(player.GetMaxAmmo());
                        break;
                    case PowerupType.Live:
                        player.AddLives(1);
                        break;
                    case PowerupType.MultiDirection:
                        player.MultiDirectionShotActive();
                        break;
                }
            }

            _audioManager.PlayPowerupSound();

            Destroy(gameObject);
        }
    }
}

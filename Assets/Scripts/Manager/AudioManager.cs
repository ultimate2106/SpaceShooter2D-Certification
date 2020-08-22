using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Fields
    [SerializeField]
    AudioSource _laserSound;
    [SerializeField]
    AudioSource _explosionSound;
    [SerializeField]
    AudioSource _powerupSound;
    #endregion

    public void PlayLaserSound()
    {
        _laserSound.Play();
    }

    public void PlayExplosionSound()
    {
        _explosionSound.Play();
    }

    public void PlayPowerupSound()
    {
        _powerupSound.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Fields
    #region Sprites
    [SerializeField]
    private Sprite[] _livesSprites;
    #endregion

    #region UI References
    #region Text
    [SerializeField]
    private GameObject _gameOverText;
    [SerializeField]
    private GameObject _restartText;
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _thrusterCharge;
    #endregion

    #region UI Elements
    [SerializeField]
    private Image _livesDisplayImage;
    #endregion
    #endregion
    #endregion

    public void UpdateScore(int currentScore)
    {
        _scoreText.text = "Score: " + currentScore;
    }

    public void UpdateLives(int currentLives)
    {
        if (!(currentLives >= _livesSprites.Length) || !(currentLives < 0))
        {
            _livesDisplayImage.sprite = _livesSprites[currentLives];
        }
    }

    public void UpdateAmmo(int currentAmmo)
    {
        _ammoText.text = "Ammo: " + currentAmmo;
    }

    public void UpdateThrusterCharge(int currentThrusterCharge)
    {
        _thrusterCharge.text = "Thrusters: " + currentThrusterCharge;
    }

    public void OnPlayerDeath()
    {
        _restartText.SetActive(true);
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        while (true)
        {
            _gameOverText.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            _gameOverText.SetActive(false);

            yield return new WaitForSeconds(0.5f);
        }
    }
}

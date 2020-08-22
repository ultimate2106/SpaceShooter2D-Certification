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
    [SerializeField]
    private GameObject _gameOverText;
    [SerializeField]
    private GameObject _restartText;
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesDisplayImage;
    #endregion
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
